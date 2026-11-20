using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Windows;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Collections;

public class PlayerInputs : MonoBehaviour
{
    public List<SpellsList.Arrow> castingPress = new List<SpellsList.Arrow>();
    private Vector2 moveInput;
    private Rigidbody2D mybody;
    public bool Casting = false;
    public Coroutine shiftRoutine;
    private SpellExecutor spellExecutor;
    private PlayerInformation playerInfo;
    private InputAction shiftAction;

    [SerializeField] private CastInputUI castInputUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mybody = GetComponent<Rigidbody2D>();
        playerInfo = GetComponent<PlayerInformation>();
        spellExecutor = GetComponent<SpellExecutor>();
        shiftAction = GetComponent<PlayerInput>().actions["Shift"];

        if (castInputUI != null)
            castInputUI.SetCastingState(false, castingPress);
    }
    public void OnMove(InputValue moving)
    {
        moveInput = moving.Get<Vector2>();
    }
    public void OnShift(InputValue shift)
    {
        if (shift.isPressed )
        {
            if (shiftRoutine == null)
            {
                shiftRoutine = StartCoroutine(WhileHoldingShift());
            }
        }
    }
    private IEnumerator WhileHoldingShift()
    {
        Casting = true;
        if (castInputUI != null)
            castInputUI.SetCastingState(true, castingPress);

        while (shiftAction.IsPressed())
        {
            Casting = true;
            yield return null;
        }
        Casting = false;
        castingPress.Clear();
        if (castInputUI != null)
            castInputUI.SetCastingState(false, castingPress);
        shiftRoutine = null;
    }
    public void OnCast(InputValue casting)
    {
        if (Casting)
        {
            Vector2 dir = casting.Get<Vector2>();
            //Debug.Log(casting.Get<Vector2>());
            if (dir == Vector2.zero) return;
            if (dir.y > 0.5f) CheckSpells(SpellsList.Arrow.Up);
            else if (dir.y < -0.5f) CheckSpells(SpellsList.Arrow.Down);
            else if (dir.x < -0.5f) CheckSpells(SpellsList.Arrow.Left);
            else if (dir.x > 0.5f) CheckSpells(SpellsList.Arrow.Right);
        }
    }
    void CheckSpells(SpellsList.Arrow input)
    {
        castingPress.Add(input);
        if (castInputUI != null)
            castInputUI.UpdateInputs(castingPress);
        int c = 0;
        foreach(SpellsList.Arrow cast in castingPress) //for every input the player recently did to cast
        {
            //Debug.Log("Checking Arrow");
            foreach(EquippedSpells Arrows in GetComponent<PlayerInformation>().currentSpells) //in each currentspell
            {
                if (Arrows.SpellArrow.Length != c && Arrows.SpellArrow.Length > c)
                {
                    if (cast == Arrows.SpellArrow[c])
                    {
                        //Debug.Log("CORRECT");
                        if (Arrows.SpellArrow.Length == c + 1)
                        {
                            spellExecutor.CastSpellFromSlot(Arrows.SpellSlot, mybody.transform.position);
                            //Debug.Log("FIREBALLLLLLLLL");
                            //CODE TO CHOOSE SPELL
                            castingPress.Clear();
                            if (castInputUI != null)
                                castInputUI.UpdateInputs(castingPress);
                            return;
                        }
                        //CODE TO HIGHLIGHT ARROWS

                        continue;
                    }
                    else
                    {
                        //CODE TO MAKE THIS SPECIFIC SPELL IGNORABLE/DIM DOWN
                    }
                }
                else
                {
                    //CODE TO DARKEN THE ARROW
                }
            }
            c++;
        }
    }
    // Update is called once per frame
    void Update()
    {
        mybody.linearVelocity = moveInput*playerInfo.MVSPD;
    }
}
