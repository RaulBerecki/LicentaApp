using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SupabaseConsoleApp
{
    class Program
    {
        // Supabase settings
        private static readonly string supabaseUrl = "https://ftfanfreufswyjzubhdc.supabase.co";
        private static readonly string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImZ0ZmFuZnJldWZzd3lqenViaGRjIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDg5NTI2MjAsImV4cCI6MjA2NDUyODYyMH0.8_YWBEAI37j0Gssu6ieT76lkq4Z3d5qgMC0A4bc4j78"; // Replace with your key
        private static readonly string tableName = "generateObject";
        //Blender
        private static readonly string blenderPath = @"C:\Program Files\Blender Foundation\Blender 4.2\blender.exe";
        private static readonly string blendFile = @"D:\UnityProjects\LicentaApp\ObjectIntegratorWithAI\AIExporter\untitled.blend";
        private static readonly string pythonScriptPath = @"D:\UnityProjects\LicentaApp\ObjectIntegratorWithAI\AIExporter\script.py";
        private static readonly string outputFBXPath = @"D:\UnityProjects\LicentaApp\ObjectIntegratorWithAI\AIExporter\exported_model.fbx";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Supabase Polling Started...");

            while (true)
            {
                await CheckForPendingPythonCode();
                await Task.Delay(1000); // Check every second
            }
        }

        private static async Task CheckForPendingPythonCode()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("apikey", supabaseKey);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + supabaseKey);
                string url = $"{supabaseUrl}/rest/v1/{tableName}?status=eq.pending&limit=1&order=id.asc";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    JArray data = JArray.Parse(jsonResponse);
                    if (data.Count > 0)
                    {
                        var record = data[0];
                        long id = record.Value<long>("id");
                        string code = record.Value<string>("code");
                        Console.WriteLine($"Python Code Received:\n{code}\n");
                        // Update status to 'processing'
                        await UpdateStatus(id, "processing");
                        // Save the code to script.py
                        File.WriteAllText(pythonScriptPath, code);
                        // Run Blender process
                        bool success = await RunBlenderProcess();
                        // Update status based on success
                        if (success)
                        {
                            await UpdateStatus(id, "done");
                        }
                        else
                        {
                            await UpdateStatus(id, "error");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No pending tasks...");
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
        }
        private static async Task<bool> RunBlenderProcess()
        {
            try
            {
                string arguments = $"\"{blendFile}\" --background --python \"{pythonScriptPath}\" -- \"{outputFBXPath}\"";

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = blenderPath,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(psi))
                {
                    string output = await process.StandardOutput.ReadToEndAsync();
                    string error = await process.StandardError.ReadToEndAsync();
                    process.WaitForExit();

                    Console.WriteLine("Blender Output:");
                    Console.WriteLine(output);

                    if (!string.IsNullOrEmpty(error))
                    {
                        Console.WriteLine("Blender Errors:");
                        Console.WriteLine(error);
                    }

                    return process.ExitCode == 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error running Blender: {ex.Message}");
                return false;
            }
        }
        private static async Task UpdateStatus(long id, string newStatus)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("apikey", supabaseKey);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + supabaseKey);
                client.DefaultRequestHeaders.Add("Prefer", "return=representation"); // Important header
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                string url = $"{supabaseUrl}/rest/v1/{tableName}?id=eq.{id}";

                string json = $"{{\"status\": \"{newStatus}\"}}";
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                Console.WriteLine(content);
                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), url);
                request.Content = content;

                HttpResponseMessage response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Status updated to '{newStatus}' for ID: {id}");
                }
                else
                {
                    Console.WriteLine($"Failed to update status: {response.StatusCode}");
                }
            }
        }
    }
}
