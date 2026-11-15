using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Windows;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.Concurrent;

public class PlayerInputs : MonoBehaviour
{
    public List<SpellsList.Arrow> castingPress = new List<SpellsList.Arrow>();
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
    public void OnCast(InputValue casting)
    {
        Vector2 dir = casting.Get<Vector2>();
        Debug.Log(casting.Get<Vector2>());
        if (dir == Vector2.zero) return;
        if (dir.y > 0.5f) CheckSpells(SpellsList.Arrow.Up);
        else if (dir.y < -0.5f) CheckSpells(SpellsList.Arrow.Down);
        else if (dir.x < -0.5f) CheckSpells(SpellsList.Arrow.Left);
        else if (dir.x > 0.5f) CheckSpells(SpellsList.Arrow.Right);
    }
    void CheckSpells(SpellsList.Arrow input)
    {
        castingPress.Add(input);
        int c = 0;
        foreach(SpellsList.Arrow cast in castingPress) //for every input the player recently did to cast
        {
            //Debug.Log("Checking Arrow");
            foreach(EquippedSpells Arrows in GetComponent<PlayerInformation>().currentSpells) //in each currentspell
            {
                //Debug.Log("Checking Spells");
                if (cast == Arrows.SpellArrow[c])
                {
                    Debug.Log("CORRECT");
                    if (Arrows.SpellArrow.Length == c+1)
                    {
                        Debug.Log("FIREBALLLLLLLLL");
                    }
                }
            }
            c++;
        }
    }
    // Update is called once per frame
    void Update()
    {
        mybody.linearVelocity = moveInput*5;
    }
}
