using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private GameObject playerRef;
    public static GameObject playerInputForPrefab;
    public string EnemyType;
    public string EnemyElement;
    private BasicEnemy ActiveAI;
    public float spdMulti;
    public float dmgMulti;
    public float hpMulti;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (playerRef != null)
            playerInputForPrefab = playerRef;
        else
            playerInputForPrefab = GameObject.FindGameObjectWithTag("Player");
        switch(EnemyType) 
        {
            case "Basic":
                ActiveAI = new BasicEnemy(this.transform, EnemyElement);
                break;
            case "Ranged":
                ActiveAI = new RangedEnemy(this.transform, EnemyElement);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ActiveAI.MoveTowardsEnemy();
        ActiveAI.Cooldown += Time.deltaTime;
        //ACTIVE AI LOGIC HERE
        if(ActiveAI.HP <= 0)
        {
            ActiveAI.PayPlayer();
            Destroy(gameObject);
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (ActiveAI == null) return;
        if (collision.gameObject.CompareTag("Player")&& EnemyType == "Basic" && ActiveAI.Cooldown >= ActiveAI.CooldownSet)
        {
            Debug.Log("DIED");
            ActiveAI.Cooldown = 0f;
        }
    }
}
public class BasicEnemy
{
    protected Transform Mylegsbro;
    protected string MyElement;
    float moveSpeed = 3f;
    public float stopDistance = 1f;
    public float Cooldown;
    public float CooldownSet;
    public int HP;
    public BasicEnemy(Transform transform, string element)
    {
        Mylegsbro = transform;
        MyElement = element;
        CooldownSet = 1;
        Cooldown = 0;
        HP = 10;
    }
    public virtual void MoveTowardsEnemy()
    {
        if (EnemyAI.playerInputForPrefab == null) return;

        if (!InRange())
            MoveTowardsPlayer();
    }
    public Vector3 GetDirectionToPlayer()
    {
        Vector3 direction = EnemyAI.playerInputForPrefab.transform.position - Mylegsbro.transform.position;
        direction.z = 0f;
        return direction;
    }
    public bool InRange()
    {
        return GetDirectionToPlayer().magnitude <= stopDistance;
    }
    public void MoveTowardsPlayer()
    {
        Vector3 moveDir = GetDirectionToPlayer().normalized;
        Mylegsbro.transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
    public void TakeDamage(int totaldamage)
    {
        HP -= totaldamage;
    }
    public void PayPlayer()
    {
        EnemyAI.playerInputForPrefab.GetComponent<PlayerInformation>().totalGold += 10;
    }
}
public class RangedEnemy : BasicEnemy
{
    private static Dictionary<string, GameObject> spellCache = new Dictionary<string, GameObject>();
    GameObject loadedSpell;
    public RangedEnemy(Transform transform, string element) : base(transform,element)
    {
        stopDistance = 8f;
        loadedSpell = LoadSpellByElement(element);
        CooldownSet = 3f;
    }
    private GameObject LoadSpellByElement(string element)
    {
        //if (string.IsNullOrEmpty(element)) return null;
        if (spellCache.TryGetValue(element, out GameObject cachedSpell))
            return cachedSpell;
        GameObject loadedSpell = Resources.Load<GameObject>($"SpellPrefabs/BulletPlaceholder");
        if (loadedSpell == null)
            Debug.LogWarning($"Spell prefab not found at Resources/Spells/BulletPlaceholder");
        else
            spellCache[element] = loadedSpell;
        return loadedSpell;
    }
    public override void MoveTowardsEnemy()
    {
        if (EnemyAI.playerInputForPrefab == null) return;

        if (!InRange())
            MoveTowardsPlayer();
        else ShootPlayer();
    }
    public void ShootPlayer()
    {
        if (Cooldown >= CooldownSet)
        {
            GameObject obj = Object.Instantiate(loadedSpell, Mylegsbro.transform.position, Quaternion.identity);
            obj.GetComponent<MoveProjectile>().TargetDirection = GetDirectionToPlayer().normalized;
            Cooldown = 0f;
        }
    }
}