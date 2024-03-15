using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextOrientation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerBattle playerBattle;
    private TextureDrawer textureDrawer;
    public GameObject parentBlock;
    //private string playerCharacter;

    [Header("Text")]
    public HiraganaObject currentHiragana;
    public TextMeshPro text;
    

    [Header("Location")]
    [SerializeField] Transform block;
    private Transform cam;
    // Start is called before the first frame update
    void Awake()
    {
        textureDrawer = GameObject.Find("SpellHandler").GetComponent<TextureDrawer>();
        cam = GameObject.Find("State-Driven Camera").transform;
        playerBattle = GameObject.Find("Player").GetComponent<PlayerBattle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (parentBlock != null && playerCountered() ) 
        {
            textureDrawer.currentJapaneseOutput = "";
            Destroy(parentBlock);
        }
        //transform.LookAt(cam);
        transform.rotation = Quaternion.LookRotation(transform.position - cam.position);
        transform.position = block.position;
        if(playerBattle.goggleMode)
            text.text = currentHiragana.hiragana;
        else
            text.text = currentHiragana.romaji;
        //Debug.Log(currentText);


    }

    private bool playerCountered()
    {
        return textureDrawer.currentJapaneseOutput.Contains(currentHiragana.hiragana);
    }
}
