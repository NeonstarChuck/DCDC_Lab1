using Fusion;
using UnityEngine;

public class RoomProgressManager : NetworkBehaviour
{
    public Door1 leftDoor;
    public Door1 rightDoor;

    [Networked] public bool ColorSolved { get; set; }
    [Networked] public bool KeySolved { get; set; }

    private ChangeDetector _changes;

    public override void Spawned()
    {
        _changes = GetChangeDetector(ChangeDetector.Source.SimulationState);
        CheckCompletion();
    }

    public override void Render()
    {
        foreach (var change in _changes.DetectChanges(this))
        {
            switch (change)
            {
                case nameof(ColorSolved):
                case nameof(KeySolved):
                    CheckCompletion();
                    break;
            }
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_ColorPuzzleSolved()
    {
        ColorSolved = true;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_KeyPuzzleSolved()
    {
        KeySolved = true;
    }

    private void CheckCompletion()
    {
        // Either player can solve either puzzle, but ONLY the host applies the final door variables
        if (ColorSolved && KeySolved)
        {
            Debug.Log("Both puzzles completed - Opening doors via Host Authority");
            
            if (Object.HasStateAuthority)
            {
                if (leftDoor != null) leftDoor.IsOpen = true;
                if (rightDoor != null) rightDoor.IsOpen = true;
            }
        }
    }
}