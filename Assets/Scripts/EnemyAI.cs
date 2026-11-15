using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class EnemyAI : MonoBehaviour
{
    float moveSpeed = 3f;
    public float stopDistance = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsEnemy();
    }
    public void MoveTowardsEnemy()
    {
        if (player == null) return;

        if (!InRange())
            MoveTowardsPlayer();

    }
    Vector3 GetDirectionToPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;
        return direction;
    }
    bool InRange()
    {
        return  GetDirectionToPlayer().magnitude <= stopDistance;
    }
    public void MoveTowardsPlayer()
    {
        Vector3 moveDir = GetDirectionToPlayer().normalized;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}
public class RangedEnemy : EnemyAI
{
     void Reset()
    {
        stopDistance = 8f;
    }
}