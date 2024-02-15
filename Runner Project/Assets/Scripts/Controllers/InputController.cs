using UnityEngine;

public abstract class InputController : ScriptableObject
{
    public abstract float RetrieveHorizontalInput();
    public abstract bool RetrieveJumpInput();
    public abstract bool RetrieveRollInput();
}
