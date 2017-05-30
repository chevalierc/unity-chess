using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    public int x;
    public int y;
    public BoardManager boardManager;
    public Sprite lightSprite;
    public Sprite darkSprite;
    public Sprite selectedSprite;
    public Sprite lightOptionsSprite;
    public Sprite darkOptionsSprite;
    public bool isLight;

    private SpriteRenderer spriteRenderer;


    public void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        setUnSelected();
    }

    void OnMouseDown() {
        Debug.Log("yo");
        boardManager.onClick(x, y);
    }

    public void setAsOption() {
        if (isLight) {
            spriteRenderer.sprite = lightOptionsSprite;
        } else {
            spriteRenderer.sprite = darkOptionsSprite;
        }
    }

    public void setSelected() {
        spriteRenderer.sprite = selectedSprite;
    }

    public void setUnSelected() {
        if (isLight) {
            spriteRenderer.sprite = lightSprite;
        } else {
            spriteRenderer.sprite = darkSprite;
        }
    }


}
