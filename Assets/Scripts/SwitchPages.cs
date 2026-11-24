using UnityEngine;
using System.Collections;
public class SwitchPages : MonoBehaviour
{
    public GameObject MenuO;
    public GameObject MenuP;
    public GameObject MenuB;
    public SpriteRenderer TheMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Animator BookAnim;
    void Start()
    {
        BookAnim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FixData(int page)
    {
        if(page == 1)
        {
            MenuO.SetActive(true);
            TheMenu.sprite = Resources.Load<Sprite>("Textures/BookSpells");
        }
        else if(page == 2)
        {
            MenuP.SetActive(true);
            TheMenu.sprite = Resources.Load<Sprite>("Textures/BookTree");
        }
        else if (page == 3)
        {
            MenuB.SetActive(true);
            TheMenu.sprite = Resources.Load<Sprite>("Textures/BookUpgrade");
        }
    }
    public void OpenMenu()
    {
        BookAnim.SetBool("IsOpen", true);
        BookAnim.ResetTrigger("Close");
        BookAnim.SetTrigger("Open");
    }
    public void CloseMenu()
    {
        BookAnim.SetBool("IsOpen", false);
        BookAnim.ResetTrigger("Open");
        BookAnim.SetTrigger("Close");
    }
    public void Upgrades()
    {
        if (!BookAnim.IsInTransition(0))
        {
            if (BookAnim.GetInteger("State") != 1 && BookAnim.GetInteger("State") != 3)
            {
                MenuO.SetActive(false);
                MenuP.SetActive(false);
                MenuB.SetActive(false);
                if (BookAnim.GetInteger("State") == 0 || BookAnim.GetInteger("State") == 5 || BookAnim.GetInteger("State") == -1)
                {
                    BookAnim.SetInteger("State", 1);
                }
                else if (BookAnim.GetInteger("State") == 2 || BookAnim.GetInteger("State") == 4)
                {
                    BookAnim.SetInteger("State", 3);
                }
            }
        }
    }
    public void Tree()
    {
        if (!BookAnim.IsInTransition(0))
        {
            if (BookAnim.GetInteger("State") != 2 && BookAnim.GetInteger("State") != 4)
            {
                MenuO.SetActive(false);
                MenuP.SetActive(false);
                MenuB.SetActive(false);
                if (BookAnim.GetInteger("State") == 1 || BookAnim.GetInteger("State") == 3)
                {
                    BookAnim.SetInteger("State", 2);
                }
                else if (BookAnim.GetInteger("State") == 0 || BookAnim.GetInteger("State") == 5 || BookAnim.GetInteger("State") == -1)
                {
                    BookAnim.SetInteger("State", 4);
                }
            }
        }
    }
    public void Equip()
    {
        if (!BookAnim.IsInTransition(0))
        {
            if (BookAnim.GetInteger("State") != 0 && BookAnim.GetInteger("State") != 5)
            {
                MenuO.SetActive(false);
                MenuP.SetActive(false);
                MenuB.SetActive(false);
                if (BookAnim.GetInteger("State") == 1 || BookAnim.GetInteger("State") == 3)
                {
                    BookAnim.SetInteger("State", 0);
                }
                else if (BookAnim.GetInteger("State") == 2 || BookAnim.GetInteger("State") == 4)
                {
                    BookAnim.SetInteger("State", 5);
                }
            }
        }
    }
}
