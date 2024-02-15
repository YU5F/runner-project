using UnityEngine;

[CreateAssetMenu(fileName = "MovementController", menuName = "InputController/MovementController")]
public class MovementController : InputController
{
    public override float RetrieveHorizontalInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return -1;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public override bool RetrieveJumpInput()
    {
        return Input.GetKeyDown(KeyCode.UpArrow);
    }

    public override bool RetrieveRollInput()
    {
        return Input.GetKeyDown(KeyCode.DownArrow);
    }
}
