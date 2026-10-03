using UnityEngine;

public class Road : MonoBehaviour
{
    public GameObject[] roadParts;
    //public float speed;

    private void Update()
    {
        // Move();
    }

    // Fais bouger la route
    public void Move(float speed)
    {
        foreach (GameObject road in roadParts)
        {
            Vector3 position = road.transform.position;
            road.transform.position = new Vector3(position.x - speed, position.y, position.z);
            if (position.x < -10.47f)
            {
                road.transform.position = new Vector3(15.35f, position.y, position.z);
            }
        }
    }
}
