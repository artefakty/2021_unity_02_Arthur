using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public float TimeToLive;

    private void Start()
    { 
        Destroy(this.gameObject, TimeToLive);
    }

    public void SetColor(Color color)
    {
        foreach(var meshRenderer in GetComponentsInChildren<MeshRenderer>())
        {
            var property = new MaterialPropertyBlock();
            property.SetColor("_Color", color);
            meshRenderer.SetPropertyBlock(property);
        }
    }
}
