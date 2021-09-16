using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    private Vector3 startingPoint;
    private Vector3 openPosition;
    [SerializeField] private Vector3 direction = Vector3.down;
    [SerializeField] private float distance;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float waitTime = 3f;
    private bool isOpen = false;
    private float nextTimeDoorMoves;
    Coroutine running;
    void Start()
    {
        startingPoint = transform.position;
        openPosition = transform.position + (direction.normalized * distance);

        nextTimeDoorMoves = waitTime;
    }
    void Update()
    {
        if(nextTimeDoorMoves <= Time.time)
        {
            nextTimeDoorMoves = Time.time + waitTime;

            if(isOpen)
            {
                if (running != null) StopCoroutine(running);
                running = StartCoroutine(MoveDoor(startingPoint));
                isOpen = false;
            }
            else
            {
                isOpen = true;
                if (running != null) StopCoroutine(running);
                running = StartCoroutine(MoveDoor(openPosition));
            }
            Debug.Log("OPEN DOOR");
        }
    }
    IEnumerator MoveDoor(Vector3 position)
    {
        while (Vector3.Distance(transform.position, position) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * speed);
            yield return null; // wait one frame
        }
    }
}
