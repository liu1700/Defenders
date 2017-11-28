using System.Collections;
using UnityEngine;
using Anima2D;

public class EnemyBomberController : MonoBehaviour
{

    /// <summary>
    /// Main enemy controller class.
    /// This class handles enemy difficulty, enemy health, shoot AI, body rotation, movement and dying sequences.
    /// </summary>

    //Difficulty settings
    public enum enemySkillLevels { easy, normal, hard, Robinhood }
    public enemySkillLevels enemySkill = enemySkillLevels.easy;

    public int enemyId;

    [Header("Public GamePlay settings")]
    public int enemyHealth = 100;                   //initial (full) health. can be edited.
    public float baseShootAngle = 23f;           //Very important! - avoid editing this value! (it has been calculated based on the size/shape/weight of the arrow) preVal 61.5
    private float shootAngleError = 0;              //We use this to give some erros to enemy shoots. Setting this to 0 will results in accurate shoots
    public static float fakeWindPower = 0;              //We use this if we need to add more randomness to enemy shots.
    internal int enemyCurrentHealth;                //not editable.
    public bool isEnemyDead;                 //flag for gameover event

    //Hidden gameobjects
    private GameController gc;                          //game controller object

    [Header("Audio Clips")]
    public AudioClip[] attackSfx;
    public AudioClip[] hitSfx;

    //Enemy shoot settings
	private bool canAttack;

    EnemyPool poolRef;
	AudioSource audioSource;

    //Init
    void Awake()
    {

        //First of all, check if we need an enemy with the current game mode
        if (!GameModeController.isEnemyRequired())
        {
            gameObject.SetActive(false);
        }

        enemyCurrentHealth = enemyHealth;
        canAttack = false;
        isEnemyDead = false;
        var g = GameObject.FindGameObjectWithTag("GameController");
        gc = g.GetComponent<GameController>();

		audioSource = GetComponent<AudioSource> ();

        //Increase difficulty by decreasing the enemy error when shooting
        //Please note that "shootAngleError" is not editable. If you want to change the precision, you need to edit "fakeWindPower"
        switch (enemySkill)
        {
            case enemySkillLevels.easy:
                shootAngleError = 15f;
                fakeWindPower = Random.Range(0, 30);
                break;
            case enemySkillLevels.normal:
                shootAngleError = 10f;
                fakeWindPower = Random.Range(0, 20);
                break;
            case enemySkillLevels.hard:
                shootAngleError = 8f;
                fakeWindPower = Random.Range(0, 10);
                break;
            case enemySkillLevels.Robinhood:
                shootAngleError = 2;
                fakeWindPower = 0;
                break;
        }

        poolRef = GetComponentInParent<EnemyPool>();
    }

    /// <summary>
    /// move the enemy to a random position, each time the game begins
    /// </summary>
    public void setStartingPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void InitEnemy(int id, enemySkillLevels enemySkillLevel, string objName)
    {
        enemySkill = enemySkillLevel;
        enemyId = id;

        var obj = gameObject.transform.Find(objName).gameObject;
        obj.SetActive(true);
        var ctrl = obj.GetComponentInChildren<Control>(true);
    }


    public void LetMeFly()
    {
        GetComponentInChildren<BodyController>().ActiveRigidBodys();
    }

    /// <summary>
    /// FSM
    /// </summary>
    void Update()
    {

        //if the game has not started yet, or the game is finished, just return
        if (!GameController.gameIsStarted || GameController.gameIsFinished || isEnemyDead)
            return;

        //Check if this object is dead or alive
        if (enemyCurrentHealth <= 0)
        {
            enemyCurrentHealth = 0;
            isEnemyDead = true;
            var goldCount = 3;
            if (enemySkill == enemySkillLevels.normal)
            {
                goldCount = 5;
            }
            else if (enemySkill == enemySkillLevels.hard || enemySkill == enemySkillLevels.Robinhood)
            {
                goldCount = 7;
            }

            gc.KillEnemies(1, goldCount);
            if (poolRef != null)
            {
                poolRef.KillEnemy(enemyId);
            }
            return;
        }

        //if we have killed the player, but the controller has not finished the game yet
        if (GameController.noMoreShooting)
            return;

        if (canAttack)
			StartCoroutine(performAttack());
    }

    /// <summary>
    /// Enemy shoot AI.
    /// We just need to create the enemy-Arrow and feed it with initial shoot angle. It will calculate the shoot-power itself.
    /// </summary>
    IEnumerator performAttack()
    {
        if (!canAttack)
            yield break;

        canAttack = false;

        if (isEnemyDead)
        {
            yield break;
        }
			
        //play atk sound
		if (attackSfx.Length > 0) {
			playSfx(attackSfx[Random.Range(0, attackSfx.Length)]);
		}
    }

    /// <summary>
    /// Plays the sfx.
    /// </summary>
    void playSfx(AudioClip _clip)
    {
		audioSource.clip = _clip;
		if (!audioSource.isPlaying)
        {
			audioSource.Play();
        }
    }


    /// <summary>
    /// Play a sfx when enemy is hit by an arrow
    /// </summary>
    public void playRandomHitSound()
    {
        int rndIndex = Random.Range(0, hitSfx.Length);
        playSfx(hitSfx[rndIndex]);
    }
}
