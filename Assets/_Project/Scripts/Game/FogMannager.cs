using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FogMannager : MonoBehaviour
{
    public Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
            transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
    }
}
