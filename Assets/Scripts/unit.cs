using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

[SelectionBase]

public class unit : MonoBehaviour
{
    private int movementPoints = 20; //movement points of our movement

    public int currentPower = 10; //power of character

    public int gold = 0; //currency of player

    public int currentTurns = 3; //how many turns to move a player has

    public int maxTurns = 5; //the maximum of turns player will have after tavern

    public int MovementPoints { get => movementPoints; } //gets this amount when planning path 

    [SerializeField]
    private float movementDuration = 1, rotationDuration = .3f; //how long movement will take

    [SerializeField]
    private TextMeshProUGUI notifText;

    [SerializeField]
    private PopupManager popupManager;

    private GlowHighlight glowHighlight;//player glows so know it is selected
    private Queue<Vector3> pathPositions = new Queue<Vector3>();//give unit path it will travel

    public event Action<unit> MovementFinished;

    private void Awake()
    {
        notifText.gameObject.SetActive(false);
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
            Visit_Tavern();
        }
    }



    private System.Random random = new System.Random();

    public void Attack()
    {
        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
        for (int i = 0; i < enemies.Length; i++)
        {
            if (transform.position.x == enemies[i].transform.position.x && transform.position.z == enemies[i].transform.position.z)
            {
                double winProbability;
                bool isPlayerWin;
                int playerLosses, enemySurrenders;

                // Simulate battle
                SimulateBattle(currentPower, enemies[i].power, out winProbability, out isPlayerWin, out playerLosses, out enemySurrenders);

                Debug.Log($"Player win chance: {winProbability:0.##}%");
                Debug.Log($"Player won: {isPlayerWin}");
                Debug.Log($"Player troop losses: {playerLosses}");
                Debug.Log($"Enemy troops surrendered: {enemySurrenders}");

                if (isPlayerWin)
                {
                    notifText.text = $"You DEFEATED the ENEMY!\r\nYou lost {playerLosses} troops: -{playerLosses} power.\r\n{enemySurrenders} enemy troops surrendered and joined your army: +{enemySurrenders} power.";
                    notifText.gameObject.SetActive(true);
                    enemies[i].gameObject.SetActive(false);
                    this.currentPower += enemySurrenders - playerLosses;
                }
                else
                {
                    this.currentPower -= playerLosses;
                    if (this.currentPower <= 0)
                    {
                        popupManager.ShowLossPopup("You Lose");


                    }
                }
            }
            else
            {
                Debug.Log("No Enemy here");
            }
        }
    }

    private double WinningChance(double P, double E, double k = 0.5)
    {
        double ratioTerm = k * (P / E - 1);
        double absoluteDifferenceTerm = k * (P - E) / (P + E);
        return 50 * (1 + Math.Tanh(ratioTerm + absoluteDifferenceTerm));
    }

    private void SimulateBattle(double P, double E, out double C, out bool isPlayerWin, out int playerLosses, out int enemySurrenders, double k = 0.5, double lossFactor = 0.5, double surrenderFactor = 0.5)
    {
        C = WinningChance(P, E, k);
        isPlayerWin = random.NextDouble() * 100 < C;

        double absoluteDifferenceTerm = k * (P - E) / (P + E);

        if (isPlayerWin)
        {
            playerLosses = (int)(P * (1 - C / 100) * lossFactor * (random.NextDouble() * 0.5 + 0.75));
            double baseSurrenderRate = (C / 100) * surrenderFactor;
            double additionalSurrenderRate = absoluteDifferenceTerm * 0.5;
            double surrenderRate = Math.Min(baseSurrenderRate + additionalSurrenderRate, 1);
            enemySurrenders = (int)(E * surrenderRate * (random.NextDouble() * 0.5 + 0.75));
        }
        else
        {
            playerLosses = 10; //(int)(P * lossFactor * (random.NextDouble() * 0.5 + 0.75));
            enemySurrenders = 0;
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
                coins[i].gameObject.SetActive(false); //Delete coin from the screen
            }
            else
            {
                Debug.Log("No Gold Here");
            }
        }
    }

    public void Visit_Tavern()
    {
        Tavern[] taverns = GameObject.FindObjectsOfType<Tavern>();
        for (int i = 0; i < taverns.Length; i++)
        {

            if ((transform.position.x == (taverns[i].transform.position.x)) && transform.position.z == taverns[i].transform.position.z && gold >= 10)
            {
                this.gold -= 10;
                this.currentTurns = this.maxTurns;
                //coins[i].gameObject.SetActive(false); //Delete coin from the screen
            }
            else
            {
                Debug.Log("You cannot enter this tavern");
            }
        }
    }

}
