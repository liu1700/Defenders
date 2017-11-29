using System.Collections;
using UnityEngine;
using Anima2D;

public class EnemyArcherController : EnemyController
{

    /// <summary>
    /// Main enemy controller class.
    /// This class handles enemy difficulty, enemy health, shoot AI, body rotation, movement and dying sequences.
    /// </summary>

    ////Difficulty settings
    //public enum enemySkillLevels { easy, normal, hard, Robinhood }
    //public enemySkillLevels enemySkill = enemySkillLevels.easy;

    //public int enemyId;

    public DestructableController plateformController;

    //[Header("Public GamePlay settings")]
    //public int enemyHealth = 100;                   //initial (full) health. can be edited.
    //public float baseShootAngle = 23f;           //Very important! - avoid editing this value! (it has been calculated based on the size/shape/weight of the arrow) preVal 61.5
    //private float shootAngleError = 0;              //We use this to give some erros to enemy shoots. Setting this to 0 will results in accurate shoots
    //public static float fakeWindPower = 0;              //We use this if we need to add more randomness to enemy shots.
    //internal int enemyCurrentHealth;                //not editable.
    //public bool isEnemyDead;                 //flag for gameover event

    //[Header("Linked GameObjects")]
    ////Reference to game objects (childs and prefabs)
    //public GameObject arrow;
    //public Control enemyTurnPivot;
    //public GameObject enemyShootPosition;
    ////Hidden gameobjects
    //private GameController gc;                          //game controller object

    //[Header("Audio Clips")]
    //public AudioClip[] shootSfx;
    //public AudioClip[] hitSfx;

    ////Enemy shoot settings
    //private bool canShoot;

    //EnemyPool poolRef;

    //AudioSource audioSource;
    [Header("Linked GameObjects")]
    //Reference to game objects (childs and prefabs)
    public GameObject arrow;
    public Control enemyTurnPivot;
    public GameObject enemyShootPosition;

    void Start()
    {
        StartCoroutine(reactiveEnemyShoot());
    }


    /// <summary>
    /// move the enemy to a random position, each time the game begins
    /// </summary>
    public void setStartingPosition(Vector3 pos)
    {
        //float randomX = Random.Range(15, 60);
        transform.position = pos;
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

        if (canShoot)
            StartCoroutine(shootArrow());
    }

    /// <summary>
    /// Enemy shoot AI.
    /// We just need to create the enemy-Arrow and feed it with initial shoot angle. It will calculate the shoot-power itself.
    /// </summary>
    IEnumerator shootArrow()
    {

        if (!canShoot)
            yield break;

        canShoot = false;

        //set the unique flag
        GameController.isArrowInScene = true;

        if (isEnemyDead)
        {
            yield break;
        }

        float finalShootAngle = baseShootAngle + Random.Range(-shootAngleError, shootAngleError);

        //we need to rotate enemy body to a random/calculated rotation angle
        //float targetAngle = Random.Range(25, 40) * -1;  //important! (originate from 55)
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            //enemyTurnPivot.transform.rotation = Quaternion.Euler(0, 0, Mathf.SmoothStep(-90, targetAngle - 90, t));
            enemyTurnPivot.transform.rotation = Quaternion.Euler(0, 0, Mathf.SmoothStep(-90, -finalShootAngle - 90, t));
            yield return 0;
        }

        if (isEnemyDead)
        {
            yield break;
        }
        //play shoot sound
        playSfx(shootSfx[Random.Range(0, shootSfx.Length)]);

        //shoot calculations
        GameObject ea = Instantiate(arrow, enemyShootPosition.transform.position, Quaternion.Euler(0, 0, 10)) as GameObject;
        //ea.layer = GameController.enemyLayer;
        ea.name = "EnemyProjectile";
        ea.GetComponent<MainLauncherController>().ownerID = 1;

        //float finalShootAngle = baseShootAngle + Random.Range(-shootAngleError, shootAngleError);
        //float finalShootAngle = baseShootAngle;
        ea.GetComponent<MainLauncherController>().enemyShootAngle = finalShootAngle;

        //at the end
        StartCoroutine(reactiveEnemyShoot());

        //reset body rotation
        StartCoroutine(resetBodyRotation());
    }


    /// <summary>
    /// tunr enemy body to default rotation
    /// </summary>
    IEnumerator resetBodyRotation()
    {
        yield return new WaitForSeconds(1.5f);
        enemyTurnPivot.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
    }

    /// <summary>
    /// Enable enemy to shoot again
    /// </summary>
    IEnumerator reactiveEnemyShoot()
    {
        yield return new WaitForSeconds(1.5f);
        canShoot = true;
    }

    public override void LetMeFly()
    {
        plateformController.Break();

        GetComponentInChildren<BodyController>().ActiveRigidBodys();
    }

    public override void InitEnemy(int id, enemySkillLevels enemySkillLevel, string objName)
    {
        enemySkill = enemySkillLevel;
        enemyId = id;

        var obj = gameObject.transform.Find(objName).gameObject;
        obj.SetActive(true);
        var ctrl = obj.GetComponentInChildren<Control>(true);
        enemyTurnPivot = ctrl;
        enemyShootPosition = obj.GetComponentInChildren<ShootPos>().gameObject;
    }


    /// <summary>
    /// Play a sfx when enemy is hit by an arrow
    /// </summary>
    public override void playRandomHitSound()
    {
        int rndIndex = Random.Range(0, hitSfx.Length);
        playSfx(hitSfx[rndIndex]);
    }
}
