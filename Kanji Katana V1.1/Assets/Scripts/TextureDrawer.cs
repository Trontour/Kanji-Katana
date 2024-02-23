using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureDrawer : MonoBehaviour
{
    public Camera cam;
    public Texture2D drawingTexture; // Public variable to assign your texture
    public Renderer rend;
    public KeyCode saveKey = KeyCode.N;
    //public Material myMaterial;
    // Start is called before the first frame update
    void Start()
    {

        rend = GameObject.Find("SpellCanvas").GetComponent<Renderer>();
        
        
        if (!cam)
        {
            cam = Camera.main;
        }//if end

        // Optional: Create and assign the texture programmatically if not assigned
        if (drawingTexture == null)
        {
            int width = 128; // Example width
            int height = 128; // Example height
            drawingTexture = new Texture2D(width, height);
            // Initialize texture with a blank color (e.g., white)
            for (int y = 0; y < drawingTexture.height; y++)
            {
                for (int x = 0; x < drawingTexture.width; x++)
                {
                    drawingTexture.SetPixel(x, y, Color.white);
                }
            }
            drawingTexture.Apply();
        }
        rend.material.mainTexture = drawingTexture;
    }

    void Update()
    {

        if (Input.GetKeyDown(saveKey))
        {
            SaveTexture(drawingTexture);
            Debug.Log("Saved Texture");
            Color[] clearColors = new Color[drawingTexture.width * drawingTexture.height];
            for (int i = 0; i < clearColors.Length; i++)
            {
                clearColors[i] = Color.white;
            }
            drawingTexture.SetPixels(clearColors);
            drawingTexture.Apply();

        }
        if (!Input.GetMouseButton(0))
            return;

        RaycastHit hit;
        if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
            return;

        
        MeshCollider meshCollider = hit.collider as MeshCollider;
        //sssrend.material = myMaterial;
        if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
        {
            if (rend == null)
                Debug.Log("1");
            if (rend.sharedMaterial == null)
                Debug.Log("2");
            if (rend.sharedMaterial.mainTexture == null)
                Debug.Log("3");
            if (meshCollider == null)
                Debug.Log("4");
            Debug.Log("Condition Failed");
            return;

            

        }

        Vector2 pixelUV = hit.textureCoord;
        pixelUV.x *= drawingTexture.width;
        pixelUV.y *= drawingTexture.height;

        // Use the specified drawingTexture instead of the renderer's mainTexture
        drawingTexture.SetPixel((int)pixelUV.x, (int)pixelUV.y, Color.black);

        Debug.Log((int)pixelUV.x + " " + (int)pixelUV.y);
        drawingTexture.Apply();


       
    }


    private void SaveTexture(Texture2D texture)
    {
        byte[] bytes = texture.EncodeToPNG();
        var dirPath = Application.dataPath + "/RenderOutput";
        if (!System.IO.Directory.Exists(dirPath))
        {
            System.IO.Directory.CreateDirectory(dirPath);
        }
        System.IO.File.WriteAllBytes(dirPath + "/R_" + Random.Range(0, 100000) + ".png", bytes);
        Debug.Log(bytes.Length / 1024 + "Kb was saved as: " + dirPath);
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

}


