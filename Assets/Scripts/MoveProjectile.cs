using UnityEngine;

public class MoveProjectile : MonoBehaviour
{
    public float timealive = 0f;
    public Vector3 TargetDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += TargetDirection * 3 * Time.deltaTime;
        timealive += Time.deltaTime;
        if (timealive >= 7)
        {
            Destroy(gameObject);
        }
    }
}
