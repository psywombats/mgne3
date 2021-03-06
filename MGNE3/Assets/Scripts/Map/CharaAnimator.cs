﻿using System.Collections;
using System.Collections.Generic;
using Tiled2Unity;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class CharaAnimator : MonoBehaviour {

    public bool AlwaysAnimates = false;

    private Vector2 lastPosition;

    public void Start() {
        lastPosition = gameObject.transform.position;

        if (GetComponent<CharaEvent>() != null) {
            GetComponent<Dispatch>().RegisterListener(MapEvent.EventEnabled, (object payload) => {
                bool enabled = (bool)payload;
                GetComponent<SpriteRenderer>().enabled = enabled;
            });
        }
    }

    public void Update() {
        if (GetComponent<CharaEvent>() != null) {
            Vector2 position = gameObject.transform.position;
            Vector2 delta = position - lastPosition;

            bool stepping = AlwaysAnimates || delta.sqrMagnitude > 0 || GetComponent<CharaEvent>().Tracking;
            GetComponent<Animator>().SetBool("stepping", stepping);
            GetComponent<Animator>().SetInteger("dir", GetComponent<CharaEvent>().Facing.Ordinal());

            lastPosition = position;
        } else {
            GetComponent<Animator>().SetBool("stepping", AlwaysAnimates);
            GetComponent<Animator>().SetInteger("dir", OrthoDir.South.Ordinal());
        }
    }

    public void Populate(string spriteName) {
        string controllerPath = "Animations/Charas/Instances/" + spriteName;
        RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>(controllerPath);
        GetComponent<Animator>().runtimeAnimatorController = controller;

        string spritePath = "Sprites/Charas/" + spriteName;
        Sprite[] sprites = Resources.LoadAll<Sprite>(spritePath);
        foreach (Sprite sprite in sprites) {
            if (sprite.name == spriteName + GetComponent<CharaEvent>().Facing.DirectionName() + "Center") {
                GetComponent<SpriteRenderer>().sprite = sprite;
                break;
            }
        }
    }

    private void UpdatePositionMemory() {
        lastPosition.x = gameObject.transform.position.x;
        lastPosition.y = gameObject.transform.position.y;
    }
}
