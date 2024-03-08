using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public MangaOcrRun OCRscript;
    async void Start()
    {
        // Ensure mangaOcrRun is assigned, either via the Inspector or programmatically
        string imagePath = "path/to/your/image.jpg"; // Set the path to your image
        try
        {
            string ocrResult = await OCRscript.RequestOcrResultAsync(imagePath);
            Debug.Log($"OCR Result: {ocrResult}");
            // Now you can store the ocrResult string as needed
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"An error occurred: {ex.Message}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
