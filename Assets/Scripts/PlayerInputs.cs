using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Collections;
using UnityEngine.Windows;

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
    private CastInputUI castInputUI;
    [SerializeField] private SpriteRenderer ArenaFloor;
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mybody = GetComponent<Rigidbody2D>();
        playerInfo = GetComponent<PlayerInformation>();
        spellExecutor = GetComponent<SpellExecutor>();
        shiftAction = GetComponent<PlayerInput>().actions["Shift"];
        castInputUI = GetComponent<CastInputUI>();
        animator = GetComponent<Animator>();
        if (castInputUI != null)
            castInputUI.SetCastingState(false);
    }
    public void OnMove(InputValue moving)
    {
        moveInput = moving.Get<Vector2>();
        UpdateMoveAnimation(moveInput);
    }
    private void UpdateMoveAnimation(Vector2 input)
    {
        int dir = 0; // 0 = Idle

        if (input.sqrMagnitude > 0.01f)
        {
            // Vertical priority on diagonals (swap >= to > for horizontal priority)
            if (Mathf.Abs(input.y) >= Mathf.Abs(input.x))
                dir = input.y < 0f ? 1 : 2; // 1 = WalkingFront, 2 = WalkingBack
            else
                dir = input.x < 0f ? 3 : 4; // 3 = WalkingLeft, 4 = WalkingRight
        }
        animator.SetInteger("Direction", dir);
        animator.SetInteger("Idling", dir);
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
        if (castInputUI != null)
            castInputUI.SetCastingState(true);
        while (shiftAction.IsPressed())
        {
            Casting = true;
            yield return null;
        }
        Casting = false;
        castingPress.Clear();
        if (castInputUI != null)
            castInputUI.SetCastingState(false);
        ResetIgnoredSpells();
        shiftRoutine = null;
    }
    public void OnCast(InputValue casting)
    {
        if (Casting)
        {
            Vector2 dir = casting.Get<Vector2>();
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
            castInputUI.ClearDisplay();
        int c = castingPress.Count - 1;

        foreach(EquippedSpells Arrows in playerInfo.currentSpells) //in each currentspell
        {
            if (Arrows.SpellArrow.Length > c && !Arrows.ignored)
            {
                if (input == Arrows.SpellArrow[c])
                {
                    //Debug.Log("CORRECT");
                    if (Arrows.SpellArrow.Length == c + 1)
                    {
                        spellExecutor.CastSpellFromSlot(Arrows.SpellSlot, mybody.transform.position);
                        //Debug.Log("FIREBALLLLLLLLL");
                        //CODE TO CHOOSE SPELL
                        castingPress.Clear();
                        ResetIgnoredSpells();
                        if (castInputUI != null)
                            castInputUI.SetCastingState(true);
                        return;
                    }
                    //CODE TO HIGHLIGHT ARROWS
                    if (castInputUI != null)
                        castInputUI.AddValidSpell(Arrows, c + 1);
                    continue;
                }
                else
                {
                    Arrows.ignored = true;
                    //CODE TO MAKE THIS SPECIFIC SPELL IGNORABLE/DIM DOWN
                }
            }
        }
    }
    private void ResetIgnoredSpells()
    {
        foreach (EquippedSpells spell in playerInfo.currentSpells)
        {
            spell.ignored = false;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        mybody.linearVelocity = moveInput*playerInfo.MVSPD;
    }
}
