using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLauncherController : MonoBehaviour
{

    /// <summary>
    /// Manages all things related to player and enemy weapons after shot. Including starting force, rotation and collisions.
    /// </summary>

    //we need to know if this weapon is for player or the enemy
    //ID 0 = player
    //ID 1 = enemy
    //ID 2 = Demo character
    internal int ownerID;

    /// <summary>
    /// Player specific variables
    /// </summary>
    //We set this variable from PlayerController class upon shoot command (releasing the touch)
    //internal Vector3 playerShootVector;
    internal Vector2 playerShootVector;

    /// <summary>
    /// Enemy specific variables
    /// </summary>
    internal float enemyShootAngle;         // shooting angle (set from EnemyController class upon shoot command) 

    //only check for collision after a few seconds passed after the shot
    private float timeOfShot;                   //time of the creation of this projectile
    private float collisionCheckDelay = 0.1f;   //seconds.

    ////reference to player and enemy game objects
    ////private GameObject enemy;
    //private GameObject player;

    //effect objects
    public GameObject bloodFx;
    public GameObject trailFx;
    public GameObject[] explosionFxs;

    //all available arrow types (Each with their unique behaviours)
    public enum arrowTypes { Arrow, Grenade, Sword, Axe, Bomb }
    public arrowTypes arrowType = arrowTypes.Arrow;

    public static int damage;       //damage this weapon deals to target
    private bool stopUpdate;        //we need to stop this weapon after collision
    //private bool bypassCode;        //we use this for the segments of our class that requires an enemy, but the current gameMode does not need an enemy

    //special weapon-specific flags
    //private bool bombExplosionByPlayer;		//flag to set if explosion has been done by player input
    //private bool bombExplosionByEnemy;		//flag to set if explosion has been done by enemy AI

    Rigidbody2D arrRigid;
    BoxCollider2D arrCollider;

    int enemyShootingLayer;
    int playerShootingLayer;

    void Awake()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        arrRigid = GetComponent<Rigidbody2D>();
        arrCollider = GetComponent<BoxCollider2D>();

        switch (arrowType)
        {
            case arrowTypes.Arrow:
                damage = MasterWeaponManager.arrowDamage;
                break;
            case arrowTypes.Axe:
                damage = MasterWeaponManager.axeDamage;
                break;
            case arrowTypes.Bomb:
                damage = MasterWeaponManager.bombDamage;
                break;
            case arrowTypes.Grenade:
                damage = MasterWeaponManager.grenadeDamage;
                break;
            case arrowTypes.Sword:
                damage = MasterWeaponManager.swordDamage;
                break;
        }

        enemyShootingLayer = LayerMask.NameToLayer("enemyShooting");
        playerShootingLayer = LayerMask.NameToLayer("playerShooting");
    }

    void Start()
    {

        //set shoot time
        timeOfShot = Time.time;

        stopUpdate = false;

        //if the owner of this weapon is player, just add force to shoot it, 
        //as all calculations has already been done by payerController
        if (ownerID == 0 || ownerID == 2)
        {

            //add force and let the weapon fly
            if (playerShootVector.magnitude > 0)
            {
                //GetComponent<Rigidbody>().AddForce(playerShootVector, ForceMode.Impulse);
                arrRigid.AddForce(playerShootVector, ForceMode2D.Impulse);
                gameObject.layer = playerShootingLayer;
            }
        }
        else if (ownerID == 1)
        {
            enemyShoot();
            gameObject.layer = enemyShootingLayer;
        }

        //we can destroy the arrow after a short time (if it is not already destroyed)
        Destroy(gameObject, 10);
    }


    void Update()
    {

        if (arrowType == arrowTypes.Arrow)
        {
            manageArrowRotation();
        }

        //bug prevention
        if (transform.position.y < -8)
            Destroy(gameObject);
    }

    /// <summary>
    /// Enemy object just commands the shot sequence. All calculations for the actual arrow happens here.
    /// </summary>
    private void enemyShoot()
    {

        // get initial and target positions
        Vector3 pos = transform.position;
        Vector3 target = GameController.playerController.transform.position;

        // get distance
        float dist = Vector3.Distance(pos, target);

        // calculate initival velocity required for the arrow to move through distance
        float Vi = Mathf.Sqrt(dist * -Physics.gravity.y / (Mathf.Sin(Mathf.Deg2Rad * enemyShootAngle * 1.05f)));
        float Vy, Vx;

        Vx = Vi * Mathf.Cos(Mathf.Deg2Rad * enemyShootAngle);
        Vy = Vi * Mathf.Sin(Mathf.Deg2Rad * enemyShootAngle);

        // create the velocity vector
        Vector3 localVelocity = new Vector3(-Vx, Vy, 0);
        Vector3 globalVelocity = transform.TransformVector(localVelocity);

        // shoot the arrow
        arrRigid.velocity = globalVelocity;

        //add a little wind (stochastic destination) to make the enemy weapon more realistic
        arrRigid.AddForce(new Vector3(EnemyController.fakeWindPower, 0, 0), ForceMode2D.Force);
    }


    /// <summary>
    /// Manages the arrow rotation based on its position on the move curve.
    /// </summary>
    private Vector3 v;      //velocity
    private float zDir;
    private float yDir;
    void manageArrowRotation()
    {

        //if (GetComponent<Rigidbody>().velocity != Vector3.zero && !stopUpdate)
        if (arrRigid.velocity != Vector2.zero && !stopUpdate)
        {
            v = arrRigid.velocity;

            if (ownerID == 0 || ownerID == 2)
            {
                zDir = (Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg) + 180;
                yDir = Mathf.Atan2(v.z, v.x) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0, -yDir, zDir);
            }
            else if (ownerID == 1)
            {
                zDir = (Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg);
                yDir = Mathf.Atan2(v.z, v.x) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0, yDir, -zDir);
            }
        }
    }


    void rotateWeapon(float rotationSpeed)
    {

        if (arrRigid.velocity != Vector2.zero && !stopUpdate)
        {
            transform.eulerAngles = new Vector3(0, 180, Time.timeSinceLevelLoad * rotationSpeed);
        }
    }

    void explodeOnTouch(Vector2 contactPoint)
    {
        GameObject exp = Instantiate(explosionFxs[Random.Range(0, explosionFxs.Length)], contactPoint, Quaternion.Euler(0, 0, 0)) as GameObject;
        exp.name = "Explosion";
    }


    /// <summary>
    /// Check for collisions
    /// </summary>
    /// 
    private bool isChecking = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (isChecking)
            return;

        isChecking = true;

        var collisionLayerMask = collision.gameObject.layer;
        if ((collisionLayerMask == GameController.enemyLayer || collisionLayerMask == GameController.bomberEnemyLayer) && gameObject.layer == playerShootingLayer)
        {
            //disable the arrow
            stopUpdate = true;

            arrRigid.gravityScale = 0;
            //arrRigid.isKinematic = true;

            if (arrCollider)
                arrCollider.enabled = false;

            trailFx.SetActive(false);
            GameController.isArrowInScene = false;
            transform.parent = collision.collider.gameObject.transform;

            //create blood fx
            if (gameObject.tag == "arrow" || gameObject.tag == "axe")
            {
                GameObject blood = Instantiate(bloodFx, collision.contacts[0].point, Quaternion.Euler(0, 0, 0)) as GameObject;
                blood.name = "BloodFX";
                blood.transform.parent = collision.gameObject.transform;
                Destroy(blood, 1.5f);
            }

            var enemy = collision.collider.gameObject.GetComponentInParent<EnemyController>();

            //manage victim's helath status
            enemy.enemyCurrentHealth -= damage;
            if (enemy.enemyCurrentHealth <= 0)
            {
                enemy.LetMeFly();
                var bodyPart = collision.collider.gameObject.GetComponent<Rigidbody2D>();
                if (bodyPart)
                {
                    bodyPart.velocity = Vector2.right * 50;
                    bodyPart.gravityScale = 3f;
                }
            }

            //play hit sfx
            enemy.playRandomHitSound();

            // sleep it
            arrRigid.Sleep();
        }
        else if (collisionLayerMask == GameController.towerLayer && gameObject.layer == enemyShootingLayer)
        {
            stopUpdate = true;
            arrRigid.gravityScale = 0;
            arrRigid.bodyType = RigidbodyType2D.Static;
            arrRigid.Sleep();

            if (arrCollider)
                arrCollider.enabled = false;

            trailFx.SetActive(false);

            GameController.isArrowInScene = false;

            transform.parent = collision.gameObject.transform;

            //manage victim's helath status
            GameController.playerController.AddHealth(-damage);

            //play hit tower sfx
            GameController.playerController.playRandomHitTowerSound();

            Destroy(gameObject, 10f);
        }
        else if (collisionLayerMask == GameController.playerLayer && gameObject.layer == enemyShootingLayer)
        {
            stopUpdate = true;
            arrRigid.gravityScale = 0;
            arrRigid.bodyType = RigidbodyType2D.Static;
            arrRigid.Sleep();

            if (arrCollider)
                arrCollider.enabled = false;

            trailFx.SetActive(false);

            GameController.isArrowInScene = false;

            transform.parent = collision.gameObject.transform;

            //create blood fx
            GameObject blood = Instantiate(bloodFx, collision.contacts[0].point, Quaternion.Euler(0, 0, 0)) as GameObject;
            blood.name = "BloodFX";
            Destroy(blood, 1.5f);

            //manage victim's helath status
            GameController.playerController.AddHealth(-damage);

            //play hit sfx
            GameController.playerController.playRandomHitSound();

            Destroy(gameObject, 10f);
        }
        else if ((collisionLayerMask == playerShootingLayer && gameObject.layer == enemyShootingLayer) ||
          (collisionLayerMask == enemyShootingLayer && gameObject.layer == playerShootingLayer))
        {
            // 敌我的箭相撞
            explodeOnTouch(collision.contacts[0].point);
            Destroy(gameObject, .1f);
            Destroy(collision.gameObject, .1f);

            var gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            gc.AddGold(8);
            gc.addBonusTime(5);
        }
        else if (collisionLayerMask == GameController.environmentLayer)
        {
            //disable the arrow
            stopUpdate = true;
            arrRigid.gravityScale = 0;
            arrRigid.bodyType = RigidbodyType2D.Static;
            arrRigid.Sleep();

            if (arrCollider)
                arrCollider.enabled = false;

            trailFx.SetActive(false);

            GameController.isArrowInScene = false;

            Destroy(gameObject, 2f);
        }
    }
}
