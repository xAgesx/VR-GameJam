using UnityEngine;

public class ItemFeedback : MonoBehaviour
{
    
    public Vector3 rotationSpeed = new Vector3(0, 50, 0);

  
    public float amplitude = 0.2f;
    public float frequency = 1f;  

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
        Vector3 tempPos = startPos;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
    }
}