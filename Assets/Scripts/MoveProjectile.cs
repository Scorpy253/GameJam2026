using UnityEngine;

public class MoveProjectile : MonoBehaviour
{
    public Vector3 TargetDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += TargetDirection * 3 * Time.deltaTime;
    }
}
