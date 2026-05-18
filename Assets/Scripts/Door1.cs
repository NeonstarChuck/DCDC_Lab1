using Fusion;
using UnityEngine;

public class Door1 : NetworkBehaviour
{
    [Networked] public bool IsOpen { get; set; }

    private Collider col;
    private Renderer[] rends;

    void Awake()
    {
        col = GetComponent<Collider>();
        rends = GetComponentsInChildren<Renderer>();
    }

    public override void Render()
    {
        // If the network data says open, hide colliders and renderers on ALL headsets
        if (IsOpen)
        {
            if (col != null && col.enabled) col.enabled = false;
            foreach (var r in rends)
            {
                if (r != null && r.enabled) r.enabled = false;
            }
        }
    }
}