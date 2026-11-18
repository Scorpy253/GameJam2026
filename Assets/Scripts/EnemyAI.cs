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
                ActiveAI = new BasicEnemy(this.transform);
                break;
            case "Ranged":
                ActiveAI = new RangedEnemy(this.transform);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ActiveAI.MoveTowardsEnemy();
        //ACTIVE AI LOGIC HERE
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (ActiveAI == null) return;
        Debug.Log("Hit: " + collision.gameObject.name + " | Tag: " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("DIED");
        }
    }
}
public class BasicEnemy
{
    protected Transform Mylegsbro;
    float moveSpeed = 3f;
    public float stopDistance = 1f;
    public BasicEnemy(Transform transform)
    {
        Mylegsbro = transform;
    }
    public void MoveTowardsEnemy()
    {
        if (EnemyAI.playerInputForPrefab == null) return;

        if (!InRange())
            MoveTowardsPlayer();
    }
    Vector3 GetDirectionToPlayer()
    {
        Vector3 direction = EnemyAI.playerInputForPrefab.transform.position - Mylegsbro.transform.position;
        direction.z = 0f;
        return direction;
    }
    bool InRange()
    {
        return GetDirectionToPlayer().magnitude <= stopDistance;
    }
    public void MoveTowardsPlayer()
    {
        Vector3 moveDir = GetDirectionToPlayer().normalized;
        Mylegsbro.transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}
public class RangedEnemy : BasicEnemy
{
    public RangedEnemy(Transform transform) : base(transform)
    {
        stopDistance = 8f;
    }
}