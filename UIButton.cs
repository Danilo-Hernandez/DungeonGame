using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
	public Text nameText, descText;
	public string nameString, descString;
	public Sprite imgSprite, bckSprite;
	public Image img, bckImg;
	public float scale = 1;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ChangeUI);
    }

    void ChangeUI() {
    	img.sprite = imgSprite;
    	nameText.text = nameString;
    	descText.text = descString;
    	bckImg.sprite = bckSprite;
    	img.transform.localScale = new Vector3(scale, scale, 1);
    }
}
