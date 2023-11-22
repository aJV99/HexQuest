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

    public int currentTurns = 10; //how many turns to move a player has

    public int maxTurns = 10; //the maximum of turns player will have after tavern

    public int MovementPoints { get => movementPoints; } //gets this amount when planning path 

    [SerializeField]
    private float movementDuration = 1, rotationDuration = .3f; //how long movement will take

    [SerializeField]
    private TextMeshProUGUI notifText;

    [SerializeField]
    private PopupManager popupManager;

    //public Image battleImage;
    //public float displayDuration = 6f;
    //public Sprite battleSprite;
    //public Sprite winSprite;
    //public Sprite lossSprite;
    //public AudioClip battleSound;
    //private AudioSource audioSource;

    private GlowHighlight glowHighlight;//player glows so know it is selected
    private Queue<Vector3> pathPositions = new Queue<Vector3>();//give unit path it will travel

    public event Action<unit> MovementFinished;

    private void Awake()
    {
        //battleImage.gameObject.SetActive(false);
        notifText.gameObject.SetActive(false);
        glowHighlight = GetComponent<GlowHighlight>();
        //audioSource = GetComponent<AudioSource>();
        //if (audioSource == null)
        //{
        //    audioSource = gameObject.AddComponent<AudioSource>();
        //}

    }


    internal void Deselect()
    {
        glowHighlight.ToggleGlow(false);
    }

    public void Select()
    {
        glowHighlight.ToggleGlow();
    }

    //public void DisplayImageDuringBattle(BattleImageType imageType)
    //{
    //    Debug.Log("DisplayImageDuringBattle called with type: " + imageType.ToString());
    //    StartCoroutine(ShowImage(imageType));
    //}

    //private IEnumerator ShowImage(BattleImageType imageType, Action callback = null)
    //{
    //    Debug.Log("ShowImage coroutine started for type: " + imageType.ToString());

    //    // Set the correct sprite based on the image type
    //    switch (imageType)
    //    {
    //        case BattleImageType.Battle:
    //            battleImage.sprite = battleSprite;
    //            notifText.text = $"Battling...";
    //            notifText.gameObject.SetActive(true);
    //            PlaySound(battleSound);
    //            break;
    //        case BattleImageType.Win:
    //            battleImage.sprite = winSprite;
    //            break;
    //        case BattleImageType.Loss:
    //            battleImage.sprite = lossSprite;
    //            break;
    //    }

    //    battleImage.gameObject.SetActive(true); // Show the image
    //    yield return new WaitForSeconds(displayDuration); // Wait for specified duration
    //    battleImage.gameObject.SetActive(false); // Hide the image

    //    callback?.Invoke();
    //}

    //public enum BattleImageType
    //{
    //    Battle,
    //    Win,
    //    Loss
    //}

    //private void PlaySound(AudioClip clip)
    //{
    //    if (audioSource != null && clip != null)
    //    {
    //        audioSource.clip = clip;
    //        audioSource.Play();
    //    }
    //}

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
            this.currentTurns -= 1;
            Attack(startPosition);
            Collect_Coin();
            Visit_Tavern();
            Visit_Town();
            Rough_Sleep();
            if (this.currentPower <= 0)
            {
                popupManager.ShowLossPopup("You Lose");
            }

        }

    }

    private int playerStrength;
    private int enemyStrength;
    private int playerBonus = 0;
    private int enemyBonus = 0;
    private int playerGold; // Store the player's current gold amount

    private float playerEffectiveStrength;
    private float enemyEffectiveStrength;
    private List<Vector3> lossTiePath = new List<Vector3>();

    public void Attack(Vector3 startPosition)
    {
        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
        for (int i = 0; i < enemies.Length; i++)
        {
            if (transform.position.x == enemies[i].transform.position.x && transform.position.z == enemies[i].transform.position.z)
            {
                // Start by displaying the Battle image
                //StartCoroutine(ShowImage(BattleImageType.Battle, () =>

                //{
                    // After the Battle image is done, compute the result
                    playerStrength = this.currentPower;
                    enemyStrength = enemies[i].power;
                    playerGold = this.gold;
                    CalculateEffectiveStrengths();

                    const float epsilon = 0.1f;
                    if (Mathf.Abs(playerEffectiveStrength - enemyEffectiveStrength) < epsilon)
                    {
                        BattleTie();
                        this.currentPower = playerStrength;
                        enemies[i].power = enemyStrength;
                        lossTiePath.Add(startPosition);
                        MoveThroughoutPath(lossTiePath);
                        return;
                    }

                    if (playerEffectiveStrength > enemyEffectiveStrength)
                    {
                        PlayerWins();
                        //DisplayImageDuringBattle(BattleImageType.Win);
                        notifText.text = $"You DEFEATED the ENEMY!";
                        notifText.gameObject.SetActive(true);
                        enemies[i].gameObject.SetActive(false);
                        this.currentPower = playerStrength;
                        this.gold = playerGold;
                        if (enemies[i].isBoss)
                    {   
                        if(this.currentTurns <= 0)
                        {
                            this.currentTurns = 1;
                        }
                        popupManager.ShowLossPopup("You Win");
                    }
                    }
                    else
                    {
                        //DisplayImageDuringBattle(BattleImageType.Loss);
                        EnemyWins();
                        this.currentPower = playerStrength;
                    }

                    if (this.currentPower <= 0)
                    {
                        popupManager.ShowLossPopup("You Lose");
                    }
                //}));
                //break; // exit the loop as we've found an enemy to battle
            }
            else
            {
                Debug.Log("No Enemy here");
            }
        }
    }


    private float RoundToOneDecimalPlace(float value)
    {
        return Mathf.Round(value * 10f) / 10f;
    }

    private void CalculateEffectiveStrengths()
    {
        playerEffectiveStrength = playerStrength + playerBonus + RoundToOneDecimalPlace(UnityEngine.Random.Range(0f, playerStrength * 0.1f));
        enemyEffectiveStrength = enemyStrength + enemyBonus + RoundToOneDecimalPlace(UnityEngine.Random.Range(0f, enemyStrength * 0.1f));

        Debug.Log($"playerEffectiveStrength: {playerEffectiveStrength}, enemyEffectiveStrength: {enemyEffectiveStrength}");
    }

    private void PlayerWins()
    {
        Debug.Log("Player Wins!");

        float winMargin = (float)playerEffectiveStrength / enemyEffectiveStrength;
        playerStrength += Mathf.FloorToInt(playerStrength * (1 - winMargin) * 0.2f); // Player loses power based on win closeness
        Debug.Log($"Win Margin: {winMargin}, Player Strength Loss: {Mathf.FloorToInt(playerStrength * (1 - winMargin) * 0.2f)}");

        // Calculate gold reward
        float goldFactor = UnityEngine.Random.Range(0.35f, 0.65f);
        int goldReward = Mathf.FloorToInt(goldFactor * 2 * enemyStrength);
        playerGold += goldReward;

        Debug.Log($"Player has been rewarded with {goldReward} gold!");
        enemyStrength = 0; // Destroy the enemy
        
    }

    private void EnemyWins()
    {
        Debug.Log("Enemy Wins!");
        Debug.Log("Game Over for the player!");
        // Implement logic to handle player's game over scenario
        playerStrength = 0;
    }

    private void BattleTie()
    {
        Debug.Log("It's a tie!");

        // Both sides lose 20% of their troops
        playerStrength -= Mathf.FloorToInt(playerStrength * 0.2f);
        enemyStrength -= Mathf.FloorToInt(enemyStrength * 0.2f);

        // Player is sent back to previous hex in the game logic (not shown here)
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

            if ((transform.position.x == (taverns[i].transform.position.x)) && transform.position.z == taverns[i].transform.position.z)
            {
                if (this.currentTurns == 0)
                {
                    this.currentTurns = 1;
                }

                if (this.gold < 10)
                {
                    popupManager.ShowNoticePopup("You don't have enough gold");
                    return;

                }
                else
                {

                popupManager.ShowAreYouSurePopup("Do you want to spend 10 gold to recharge your turn count?", (bool isConfirmed) =>
                {
                    if (!isConfirmed)
                    {
                        Debug.Log("Popup -> NO");
                        return;
                    }
                    else
                    {
                        Debug.Log("Popup -> YES");
                        
                            this.gold -= 10;
                            this.currentTurns = this.maxTurns;
                    }
                });
                }


                //coins[i].gameObject.SetActive(false); //Delete coin from the screen
            }
            else
            {
                Debug.Log("You cannot enter this tavern");
            }
        }
    }

    public void Visit_Town()
    {
        Town[] towns = GameObject.FindObjectsOfType<Town>();
        for (int i = 0; i < towns.Length; i++)
        {

            if ((transform.position.x == (towns[i].transform.position.x)) && transform.position.z == towns[i].transform.position.z)
            {
                if (this.currentTurns == 0)
                {
                    this.currentTurns = 1;
                }

                popupManager.ShowTownPopup();
                return;



            }
        }
    }




    public void Rough_Sleep()
    {
        if (this.currentTurns == 0)
        {
            Debug.Log("in rough sleep");

            if (this.gold < 10)
            {
                this.currentPower -= 10;
                if (currentPower > 0)
                {
                    popupManager.ShowNoticePopup("You have no turns left, you must sleep rough to continue. This has cost you 10 power");

                }


            }
            else
            {
                popupManager.ShowNoticePopup("You have no turns left, you must sleep rough to continue. This has cost you 10 gold");
                this.gold -= 10;
            }
            this.currentTurns = (this.maxTurns)/2;

            //popupManager.ShowNoticePopu("", (bool isConfirmed) =>
            //{

            //    if (isConfirmed)
            //    {
                    
            //    }
            //});
        }
    }

}
