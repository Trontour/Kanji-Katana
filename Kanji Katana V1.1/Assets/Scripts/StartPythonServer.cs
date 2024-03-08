using System.Diagnostics;
using UnityEngine;

public class StartPythonServer : MonoBehaviour
{
    Process pythonServerProcess;

    void Start()
    {
        StartPythonServerProcess();
    }

    void StartPythonServerProcess()
    {
        pythonServerProcess = new Process();
        pythonServerProcess.StartInfo.FileName = "python"; // or "python3" depending on your setup
        pythonServerProcess.StartInfo.Arguments = "C:/Github/Python/manga_ocr_unity.py"; // Path to your Python server script
        pythonServerProcess.StartInfo.CreateNoWindow = true;
        pythonServerProcess.StartInfo.UseShellExecute = false;
        pythonServerProcess.Start();

        UnityEngine.Debug.Log("Python server started.");
    }

    void OnApplicationQuit()
    {
        if (!pythonServerProcess.HasExited)
        {
            pythonServerProcess.Kill();
            UnityEngine.Debug.Log("Python server stopped.");
        }
    }
}
