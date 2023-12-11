using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextOrientation : MonoBehaviour
{
    [Header("Text")]
    public TextMeshPro text;
    

    [Header("Location")]
    [SerializeField] Transform block;
    private Transform cam;
    // Start is called before the first frame update
    void Awake()
    {
        cam = GameObject.Find("State-Driven Camera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(cam);
        transform.rotation = Quaternion.LookRotation(transform.position - cam.position);
        transform.position = block.position;
    }
}
