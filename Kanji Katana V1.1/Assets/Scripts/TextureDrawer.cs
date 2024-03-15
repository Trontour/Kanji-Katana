using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TextureDrawer : MonoBehaviour
{
    [Header("REFERENCES")]
    public Camera cam;
    public Texture2D drawingTexture;
    public Renderer rend;
    public KeyCode saveKey = KeyCode.N;
    public LayerMask ignoreLayers;
    public MangaOcrRun OCRSCript;

    [Header("BRUSH SETTINGS")]
    public Color brushColor = Color.black; // Default brush color
    public int brushSize = 1; // Default brush size

    [Header("JAPANESE HANDLING")]
    private Vector2? lastPixelUV = null;
    private string currentImgPath;
    public string currentJapaneseOutput;
    private bool isRequesting = false;
    private bool isRequestingTexture = false;
    private bool resultReady = false;
    private bool textureReady = false;
    private int numStrokes = 0;

    private void Awake()
    {
        currentJapaneseOutput = string.Empty; 
    }
    void Start()
    {

        int layer1 = LayerMask.NameToLayer("whatIsPlayer");
        int layer2 = LayerMask.NameToLayer("whatIsGround");

        ignoreLayers = (1 << layer1) | (1 << layer2);
        string directoryPath = Application.dataPath + "/RenderOutput";

        DeleteAllPngFiles();
        rend = GameObject.Find("SpellCanvas").GetComponent<Renderer>();

        if (!cam)
        {
            cam = Camera.main;
        }

        if (drawingTexture == null)
        {
            int width = 128;
            int height = 128;
            drawingTexture = new Texture2D(width, height);
            FillTexture(Color.white);
        }

        rend.material.mainTexture = drawingTexture;
    }

    void Update()
    {

        if (Input.GetKeyDown(saveKey) && !isRequesting && !isRequestingTexture)
        {
            // Assuming SaveTexture now correctly manages isRequestingTexture flag
            SaveTexture(drawingTexture);
            isRequestingTexture = true; // This should be the only place you set it true
        }

        if (textureReady && !isRequesting)
        {
            Debug.Log("Starting OCR request...");
            StartOcrRequest(currentImgPath); // Ensure this uses the current image path
            //StartOcrRequest("C:/Github/Python/maga.jpg");
            textureReady = false; // Reset textureReady after starting OCR request
            isRequestingTexture = false;
            isRequesting = true;  // Indicate OCR request is in progress
            FillTexture(Color.white);
        }

        if (resultReady)
        {
            Debug.Log($"OCR Result: {currentJapaneseOutput}");
            resultReady = false; // Reset resultReady after handling the result
            isRequesting = false; // Reset isRequesting to allow new operations
        }

        if (!Input.GetMouseButton(0)) 
        {
            lastPixelUV = null;
            return;
        }

        RaycastHit hit;
        if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, ~ignoreLayers))
        {
            lastPixelUV = null;
            return;
        }

        if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || !(hit.collider is MeshCollider))
        {
            Debug.Log("BRAKK");
            lastPixelUV = null;
            return;
        }

        Vector2 pixelUV = hit.textureCoord;
        pixelUV.x *= drawingTexture.width;
        pixelUV.y *= drawingTexture.height;

        if (lastPixelUV.HasValue)
        {
            DrawLine(lastPixelUV.Value, pixelUV, brushColor, false);
        }
        else
        {
            SetPixel((int)pixelUV.x, (int)pixelUV.y, brushColor);
        }

        lastPixelUV = pixelUV;


    }

    void LateUpdate()
    {
        // Apply the texture changes at the end of the frame to batch drawing updates.
        drawingTexture.Apply();
    }
    async void StartOcrRequest(string imagePath)
    {
        currentJapaneseOutput = await OCRSCript.RequestOcrResultAsync(imagePath);
        resultReady = true;
    }
    void FillTexture(Color color)
    {
        Color[] clearColors = new Color[drawingTexture.width * drawingTexture.height];
        for (int i = 0; i < clearColors.Length; i++)
        {
            clearColors[i] = color;
        }
        drawingTexture.SetPixels(clearColors);
        drawingTexture.Apply();
    }

    void SetPixel(int x, int y, Color color)
    {
        for (int i = 0; i < brushSize; i++)
        {
            for (int j = 0; j < brushSize; j++)
            {
                int drawX = x + i - brushSize / 2;
                int drawY = y + j - brushSize / 2;
                if (drawX >= 0 && drawX < drawingTexture.width && drawY >= 0 && drawY < drawingTexture.height)
                {
                    drawingTexture.SetPixel(drawX, drawY, color);
                }
            }
        }
    }

    void DrawLine(Vector2 from, Vector2 to, Color color, bool applyImmediately)
    {
        // Implementation of the Bresenham line algorithm modified to use brush size and color
        int x0 = (int)from.x;
        int y0 = (int)from.y;
        int x1 = (int)to.x;
        int y1 = (int)to.y;

        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            SetPixel(x0, y0, color); // Use SetPixel to account for brush size

            if (x0 == x1 && y0 == y1) break;

            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }

        if (applyImmediately)
        {
            drawingTexture.Apply();
        }
    }
    //void DrawLine2(Vector2 from, Vector2 to, Color color, bool applyImmediately)
    //{
    //    float dist = Vector2.Distance(from, to);
    //    // The number of points depends on the distance.
    //    int points = Mathf.CeilToInt(dist / 0.5f); // Example interpolation step of 0.5 units

    //    for (int i = 0; i <= points; i++)
    //    {
    //        float t = (float)i / points;
    //        Vector2 interpolatedPoint = Vector2.Lerp(from, to, t);
    //        SetPixel((int)interpolatedPoint.x, (int)interpolatedPoint.y, color);
    //    }

    //    if (applyImmediately)
    //    {
    //        drawingTexture.Apply();
    //    }
    //}

    private async void SaveTexture(Texture2D texture)
    {
        byte[] bytes = texture.EncodeToPNG();
        var dirPath = Application.dataPath + "/RenderOutput";
        if (!System.IO.Directory.Exists(dirPath))
        {
            System.IO.Directory.CreateDirectory(dirPath);
        }
        string finalPath = dirPath + "/R_" + System.DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".png";
        await Task.Run(() => System.IO.File.WriteAllBytes(finalPath, bytes));
        textureReady = true;
        Debug.Log(bytes.Length / 1024 + "Kb was saved as: " + dirPath);
        currentImgPath = finalPath;
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif

    }

    private void DeleteAllPngFiles()
    {
        // Fixed path to the RenderOutput directory
        string directoryPath = Path.Combine(Application.dataPath, "RenderOutput");

        if (Directory.Exists(directoryPath))
        {
            string[] files = Directory.GetFiles(directoryPath, "*.png");
            foreach (string file in files)
            {
                DeleteFile(file);
            }
        }
    }

    private void DeleteFile(string filePath)
    {
#if UNITY_EDITOR
        // Use AssetDatabase to delete assets if running in the editor
        string relativePath = "Assets" + filePath.Replace(Application.dataPath, "").Replace('\\', '/');
        bool fileExists = File.Exists(filePath);
        if (fileExists && AssetDatabase.DeleteAsset(relativePath))
        {
            Debug.Log($"Deleted {relativePath} via AssetDatabase.");
        }
        else if (fileExists)
        {
            // If AssetDatabase.DeleteAsset fails, fallback to manual deletion
            File.Delete(filePath);
            Debug.Log($"Deleted {filePath} manually.");
        }
#else
        // If running in a standalone build, directly delete the file
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log($"Deleted {filePath} manually.");
        }
        // Attempt to delete the .meta file as well, if it exists
        string metaFilePath = $"{filePath}.meta";
        if (File.Exists(metaFilePath))
        {
            File.Delete(metaFilePath);
            Debug.Log($"Deleted {metaFilePath} manually.");
        }
#endif
    }
}


