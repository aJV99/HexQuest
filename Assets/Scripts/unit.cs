using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

[SelectionBase]

public class unit : MonoBehaviour
{
    private int movementPoints = 20; //movement points of our movement

    public int currentPower = 100; //power of character

    public int gold = 0; //currency of player
    public int MovementPoints { get => movementPoints; } //gets this amount when planning path 

    [SerializeField]
    private float movementDuration = 1, rotationDuration = .3f; //how long movement will take

    private GlowHighlight glowHighlight;//player glows so know it is selected
    private Queue<Vector3> pathPositions = new Queue<Vector3>();//give unit path it will travel

    public event Action<unit> MovementFinished;

    private void Awake()
    {
        glowHighlight = GetComponent<GlowHighlight>();

    }

    internal void Deselect()
    {
        glowHighlight.ToggleGlow(false);
    }

    public void Select()
    {
        glowHighlight.ToggleGlow();
    }

    internal void MoveThroughoutPath(List<Vector3>currentPath)
    {
        pathPositions = new Queue<Vector3>(currentPath);
        Vector3 firstTarget = pathPositions.Dequeue();
        StartCoroutine(RotationCoroutine(firstTarget, rotationDuration));

    }

    private IEnumerator RotationCoroutine(Vector3 endPosition, float rotationDuration)
    {
        Quaternion startRotation = transform.rotation;
        endPosition.y = transform.position.y;
        Vector3 direction = endPosition - transform.position;
        Quaternion endRotation = Quaternion.LookRotation(direction, Vector3.up);

        if (Mathf.Approximately(Mathf.Abs(Quaternion.Dot(startRotation, endRotation)), 1.0f) == false)
        {
            float timeElapsed = 0;
            while(timeElapsed < rotationDuration)
            {
                timeElapsed += Time.deltaTime;
                float lerpStep = timeElapsed / rotationDuration;
                transform.rotation = Quaternion.Lerp(startRotation, endRotation, lerpStep);
                yield return null;
            }
            transform.rotation = endRotation;
        }
        StartCoroutine(MovementCoroutine(endPosition));
    }

    private IEnumerator MovementCoroutine(Vector3 endPosition)
    {

        Vector3 startPosition = transform.position;
        endPosition.y = startPosition.y;
        float timeElapsed = 0;

        while(timeElapsed < movementDuration)
        {
            timeElapsed += Time.deltaTime;
            float lerpStep = timeElapsed / movementDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, lerpStep);
            yield return null;
        }
        transform.position = endPosition;

        if(pathPositions.Count > 0)
        {
            Debug.Log("Selecting the next position");
            StartCoroutine(RotationCoroutine(pathPositions.Dequeue(), rotationDuration));
        }
        else
        {
            Debug.Log("Movement Finished");
            MovementFinished?.Invoke(this);
            Attack();
            Collect_Coin();
        }
    }



    public void Attack()
    {
        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
        for(int i = 0; i< enemies.Length; i++)
        {
            if (transform.position.x == enemies[i].transform.position.x && transform.position.z == enemies[i].transform.position.z)
            {
                enemies[i].TakeDamage(currentPower);
                this.currentPower -= enemies[i].power;

            }
            else
            {
                Debug.Log("No Enemy here");
            }
        }

    }

    public void Collect_Coin()
    {
        Coin[] coins = GameObject.FindObjectsOfType<Coin>();
        for(int i=0; i < coins.Length; i++)
        {
            if (transform.position.x == coins[i].transform.position.x && transform.position.z == coins[i].transform.position.z)
            {
                this.gold += 50;
                Destroy(coins[i].gameObject); //Delete coin from the screen
            }
            else
            {
                Debug.Log("No Gold Here");
            }
        }
    }

}
