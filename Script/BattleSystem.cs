using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Data.Common;
using System;
using UnityEditor;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN,WON,LOST}

// BattleSystem class, managing the battle logic
public class BattleSystem : MonoBehaviour
{
    //public List<Character> Participants;
   // public List<Character> TurnOrder;

    public BattleState state;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Player playerUnit;
    NonPlayer enemyUnit;

    List<Tuple<Player, NonPlayer>> participants;

    List<BattleState> turnOrder;
    int currentIndex = -1;
    bool isDead;

    public TextMeshProUGUI dialogText;
    public GameObject moveSelectorText;

    public BattleHud playerHUD;
    public BattleHud enemyHUD;


    public Button move1Button;
    public Button move2Button;

    public Button move3Button;
    public Button move4Button;

    private MagicSystem MagicSystemInstance;

    public TMP_Text move1ButtonText;
    public TMP_Text move2ButtonText;
    public TMP_Text move3ButtonText;
    public TMP_Text move4ButtonText;

    private void Awake()
    {
        
    }
    
   void Start()
   {
        
        if (SceneManager.GetActiveScene().name == "BattleScene")
        {
            MagicSystemInstance = ScriptableObject.CreateInstance<MagicSystem>();

            MagicSystemInstance.Initialize();

            state = BattleState.START;
            StartCoroutine(StartBattle());
            participants = new List<Tuple<Player, NonPlayer>>();

            participants.Add(new Tuple<Player, NonPlayer>(playerUnit, enemyUnit));
            turnOrder = new List<BattleState>();
            turnOrder.Add(BattleState.PLAYERTURN);
            turnOrder.Add(BattleState.ENEMYTURN);


            move1Button.onClick.AddListener(OnMove1ButtonClick);
            move2Button.onClick.AddListener(OnMove2ButtonClick);
            move3Button.onClick.AddListener(OnMove3ButtonClick);
            move4Button.onClick.AddListener(OnMove4ButtonClick);


            SetButtonText(move1ButtonText, MagicSystemInstance.Spells[0].SkillName);
            SetButtonText(move2ButtonText, MagicSystemInstance.Spells[1].SkillName);
            SetButtonText(move3ButtonText, MagicSystemInstance.Spells[2].SkillName);
            SetButtonText(move4ButtonText, MagicSystemInstance.Spells[3].SkillName);
        
            // Set text for the buttons
            PlayerMoveNames(MagicSystemInstance.Spells);
        }
         
        
    
        
   }

   private void SetButtonText(TMP_Text buttonText, string text)
    {
        if (buttonText != null)
        {
            // Set the text of the TextMeshPro component
            buttonText.text = text;
        }
        else
        {
            Debug.LogError("TextMeshPro component not found.");
        }
    }


    private void OnMove1ButtonClick()
    {
        if (MagicSystemInstance.Spells.Count > 0)
        {
            moveSelectorText.SetActive(false);
            dialogText.enabled = true;
            playerUnit.SelectedSkill = MagicSystemInstance.Spells[0];
            MagicSystemInstance.CastSpell(playerUnit.SelectedSkill);
            StartCoroutine(PlayerAttack());
        }
        
    }

    private void OnMove2ButtonClick()
    {
        if (MagicSystemInstance.Spells.Count > 1)
        {
            moveSelectorText.SetActive(false);
            dialogText.enabled = true;
            playerUnit.SelectedSkill = MagicSystemInstance.Spells[1];
            MagicSystemInstance.CastSpell(playerUnit.SelectedSkill);
            StartCoroutine(PlayerAttack());
        }
    }

    private void OnMove3ButtonClick()
    {
        if (MagicSystemInstance.Spells.Count > 2)
        {
            moveSelectorText.SetActive(false);
            dialogText.enabled = true;
            playerUnit.SelectedSkill = MagicSystemInstance.Spells[2];
            MagicSystemInstance.CastSpell(playerUnit.SelectedSkill);
            StartCoroutine(PlayerAttack());
        }
    }

    private void OnMove4ButtonClick()
    {
        if (MagicSystemInstance.Spells.Count > 3)
        {
            moveSelectorText.SetActive(false);
            dialogText.enabled = true;
            playerUnit.SelectedSkill = MagicSystemInstance.Spells[3];
            MagicSystemInstance.CastSpell(playerUnit.SelectedSkill);
            StartCoroutine(PlayerAttack());
        }
    }



    

    IEnumerator StartBattle()
    {
        GameObject playerGo = Instantiate(playerPrefab,playerBattleStation);
        playerUnit = playerGo.GetComponent<Player>();
        
        GameObject enemyGo = Instantiate(enemyPrefab,enemyBattleStation);
        enemyUnit = enemyGo.GetComponent<NonPlayer>();

        dialogText.text = enemyUnit.enemyName + " aproaches...";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        NextTurn();
        //PlayerTurn();
        
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        enemyHUD.SetHp(enemyUnit.HealthPoints);
        dialogText.text = "The attack is successful!";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
            // End the battle
        }
        else
        {
            state = BattleState.ENEMYTURN;
            NextTurn();
            //StartCoroutine(EnemyTurn());
            // Enemy turn
        }
    }



 
    
    void PlayerTurn()
    {
        moveSelectorText.SetActive(true);
        dialogText.enabled = false;
        dialogText.text = "Choose an action:";
    }

    IEnumerator EnemyTurn()
    {
        dialogText.text = enemyUnit.enemyName + " attacks!";

        yield return new WaitForSeconds(1f);

        enemyUnit.Attack(playerUnit);
        
        if(playerUnit.HealthPoints <= 0 )
        {
            isDead = true;
        }
        else
        {
            isDead = false;
        }
        

        playerHUD.SetHp(playerUnit.HealthPoints);

        yield return new WaitForSeconds(1f);

        if(isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            
            state = BattleState.PLAYERTURN;
            NextTurn();
            //PlayerTurn();
        }
    }

    public void EndBattle()
    {
        if(state == BattleState.WON)
        {
            dialogText.text = "You won the battle!";
            SceneManager.LoadScene("MainScene");
            enemyUnit.DropLoot();
        }
        else if (state == BattleState.LOST)
        {
            dialogText.text = "You were defeated";
        }
    }

    public void NextTurn()
    {
        currentIndex = (currentIndex + 1) % 2;

        

        // Check if the current state is the player's turn
        if (turnOrder[currentIndex] == BattleState.PLAYERTURN)
        {
            // Do something for the player's turn
            PlayerTurn();
        }
        else if (turnOrder  [currentIndex] == BattleState.ENEMYTURN)
        {
            // Do something for the enemy's turn
            StartCoroutine(EnemyTurn());
        }
    }

    public void OnAttackButton()
    {
        if(state != BattleState.PLAYERTURN)
        {
            return;
        }
        StartCoroutine(PlayerAttack());
        
    }


    public void PlayerMoveNames(List<Skill> skill)
    {
        if (skill[0].SkillName != null)
        {
            SetButtonText(move1ButtonText,skill[0].SkillName);
        }
        else
        {
            SetButtonText(move1ButtonText,"-");
        }

        if (skill[1].SkillName != null)
        {
            SetButtonText(move2ButtonText,skill[1].SkillName);
        }
        else
        {
            SetButtonText(move2ButtonText,"-");
        }

        if (skill[2].SkillName != null)
        {
            SetButtonText(move3ButtonText,skill[2].SkillName);
        }
        else
        {
            SetButtonText(move3ButtonText,"-");
        }

        if (skill[3].SkillName != null)
        {
            SetButtonText(move4ButtonText,skill[3].SkillName);
        }
        else
        {
            SetButtonText(move4ButtonText,"-");
        }
    }
}