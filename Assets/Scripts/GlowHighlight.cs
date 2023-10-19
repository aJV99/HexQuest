using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GlowHighlight : MonoBehaviour
{

    Dictionary<Renderer, Material[]> glowMaterialDictionary = new Dictionary<Renderer, Material[]>();
    Dictionary<Renderer, Material[]> originalMaterialDictionary = new Dictionary<Renderer, Material[]>();

    Dictionary<Color, Material> cachedGlowMaterials = new Dictionary<Color, Material>();

    public Material glowMaterial;

    private bool isGlowing = false;

    private void Awake()
    {
        PrepareMaterialDictionaries();
    }

    private void PrepareMaterialDictionaries()
    {
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            Material[] originMaterials = renderer.materials;
            originalMaterialDictionary.Add(renderer, originMaterials);

            Material[] newMaterials = new Material[renderer.materials.Length];
            for (int i = 0; i < originMaterials.Length; i++)
            {
                Material mat = null;
                if (cachedGlowMaterials.TryGetValue(originMaterials[i].color, out mat) == false)
                {
                    mat = new Material(glowMaterial);
                    mat.color= originMaterials[i].color;
                    cachedGlowMaterials[mat.color]= mat;
                }
                newMaterials[i] = mat;
            }
            glowMaterialDictionary.Add(renderer, newMaterials);
        }
    }

    public void ToggleGlow()
    {
        if (isGlowing == false)
        {
            foreach (Renderer renderer in originalMaterialDictionary.Keys)
            {
                renderer.materials = glowMaterialDictionary[renderer];
            }
        }
        else
        {
            foreach (Renderer renderer in originalMaterialDictionary.Keys)
            {
                renderer.materials = originalMaterialDictionary[renderer];
            }
        }
        isGlowing = !isGlowing;
    }

    public void ToggleGlow(bool state)
    {
        if (isGlowing == state)
        {
            return;
        }
        isGlowing = !state;
        ToggleGlow();
    }
}
