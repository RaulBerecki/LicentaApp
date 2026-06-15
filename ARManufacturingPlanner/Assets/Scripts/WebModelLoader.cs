using UnityEngine;
using GLTFast;
using System.Threading.Tasks;
using System.Text; // Required for StringBuilder
using UnityEngine.XR.Interaction.Toolkit.Interactables; // XRGrabInteractable
using UnityEngine.XR.Interaction.Toolkit.Transformers;  // ARTransformer

public class RuntimeGltfLoader : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("The URL or local path to the .glb file.")]
    public string modelUrl = "";

    [Tooltip("If on, the model loads automatically in Start (SceneNewPlacement behaviour). " +
             "Turn OFF in SampleScene so it only loads when the menu button is pressed.")]
    public bool loadOnStart = true;

    [Tooltip("Distance (meters) in front of the camera used when loadOnStart is on.")]
    public float spawnDistance = 1.5f;

    [Header("Hologram")]
    [Tooltip("Hologram shader used for the placement preview. Assign Assets/Shaders/Hologram.shader. " +
             "Falls back to Shader.Find(\"Custom/HologramURP\") if left empty.")]
    public Shader hologramShader;

    [Header("Debug Settings")]
    [Tooltip("Show the on-screen debug log overlay.")]
    public bool showDebugOverlay = true;
    [Tooltip("Font size for the on-screen debug log.")]
    public int fontSize = 35;

    // Using StringBuilder is more efficient than string concatenation for logs
    private StringBuilder _logBuilder = new StringBuilder();
    private Vector2 _scrollPosition;
    private GUIStyle _debugTextStyle;
    private bool _isLoading;
    private Material _hologramMaterial;

    private void Start()
    {
        Log("Status: Ready.");
        if (loadOnStart)
        {
            GetSpawnPose(out Vector3 position, out Quaternion rotation);
            _ = LoadDirect(modelUrl, position, rotation, DeriveName(modelUrl));
        }
    }

    /// UI button entry point — downloads the model and enters ground-placement mode
    /// (hologram preview follows the floor; the place panel handles the final tap).
    public void LoadModel()
    {
        _ = LoadForPlacement(modelUrl, DeriveName(modelUrl));
    }

    /// Re-creates a previously saved web model directly at its stored pose (SaveManager.LoadScene).
    public void LoadSavedModel(string url, Vector3 position, Quaternion rotation, string objectName)
    {
        _ = LoadDirect(url, position, rotation, objectName);
    }

    // ---------------------------------------------------------------- placement flow

    private async Task LoadForPlacement(string url, string objectName)
    {
        if (_isLoading) { Log("Status: A model is already loading, please wait..."); return; }
        if (string.IsNullOrEmpty(url)) { Log("Error: Model URL is empty."); ReshowMenu(); return; }

        _isLoading = true;
        Log($"Status: Downloading model from {url}...");

        GameObject template = await DownloadModel(url, objectName);
        if (template != null)
        {
            AddWebScript(template, url, objectName);

            // Build the hologram from the plain model BEFORE adding interaction components,
            // so the ghost preview has no collider/physics/interactable.
            GameObject hologram = BuildHologram(template);

            MakeInteractable(template);
            template.SetActive(false); // hidden source; ClickToPlace clones + reactivates it

            if (SetupPlacementUI(template, hologram))
                Log("Status: Model ready. Aim at the floor and tap Place.");
            else
                Log("Error: Placement components (PlaceManager/PlaceIndicator/Manager) not found.");

            AnalyzeLoadedContent(template.transform);
        }
        else
        {
            ReshowMenu();
        }

        _isLoading = false;
    }

    /// Sets the template as the object-to-place and attaches the hologram to the indicator.
    private bool SetupPlacementUI(GameObject template, GameObject hologram)
    {
        PlaceManager placeManager = FindAnyObjectByType<PlaceManager>();
        PlaceIndicator placeIndicator = FindAnyObjectByType<PlaceIndicator>();
        UserInterfaceManager ui = GetUserInterfaceManager();
        if (placeManager == null || placeIndicator == null || ui == null)
            return false;

        placeManager.objectToPlace = template;
        placeIndicator.SetObjectInstance(hologram);
        hologram.SetActive(true);
        ui.placePanel.SetActive(true);
        return true;
    }

    // ---------------------------------------------------------------- direct flow

    private async Task LoadDirect(string url, Vector3 position, Quaternion rotation, string objectName)
    {
        if (_isLoading) { Log("Status: A model is already loading, please wait..."); return; }
        if (string.IsNullOrEmpty(url)) { Log("Error: Model URL is empty."); return; }

        _isLoading = true;
        Log($"Status: Downloading model from {url}...");

        GameObject modelRoot = await DownloadModel(url, objectName);
        if (modelRoot != null)
        {
            modelRoot.transform.SetPositionAndRotation(position, rotation);
            AddWebScript(modelRoot, url, objectName);
            MakeInteractable(modelRoot);
            RegisterWithManager(modelRoot);

            Log("Status: Model instantiated successfully.");
            AnalyzeLoadedContent(modelRoot.transform);
        }

        _isLoading = false;
    }

    // ---------------------------------------------------------------- shared helpers

    /// Downloads + instantiates the GLB under a new root GameObject. Returns null on failure.
    private async Task<GameObject> DownloadModel(string url, string rootName)
    {
        var gltfImport = new GltfImport();
        var importSettings = new ImportSettings
        {
            GenerateMipMaps = true,       // Prevents texture shimmering at a distance
            AnisotropicFilterLevel = 4    // Improves texture clarity at oblique angles
        };

        bool success = await gltfImport.Load(url, importSettings);
        if (!success)
        {
            Log("Error: Failed to load or parse the glTF file.");
            return null;
        }

        GameObject root = new GameObject(string.IsNullOrEmpty(rootName) ? "WebModel" : rootName);
        await gltfImport.InstantiateMainSceneAsync(root.transform);
        return root;
    }

    /// Tags a loaded model with its source URL so it shows in the object list and saves by URL.
    private void AddWebScript(GameObject root, string url, string objectName)
    {
        ObjectUniversalScript ous = root.AddComponent<ObjectUniversalScript>();
        ous.objectName = string.IsNullOrEmpty(objectName) ? "WebModel" : objectName;
        ous.webUrl = url;
        ous.grippers = new GameObject[0]; // defensive: keep the array non-null
    }

    private void RegisterWithManager(GameObject root)
    {
        GameObject manager = GameObject.FindGameObjectWithTag("Manager");
        if (manager != null)
        {
            IdManager idManager = manager.GetComponent<IdManager>();
            if (idManager != null)
            {
                idManager.RegisterObject(root);
                return;
            }
        }
        Log("Info: No IdManager found - model spawned standalone (not added to the list).");
    }

    /// Adds the same drag/rotate stack the built-in prefabs use: a bounds-sized BoxCollider,
    /// a kinematic Rigidbody, an XRGrabInteractable and an ARTransformer (one-finger move on the
    /// plane, two-finger rotate/scale). Mirrors the FinalPositions prefab setup.
    private void MakeInteractable(GameObject root)
    {
        if (TryGetWorldBounds(root, out Bounds worldBounds))
        {
            BoxCollider box = root.AddComponent<BoxCollider>();
            // root has identity rotation / unit scale at this point, so world size == local size.
            box.center = root.transform.InverseTransformPoint(worldBounds.center);
            box.size = worldBounds.size;
        }

        Rigidbody rb = root.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        // XRGrabInteractable auto-collects child colliders (the BoxCollider above) when its
        // own collider list is empty, matching the built-in prefabs.
        root.AddComponent<XRGrabInteractable>();

        ARTransformer transformer = root.AddComponent<ARTransformer>();
        // Lock scale (min == max) so web objects can only be moved and rotated - not scaled -
        // matching the local objects. With a zero scale range the pinch gesture is ignored.
        transformer.minScale = 1f;
        transformer.maxScale = 1f;
    }

    /// Combined world-space bounds of all child renderers. Returns false if there are none.
    private bool TryGetWorldBounds(GameObject root, out Bounds bounds)
    {
        Renderer[] renderers = root.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            bounds = new Bounds(root.transform.position, Vector3.one * 0.1f);
            return false;
        }
        bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
            bounds.Encapsulate(renderers[i].bounds);
        return true;
    }

    /// Clones the model and swaps every material for the hologram material to make a ghost preview.
    private GameObject BuildHologram(GameObject source)
    {
        GameObject hologram = Instantiate(source);
        hologram.name = "WebModelHologram";
        hologram.SetActive(true);

        // The clone carries the source's ObjectUniversalScript - the preview doesn't need it.
        ObjectUniversalScript clonedScript = hologram.GetComponent<ObjectUniversalScript>();
        if (clonedScript != null)
            Destroy(clonedScript);

        Material holoMat = GetHologramMaterial();
        foreach (Renderer renderer in hologram.GetComponentsInChildren<Renderer>(true))
        {
            var mats = new Material[renderer.sharedMaterials.Length];
            for (int i = 0; i < mats.Length; i++)
                mats[i] = holoMat;
            renderer.sharedMaterials = mats;
        }
        return hologram;
    }

    private Material GetHologramMaterial()
    {
        if (_hologramMaterial != null)
            return _hologramMaterial;

        Shader shader = hologramShader != null ? hologramShader : Shader.Find("Custom/HologramURP");
        if (shader == null)
        {
            Log("Warning: Hologram shader not found; using URP/Unlit fallback.");
            shader = Shader.Find("Universal Render Pipeline/Unlit");
        }
        _hologramMaterial = new Material(shader);
        return _hologramMaterial;
    }

    private void ReshowMenu()
    {
        UserInterfaceManager ui = GetUserInterfaceManager();
        if (ui != null && ui.BackgroundPanelActive != null)
            ui.BackgroundPanelActive.SetActive(true);
    }

    private UserInterfaceManager GetUserInterfaceManager()
    {
        GameObject manager = GameObject.FindGameObjectWithTag("Manager");
        return manager != null ? manager.GetComponent<UserInterfaceManager>() : null;
    }

    /// Computes a pose a set distance in front of the main camera, facing it (yaw only).
    private void GetSpawnPose(out Vector3 position, out Quaternion rotation)
    {
        Camera cam = Camera.main;
        if (cam != null)
        {
            position = cam.transform.position + cam.transform.forward * spawnDistance;
            Vector3 toCamera = cam.transform.position - position;
            toCamera.y = 0f;
            rotation = toCamera.sqrMagnitude > 0.0001f ? Quaternion.LookRotation(toCamera) : Quaternion.identity;
        }
        else
        {
            position = transform.position;
            rotation = transform.rotation;
        }
    }

    /// Derives a friendly object name from a model URL (the file name without extension).
    private static string DeriveName(string url)
    {
        if (string.IsNullOrEmpty(url))
            return "WebModel";
        string name = System.IO.Path.GetFileNameWithoutExtension(url);
        return string.IsNullOrEmpty(name) ? "WebModel" : name;
    }

    /// Inspects the instantiated model to verify if shaders and textures are correct.
    private void AnalyzeLoadedContent(Transform root)
    {
        var renderers = root.GetComponentsInChildren<Renderer>(true);

        if (renderers.Length == 0)
        {
            Log("Warning: The loaded model has no Renderers (Meshes).");
            return;
        }

        Log($"Info: Found {renderers.Length} mesh objects.");

        foreach (var renderer in renderers)
        {
            foreach (var material in renderer.materials)
            {
                ValidateMaterial(material);
            }
        }
    }

    /// Checks a specific material for valid shaders and textures.
    private void ValidateMaterial(Material mat)
    {
        string shaderName = mat.shader.name;
        string status = "";

        // Check for valid URP or GLTF shaders
        if (shaderName.Contains("glTF") || shaderName.Contains("Universal Render Pipeline"))
        {
            status = $"[OK] Shader: {shaderName}";
        }
        // Check for the dreaded "Pink" error shader
        else if (shaderName.Contains("Error") || shaderName.Contains("Internal") || shaderName.Contains("Magenta"))
        {
            status = $"[ERROR] Invalid Shader: {shaderName}";
        }
        else
        {
            status = $"[INFO] Shader: {shaderName}";
        }

        // Check for texture presence in common properties
        Texture texture = null;
        if (mat.HasProperty("_BaseMap")) texture = mat.GetTexture("_BaseMap");           // URP
        else if (mat.HasProperty("baseColorTexture")) texture = mat.GetTexture("baseColorTexture"); // GLTF
        else if (mat.HasProperty("_MainTex")) texture = mat.GetTexture("_MainTex");      // Built-in

        if (texture != null)
        {
            status += $" | Texture: Found ({texture.width}x{texture.height})";
        }
        else
        {
            status += " | Texture: MISSING or Solid Color";
        }

        Log(status);
    }

    /// Helper method to append messages to the on-screen log.
    private void Log(string message)
    {
        _logBuilder.AppendLine(message);
        // Also log to the Unity Console for Editor debugging
        Debug.Log(message);
    }

    /// Renders the debug information on the device screen.
    private void OnGUI()
    {
        if (!showDebugOverlay)
            return;

        // Initialize GUIStyle once to avoid garbage collection overhead per frame
        if (_debugTextStyle == null)
        {
            _debugTextStyle = new GUIStyle();
            _debugTextStyle.fontSize = fontSize;
            _debugTextStyle.normal.textColor = Color.yellow;
            _debugTextStyle.wordWrap = true;
        }

        float width = Screen.width;
        float height = Screen.height;
        float margin = 20f;

        // Create a layout area with margins
        GUILayout.BeginArea(new Rect(margin, margin * 2, width - (margin * 2), height - (margin * 4)));

        // Create a scroll view for long logs
        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);

        GUILayout.Label(_logBuilder.ToString(), _debugTextStyle);

        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }
}
