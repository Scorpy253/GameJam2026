using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Windows;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerInputs : MonoBehaviour
{
    private Vector2 moveInput;
    private Rigidbody2D mybody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mybody = GetComponent<Rigidbody2D>();
    }
    public void OnMove(InputValue moving)
    {
        moveInput = moving.Get<Vector2>();
    }
    // Update is called once per frame
    void Update()
    {
        mybody.linearVelocity = moveInput;
    }
}
