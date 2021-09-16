using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshObstacle))]
public class SlidingDoor : MonoBehaviour
{
    private Vector3 startingPoint;
    private Vector3 openPosition;
    [SerializeField] private Vector3 direction = Vector3.down;
    [SerializeField] private float distance;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float waitTime = 3f;
    private float nextTimeDoorMoves;
    private bool isOpen = false;
    Coroutine running;

    NavMeshObstacle obstacle;


    [SerializeField] 
    private bool _isLocked;//field
    public bool IsLocked//properties
    {
        set
        {
            _isLocked = value;
            if(value) // value == true
            {
                if (running != null) StopCoroutine(running);
                if(obstacle != null) obstacle.carving = true;

            }
            else
            {
                if(obstacle != null) obstacle.carving = false;
            }
        }
        get
        {
            return _isLocked;
        }
    }

    private void OnValidate()
    {
        if (_isLocked == false) // value == true
        {
            if (running != null) StopCoroutine(running);
            if (obstacle != null) obstacle.carving = true;

        }
        else
        {
            if (obstacle != null) obstacle.carving = false;
        }
    }

    private void Awake()
    {
        obstacle = GetComponent<NavMeshObstacle>();
    }

    void Start()
    {
        startingPoint = transform.position;
        openPosition = transform.position + (direction.normalized * distance);
        nextTimeDoorMoves = waitTime;
    }

    void Update()
    {
        OpenCloseDoor();
    }

    void OpenCloseDoor()
    {
        if (IsLocked)
        {
            return;
        }

        if (nextTimeDoorMoves <= Time.time)
        {
            nextTimeDoorMoves = Time.time + waitTime;

            if (isOpen)
            {//Closing
                if (running != null) StopCoroutine(running);
                running = StartCoroutine(MoveDoor(startingPoint));
                isOpen = false;
            }
            else
            {//Opening
                isOpen = true;
                if (running != null) StopCoroutine(running);
                running = StartCoroutine(MoveDoor(openPosition));
            }
            Debug.Log("MOVE DOOR");
        }
    }


    IEnumerator MoveDoor(Vector3 position)
    {
        while(Vector3.Distance(transform.position, position) > Time.deltaTime * speed)
        {
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * speed);
            yield return null; //wait one frame
        }
    }

}
