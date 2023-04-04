using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START,START2, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{

    public GameObject Player;
    public GameObject Enemy1; //The first enemy
    public GameObject Enemy2; //The second enemy
    public GameObject Enemy3; //The third enemy
    public GameObject Enemy4; //The final enemy

    public Transform playerPosition;
    public Transform enemyPosition;

    Unit playerUnit;
    Unit enemyUnit;

    public Text dialogueText;

    public BattleHud playerHUD; // The battle HUDs for the player and enemy
    public BattleHud enemyHUD;

    public AudioSource playerAttackSound; //Here are all the sound effects for the game
    public AudioSource playerUltimateSound;
    public AudioSource enemy1AttackSound;
    public AudioSource enemy2AttackSound;
    public AudioSource enemy3AttackSound;
    public AudioSource enemy4AttackSound;
    public AudioSource damageTakenPlayerSound;
    public AudioSource damage1TakenEnemySound;
    public AudioSource damage2TakenEnemySound;
    public AudioSource damage3TakenEnemySound;
    public AudioSource bossAppears;

    public int enemyUltimate=0; //Enemy ultimate starts from 0
    public int playerUltimate = 0; //Player ultimate starts from 0
    public int whichEnemy = 1;
    public int dodge = 0;
    public int playerUltimateHeal=25;

    public bool readyToAttack = false;

    public BattleState state;

    GameObject playerGo;
    GameObject enemyGo;

    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle1());
    }

    IEnumerator SetupBattle1() //This is the fight with first enemy, everything gets initialized, the player, the enemy and their battle huds.
    {
        playerHUD.SetUltimate(playerUltimate);
        playerGo = Instantiate(Player, playerPosition);
        playerUnit = playerGo.GetComponent<Unit>();

        enemyGo = Instantiate(Enemy1, enemyPosition);
        enemyUnit = enemyGo.GetComponent<Unit>();

        dialogueText.text = "An angry " + enemyUnit.unitName + " has come from a bar";

        playerHUD.SetHud(playerUnit);
        enemyHUD.SetHud(enemyUnit);

        yield return new WaitForSeconds(3f);
        readyToAttack= true;

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }
    IEnumerator SetupBattle2() //This loads the second enemy after the first one gets defeated.
    {
        playerHUD.SetUltimate(playerUltimate);
        enemyGo = Instantiate(Enemy2, enemyPosition);
        enemyUnit = enemyGo.GetComponent<Unit>();
        enemyUltimate = 0;
        dialogueText.text = "An angry " + enemyUnit.unitName + " has come from a bar";

        enemyHUD.SetHud(enemyUnit);

        yield return new WaitForSeconds(3f);
        readyToAttack = true;

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }
    IEnumerator SetupBattle3()
    {
        playerHUD.SetUltimate(playerUltimate);
        enemyGo = Instantiate(Enemy3, enemyPosition);
        enemyUnit = enemyGo.GetComponent<Unit>();
        enemyUltimate = 0;
        dialogueText.text = "An angry " + enemyUnit.unitName + " has come from a bar";

        enemyHUD.SetHud(enemyUnit);

        yield return new WaitForSeconds(3f);
        readyToAttack = true;

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }
    IEnumerator SetupBattle4()
    {
        playerHUD.SetUltimate(playerUltimate);
        enemyGo = Instantiate(Enemy4, enemyPosition);
        enemyUnit = enemyGo.GetComponent<Unit>();
        enemyUltimate = 0;
        dialogueText.text = "An angry " + enemyUnit.unitName + " has come from a bar";

        enemyHUD.SetHud(enemyUnit);
        bossAppears.Play();
        yield return new WaitForSeconds(3f);
        readyToAttack = true;

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }
    IEnumerator PlayerAttack() //This is where the game checks if the attack of the player will lower the enemies health or it will defeat them.
    {
        bool isDead;
        playerUltimate+=1;//For every attack it's increamenting the ultimate.
        playerHUD.SetUltimate(playerUltimate);
        if (dodge != 1)//This checks if the enemy is not dodging the next attack.
        {
            playerAttackSound.Play();
            //Player attacks
            isDead = enemyUnit.TakeDamage(playerUnit.damage);

            enemyHUD.SetHP(enemyUnit.currentHP);
            dialogueText.text = "The enemy is hurt!!";
            
            yield return new WaitForSeconds(2f);

            if (whichEnemy == 1) {
                damage1TakenEnemySound.Play();
            }else if (whichEnemy == 2)
            {
                damage2TakenEnemySound.Play();
            }else if (whichEnemy == 3)
            {
                damage3TakenEnemySound.Play();
            }
            else
            {
                damage1TakenEnemySound.Play();
            }

            yield return new WaitForSeconds(3f);
            readyToAttack = false;
        }
        else
        {
            isDead = enemyUnit.TakeDamage(0);
            dialogueText.text = "He dodged my attack";
            yield return new WaitForSeconds(2f);
            dodge = 0;
            readyToAttack = false;
        }

        //Check if the enemy is dead, if yes then player wins, if not go to enemy turn
        if (isDead) //If the current enemy is defeated the game will increase the stats of the player and it will move on to the next fight, if the player will defeat the last one it will tick the win condition.
        {
            if (whichEnemy == 1)
            {
                state = BattleState.START2;
                playerUnit.unitLevel += 1;
                playerUnit.damage += 2;
                playerUnit.ultimateDamage += 4;
                playerUnit.defense += 2;
                playerUnit.maxHP += 10;
                playerUnit.currentHP += 10;
                playerUltimateHeal += 5;
                whichEnemy = 2;
                readyToAttack = false;
                playerHUD.SetHP(playerUnit.currentHP);
                playerHUD.SetLVL(playerUnit.unitLevel);
                Destroy(enemyGo);
                StartCoroutine(SetupBattle2());
            }
            else if (whichEnemy == 2)
            {
                state = BattleState.START2;
                playerUnit.unitLevel += 1;
                playerUnit.damage += 2;
                playerUnit.ultimateDamage += 4;
                playerUnit.defense += 2;
                playerUnit.maxHP += 10;
                playerUnit.currentHP += 10;
                playerUltimateHeal += 5;
                whichEnemy = 3;
                readyToAttack = false;
                playerHUD.SetHP(playerUnit.currentHP);
                playerHUD.SetLVL(playerUnit.unitLevel);
                Destroy(enemyGo);
                StartCoroutine(SetupBattle3());
            }
            else if (whichEnemy == 3)
            {
                state = BattleState.START2;
                playerUnit.unitLevel += 1;
                playerUnit.damage += 2;
                playerUnit.ultimateDamage += 4;
                playerUnit.defense += 2;
                playerUnit.maxHP += 10;
                playerUnit.currentHP += 10;
                playerUltimateHeal += 5;
                whichEnemy = 4;
                readyToAttack = false;
                playerHUD.SetHP(playerUnit.currentHP);
                playerHUD.SetLVL(playerUnit.unitLevel);
                Destroy(enemyGo);
                StartCoroutine(SetupBattle4());
            }
            else
            {
                //End the battle
                state = BattleState.WON;
                Destroy(enemyGo);
                EndBattle();
            }
        }
        else
        {
            readyToAttack = false;
            //Continue with the enemy turn
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
        //Change the state based on that
    }
    IEnumerator PlayerUltimate()
    {
        bool isDead;
        playerUltimate = 0;
        playerHUD.SetUltimate(playerUltimate);
        if (dodge != 1)
        {
            playerUltimateSound.Play();
            isDead = enemyUnit.TakeDamage(playerUnit.ultimateDamage);

            enemyHUD.SetHP(enemyUnit.currentHP);
            playerUnit.damage += 6;
            playerUnit.ultimateDamage += 4;
            playerUnit.Heal(playerUltimateHeal);
            playerHUD.SetHP(playerUnit.currentHP);

            dialogueText.text = "The enemy is grievously hurt!!";
            yield return new WaitForSeconds(2f);
            if (whichEnemy == 1)
            {
                damage1TakenEnemySound.Play();
            }
            else if (whichEnemy == 2)
            {
                damage2TakenEnemySound.Play();
            }
            else if (whichEnemy == 3)
            {
                damage3TakenEnemySound.Play();
            }
            else
            {
                damage1TakenEnemySound.Play();
            }

            yield return new WaitForSeconds(3f);
            readyToAttack = false;
        }
        else
        {
            isDead = enemyUnit.TakeDamage(0);
            dialogueText.text = "He dodged my attack";
            yield return new WaitForSeconds(2f);
            dodge = 0;
            readyToAttack = false;
        }

        //Check if the enemy is dead, if yes then player wins, if not it goes to the enemy turn
        if (isDead)
        {
            if (whichEnemy == 1)
            {
                state = BattleState.START2;
                playerUnit.unitLevel += 1;
                playerUnit.damage += 2;
                playerUnit.ultimateDamage += 4;
                playerUnit.defense += 2;
                playerUnit.maxHP += 10;
                playerUnit.currentHP += 10;
                playerUltimateHeal += 5;
                whichEnemy = 2;
                readyToAttack = false;
                enemyUltimate = 0;
                playerHUD.SetHP(playerUnit.currentHP);
                playerHUD.SetLVL(playerUnit.unitLevel);
                Destroy(enemyGo);
                StartCoroutine(SetupBattle2());
            }
            else if (whichEnemy == 2)
            {
                state = BattleState.START2;
                playerUnit.unitLevel += 1;
                playerUnit.damage += 2;
                playerUnit.ultimateDamage += 4;
                playerUnit.defense += 2;
                playerUnit.maxHP += 10;
                playerUnit.currentHP += 10;
                whichEnemy =3;
                playerUltimateHeal += 5;
                readyToAttack = false;
                enemyUltimate = 0;
                playerHUD.SetHP(playerUnit.currentHP);
                playerHUD.SetLVL(playerUnit.unitLevel);
                Destroy(enemyGo);
                StartCoroutine(SetupBattle3());
            }
            else if (whichEnemy == 3)
            {
                state = BattleState.START2;
                playerUnit.unitLevel += 1;
                playerUnit.damage += 2;
                playerUnit.ultimateDamage += 4;
                playerUnit.defense += 2;
                playerUnit.maxHP += 10;
                playerUnit.currentHP += 10;
                playerUltimateHeal += 5;
                enemyUltimate = 0;
                whichEnemy =4;
                readyToAttack = false;
                playerHUD.SetHP(playerUnit.currentHP);
                playerHUD.SetLVL(playerUnit.unitLevel);
                Destroy(enemyGo);
                StartCoroutine(SetupBattle4());
            }
            else
            {
                //End the battle
                state = BattleState.WON;
                Destroy(enemyGo);
                EndBattle();
            }
        }
        else
        {
            readyToAttack = false;
            //Continue with the enemy turn
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }
    IEnumerator EnemyTurn()//This is the "A.I." for each enemy, each one will attack the enemy, but if it has the ultimate ready it will use that.
    {
        if (whichEnemy == 4)
        {
            bool isDead;           
            if (enemyUltimate < enemyUnit.maxUltimate)
            {
                enemy4AttackSound.Play();
                dialogueText.text = enemyUnit.unitName + " attacks!";

                yield return new WaitForSeconds(1f);

                isDead = playerUnit.TakeDamage(enemyUnit.damage);

                playerHUD.SetHP(playerUnit.currentHP);
                enemyUltimate += 1;
                enemyHUD.SetUltimate(enemyUltimate);
                yield return new WaitForSeconds(1f);

                damageTakenPlayerSound.Play();
            }
            else // each enemy has a different ultimate.
            {
                dialogueText.text = enemyUnit.unitName + " uses it's ultimate!";
                isDead = playerUnit.TakeDamage(0);
                yield return new WaitForSeconds(1f);
                dialogueText.text = "He gains extra defense";
                enemyUnit.Defense(8);
                enemyUltimate = 0;
                enemyHUD.SetUltimate(enemyUltimate);
            }

            yield return new WaitForSeconds(2f);

            if (isDead)
            {
                state = BattleState.LOST;
                EndBattle();
            }
            else
            {
                state = BattleState.PLAYERTURN;
                PlayerTurn();
            }
        }
        else if (whichEnemy == 3)
        {
            bool isDead;         
            if (enemyUltimate < enemyUnit.maxUltimate)
            {
                enemy3AttackSound.Play();
                dialogueText.text = enemyUnit.unitName + " attacks!";

                yield return new WaitForSeconds(1f);

                isDead = playerUnit.TakeDamage(enemyUnit.damage);

                playerHUD.SetHP(playerUnit.currentHP);
                enemyUltimate += 1;
                enemyHUD.SetUltimate(enemyUltimate);
                yield return new WaitForSeconds(1f);

                damageTakenPlayerSound.Play();
            }
            else
            {
                dialogueText.text = enemyUnit.unitName + " uses it's ultimate!";
                isDead = playerUnit.TakeDamage(0);
                yield return new WaitForSeconds(1f);
                dialogueText.text = "He stuns you";
                enemyUltimate = 0;
                enemyHUD.SetUltimate(enemyUltimate);
                isDead = playerUnit.TakeDamage(enemyUnit.ultimateDamage);
                playerHUD.SetHP(playerUnit.currentHP);
                damageTakenPlayerSound.Play();
                yield return new WaitForSeconds(1f);
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }

            yield return new WaitForSeconds(2f);

            if (isDead)//Here it checks if the player has died after the enemy turn, if not it will change the state to the player turn.
            {
                state = BattleState.LOST;
                EndBattle();
            }
            else
            {
                state = BattleState.PLAYERTURN;
                PlayerTurn();
            }
        }
        else if (whichEnemy == 2)
        {
            bool isDead;        
            if (enemyUltimate < enemyUnit.maxUltimate)
            {
                enemy2AttackSound.Play();
                dialogueText.text = enemyUnit.unitName + " attacks!";

                yield return new WaitForSeconds(1f);

                isDead = playerUnit.TakeDamage(enemyUnit.damage);

                playerHUD.SetHP(playerUnit.currentHP);
                enemyUltimate += 1;
                enemyHUD.SetUltimate(enemyUltimate);
                yield return new WaitForSeconds(1f);
                damageTakenPlayerSound.Play();
            }
            else
            {
                dialogueText.text = enemyUnit.unitName + " uses it's ultimate!";
                isDead = playerUnit.TakeDamage(0);
                yield return new WaitForSeconds(1f);
                dialogueText.text = "He will dodge my next attack";
                dodge = 1;
                enemyUltimate = 0;
                enemyHUD.SetUltimate(enemyUltimate);
            }         

            yield return new WaitForSeconds(2f);

            if (isDead)
            {
                state = BattleState.LOST;
                EndBattle();
            }
            else
            {
                state = BattleState.PLAYERTURN;
                PlayerTurn();
            }
        }
        else
        {
            bool isDead;
            
            if (enemyUltimate < enemyUnit.maxUltimate)
            {
                enemy1AttackSound.Play();
                dialogueText.text = enemyUnit.unitName + " attacks!";

                yield return new WaitForSeconds(1f);

                isDead = playerUnit.TakeDamage(enemyUnit.damage);

                playerHUD.SetHP(playerUnit.currentHP);
                enemyUltimate += 1;
                enemyHUD.SetUltimate(enemyUltimate);
                yield return new WaitForSeconds(1f);

                damageTakenPlayerSound.Play();
            }
            else
            {
                dialogueText.text = enemyUnit.unitName + " uses it's ultimate!";
                isDead = playerUnit.TakeDamage(0);
                yield return new WaitForSeconds(1f);
                dialogueText.text = "He heals";
                enemyUnit.Heal(10);
                enemyHUD.SetHP(enemyUnit.currentHP);
                enemyUltimate = 0;
                enemyHUD.SetUltimate(enemyUltimate);
            }
            

            yield return new WaitForSeconds(2f);

            if (isDead)
            {
                state = BattleState.LOST;
                EndBattle();
            }
            else
            {
                state = BattleState.PLAYERTURN;
                PlayerTurn();
            }
        }
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "We won my master!";
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "He's too strong my master.";
        }
    }

    void PlayerTurn() //This informs the player that is there turn
    {
        readyToAttack = true;
        dialogueText.text = "Give me an order my master";
    }

    public void OnAttackButton()//This actives the attack function when the button is pressed
    {
        if (state != BattleState.PLAYERTURN || readyToAttack == false) return;

        StartCoroutine(PlayerAttack());
    }

    public void OnUltimateButton()//This actives the ultimate function when the button is pressed
    {
        if(state != BattleState.PLAYERTURN||readyToAttack==false) return;
        if(playerUltimate>=playerUnit.maxUltimate)//here it checks if the player has enough energy for the ultimate
            StartCoroutine(PlayerUltimate());
        else
        {
            dialogueText.text = "I can't do it now master!";
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

}
