using UnityEngine;
using OpenAI;
using System.Collections.Generic;
using TMPro;
using static UnityEngine.Mesh;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Collections;

public class ChatGPTManager : MonoBehaviour
{
    string supabaseUrl = "https://ftfanfreufswyjzubhdc.supabase.co/rest/v1/generateObject";
    string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImZ0ZmFuZnJldWZzd3lqenViaGRjIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDg5NTI2MjAsImV4cCI6MjA2NDUyODYyMH0.8_YWBEAI37j0Gssu6ieT76lkq4Z3d5qgMC0A4bc4j78";

    private OpenAIApi openAI = new OpenAIApi();

    private List<ChatMessage> messages = new List<ChatMessage>();

    public TMP_InputField input;
    public async void AskChatGPT()
    {
        string prompt = $"Write only a python script for Blender that create a" +
            $"{input.text}" +
            $", applies colors to it and export it as fbx where the path is \"D:\"UnityProjects\"LicentaApp\"ObjectIntegratorWithAI\"AIExporter\""+
            $"no explanations, no extra text";
        input.text = ""; // Reset input
        Debug.Log(prompt);
        ChatMessage newMessage = new ChatMessage();
        newMessage.Content = prompt;
        newMessage.Role = "user";
        messages.Add(newMessage);
        CreateChatCompletionRequest request = new CreateChatCompletionRequest();
        request.Messages = messages;
        request.Model = "gpt-3.5-turbo";

        var response = await openAI.CreateChatCompletion(request);

        if(response.Choices.Count > 0 && response.Choices != null)
        {
            var chatResponse = response.Choices[0].Message;
            messages.Add(chatResponse);
            Debug.Log(chatResponse.Content);
            StartCoroutine(SendInputToDb(chatResponse.Content));
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator SendInputToDb(string code)
    {
        InputAI newInput = new InputAI(code);
        string json = JsonUtility.ToJson(newInput);
        UnityWebRequest req = new UnityWebRequest(supabaseUrl, "POST");
        Debug.Log(json);
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(json);
        req.uploadHandler = new UploadHandlerRaw(bodyRaw);
        req.downloadHandler = new DownloadHandlerBuffer();

        req.SetRequestHeader("Content-Type", "application/json");
        req.SetRequestHeader("apikey", supabaseKey);
        req.SetRequestHeader("Authorization", "Bearer " + supabaseKey);
        req.SetRequestHeader("Prefer", "return=representation");

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Input send: " + req.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Input failed: " + req.error);
        }
    }
}
