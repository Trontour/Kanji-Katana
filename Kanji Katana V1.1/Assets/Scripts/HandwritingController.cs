using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandwritingController : MonoBehaviour
{

    [SerializeField] private LineRenderer currentLineRenderer;

    private Vector3 mouseScreenPosition;
    private Vector3 mouseWorldPosition;
    private bool mousePressed, mouseHeldDown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        mousePressed = Input.GetMouseButtonDown(0);
        mouseHeldDown = Input.GetMouseButtonDown(1);
        if (mousePressed || mouseHeldDown)
        {
            mouseScreenPosition = Input.mousePosition;
            Debug.Log(mouseScreenPosition + " " + mouseWorldPosition);

            // Convert the mouse position to 3D world coordinates
            mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

            // Set Z to the desired depth, for example, 0 if your canvas is at Z = 0
            mouseWorldPosition.z = 0;
        }
        if (mousePressed)
        {
            // Start drawing
            StartDrawing(mouseWorldPosition);
        }

        if (mouseHeldDown) 
        {
            // Continue drawing
            UpdateDrawing(mouseWorldPosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            // Stop drawing
        }
    }
    private void StartDrawing(Vector3 position)
    {
        GameObject line = new GameObject("Line");
        currentLineRenderer = line.AddComponent<LineRenderer>();
        // Configure your LineRenderer here (width, color, material, etc.)
        currentLineRenderer.SetPosition(0, position);
        currentLineRenderer.SetPosition(1, position);
    }

    private void UpdateDrawing(Vector3 position)
    {
        if (currentLineRenderer != null)
        {
            // Add new points to the LineRenderer
            currentLineRenderer.positionCount++;
            currentLineRenderer.SetPosition(currentLineRenderer.positionCount - 1, position);
        }
    }
}
