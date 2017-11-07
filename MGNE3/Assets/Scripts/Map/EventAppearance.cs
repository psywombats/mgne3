﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EventAppearance : MonoBehaviour {

    public void Start() {
        GetComponent<Dispatch>().RegisterListener(MapEvent.EventEnabled, (object payload) => {
            bool enabled = (bool)payload;
            GetComponent<SpriteRenderer>().enabled = enabled;
        });
    }

    public void SetAppearance(string spriteName) {
        Sprite sprite;
        if (Application.isEditor) {
            sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Resources/Sprites/" + spriteName + ".png");
        } else {
            sprite = Resources.Load<Sprite>("Sprites/" + spriteName);
        }
        
        SetAppearance(sprite);
    }

    public void SetAppearance(Sprite sprite) {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
