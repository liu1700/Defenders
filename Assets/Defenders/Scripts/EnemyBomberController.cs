using System.Collections;
using UnityEngine;
using Anima2D;
using EZCameraShake;

public class EnemyBomberController : EnemyController
{

    /// <summary>
    /// Main enemy controller class.
    /// This class handles enemy difficulty, enemy health, shoot AI, body rotation, movement and dying sequences.
    /// </summary>

    [Header("Public GamePlay settings")]
    public float moveSpeed;

    //Enemy shoot settings
    private bool canAttack;
    private bool canWalk;

    public GameObject explodeObj;

    Rigidbody2D rigidbody;

    //Init
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        canWalk = true;
    }

    public override void InitEnemy(enemySkillLevels enemySkillLevel, string objName)
    {
        enemySkill = enemySkillLevel;
        //enemyId = id;
    }


    public override void LetMeFly()
    {
        canWalk = false;
        GetComponent<BoxCollider2D>().enabled = false;
        //GetComponentInChildren<BodyController>().ActiveRigidBodys();
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
    }

    private void FixedUpdate()
    {
        if (canWalk)
        {
            WalkForward();
        }
    }

    void WalkForward()
    {
        rigidbody.velocity = new Vector2(moveSpeed, rigidbody.velocity.y);
    }

    void Explode(Vector2 contactPoint)
    {
        GameObject exp = Instantiate(explodeObj, contactPoint, Quaternion.Euler(0, 0, 0)) as GameObject;
        exp.name = "Explosion";
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var collisionLayer = collision.gameObject.layer;
        if (collisionLayer == GameController.towerLayer)
        {
            canWalk = false;

            rigidbody.velocity = new Vector2(0, 0);

            CameraShaker.Instance.ShakeOnce();
            Explode(collision.contacts[0].point);

            GameController.playerController.AddHealth(-MasterWeaponManager.bombExplosionDamage);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            rigidbody.AddForce(new Vector2(20f, 20f), ForceMode2D.Impulse);
            rigidbody.AddTorque(10f, ForceMode2D.Impulse);
            Destroy(gameObject, 1f);
        }
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
