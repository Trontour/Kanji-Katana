using System.IO;
using System.Net.Sockets;
using System.Text;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class MangaOcrRun : MonoBehaviour
{
    public async Task<string> RequestOcrResultAsync(string imagePath)
    {
        Debug.Log("Starting Request");
        string responseString = "";
        try
        {
            int port = 65432;
            using (TcpClient client = new TcpClient("127.0.0.1", port))
            {
                Byte[] data = Encoding.UTF8.GetBytes(imagePath + "<EOF>"); // Mark the end of the file send
                using (NetworkStream stream = client.GetStream())
                {
                    await stream.WriteAsync(data, 0, data.Length);

                    var memoryStream = new MemoryStream();
                    byte[] buffer = new byte[2048]; // Larger buffer
                    int bytesRead;
                    Task<int> readTask;
                    var timeout = TimeSpan.FromSeconds(5); // 5-second timeout

                    do
                    {
                        readTask = stream.ReadAsync(buffer, 0, buffer.Length);
                        var completedTask = await Task.WhenAny(readTask, Task.Delay(timeout));
                        if (completedTask == readTask)
                        {
                            bytesRead = readTask.Result;
                            memoryStream.Write(buffer, 0, bytesRead);
                        }
                        else
                        {
                            throw new TimeoutException("The read operation timed out.");
                        }
                    }
                    while (readTask.Result > 0); // Continue reading until no more data

                    responseString = Encoding.UTF8.GetString(memoryStream.ToArray());
                    Debug.Log($"Received: {responseString}");
                }
            }
        }
        catch (Exception e) // Catching any exception
        {
            Debug.LogError($"Error: {e.Message}");
        }
        return responseString;
    }
}
