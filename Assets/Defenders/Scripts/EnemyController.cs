using System.Collections;
using UnityEngine;
using Anima2D;

public abstract class EnemyController : MonoBehaviour
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
    [HideInInspector]
    public float shootAngleError = 0;              //We use this to give some erros to enemy shoots. Setting this to 0 will results in accurate shoots
    public static float fakeWindPower = 0;              //We use this if we need to add more randomness to enemy shots.
    [HideInInspector]
    public int enemyCurrentHealth;                //not editable.
    [HideInInspector]
    public bool isEnemyDead;                 //flag for gameover event

    //Hidden gameobjects
    [HideInInspector]
    public GameController gc;                          //game controller object

    [Header("Audio Clips")]
    public AudioClip[] shootSfx;
    public AudioClip[] hitSfx;


    [HideInInspector]
    public EnemyPool poolRef;
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
        isEnemyDead = false;
        var g = GameObject.FindGameObjectWithTag("GameController");
        gc = g.GetComponent<GameController>();

        audioSource = GetComponent<AudioSource>();

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

        //setStartingPosition();
        poolRef = GetComponentInParent<EnemyPool>();
    }

    /// <summary>
    /// Plays the sfx.
    /// </summary>
    public void playSfx(AudioClip _clip)
    {
        audioSource.clip = _clip;
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public abstract void InitEnemy(int id, enemySkillLevels enemySkillLevel, string objName);
    public abstract void LetMeFly();
    public abstract void playRandomHitSound();
}
