using System.Collections;
using UnityEngine;
using Anima2D;

public class EnemyBomberController : EnemyController
{

    /// <summary>
    /// Main enemy controller class.
    /// This class handles enemy difficulty, enemy health, shoot AI, body rotation, movement and dying sequences.
    /// </summary>

    [Header("Public GamePlay settings")]
    public float moveSpeed;

    [Header("Audio Clips")]
    public AudioClip[] attackSfx;

    //Enemy shoot settings
    private bool canAttack;
    private bool canWalk;

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

        //if we have killed the player, but the controller has not finished the game yet
        if (GameController.noMoreShooting)
            return;

        if (canAttack)
            StartCoroutine(performAttack());
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var collisionLayer = collision.gameObject.layer;
        if (collisionLayer == GameController.towerLayer)
        {
            Debug.Log("bomb!");
            Destroy(gameObject, 1f);
        }
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
        if (attackSfx.Length > 0)
        {
            playSfx(attackSfx[Random.Range(0, attackSfx.Length)]);
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
