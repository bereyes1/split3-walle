using UnityEngine;
using NewerInput;
using UnityEngine.InputSystem;
using System;

[CreateAssetMenu(menuName = "ScriptableObjects/InputReader")]
public class InputReader : ScriptableObject, RealInput.IPlayerActions
{
    public RealInput realInput;

    private void OnEnable()
    {
        if (realInput == null)
        {
            realInput = new RealInput();
            realInput.Player.SetCallbacks(this);
        }

        SetPlayer();
    }

    private void OnDisable()
    {
        DisableAll();
    }

    public void SetPlayer()
    {
        DisableAll();
        realInput.Player.Enable();
    }

    //disable all input maps
    public void DisableAll()
    {
        realInput.Player.Disable();
    }

    public event Action<Vector2> MoveEvent;
    public event Action<Vector2> LookEvent;
    public event Action SprintEvent;
    public event Action InteractEvent;
    public event Action CraftEvent;
    public event Action PlaceEvent;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            InteractEvent?.Invoke();
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            SprintEvent?.Invoke();
        }
    }

    public void OnCraft(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            CraftEvent?.Invoke();
        }
    }

    public void OnPlace(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PlaceEvent?.Invoke();
        }
    }
}
