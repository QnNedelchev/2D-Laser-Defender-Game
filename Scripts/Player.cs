using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Coroutine firingCoroutine;
    private ColorTint colorTint;
    private float xMin, xMax, yMin, yMax;
    private bool hasFired = false;
    private bool isFiring = false;
    private bool pickedArmor = false;
    private bool fadeIn = false;

    [Header("Player")]
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float padding = 0.5f;
    [SerializeField] private float maxHealth = 200;
    [SerializeField] private float maxArmor = 50;
    [SerializeField] private Stat health;
    [SerializeField] private Stat armor;
    [SerializeField] [Range(0, 1)] float deathSFXvolume;
    [SerializeField] GameObject explosionVFX;
    [SerializeField] AudioClip deathSFX;
    

    [Header("Projectile")]
    [SerializeField] private GameObject laserBeam;
    [SerializeField] private GameObject[] shootingExits;
    [SerializeField] private float firePeriod;
    [SerializeField] private AudioClip laserSFX;
    [SerializeField] [Range(0, 1)] float laserSFXvolume;

    [Header("Shield")]
    [SerializeField] private GameObject shield;
    [SerializeField] private Material shieldDissolveMaterial;
    [SerializeField] private Color shieldTint;
    private Material shieldTintMaterial;
    private ColorTint shieldColorTint;
    private float fade = 1f;

    [SerializeField]
    private Joystick joystick;

    #region Properties

    public ColorTint MyColorTint
    {
        get { return colorTint; }
    }

    public Stat MyHealth
    {
        get { return health; }
    }

    public Stat MyArmor
    {
        get { return armor; }
    }

    public bool PickedArmor
    {
        get { return pickedArmor; }
        set { pickedArmor = value; }
    }

    public bool HasFired
    {
        get { return hasFired; }
        set { hasFired = value; }
    }
    #endregion


    private void Awake()
    {
        colorTint = GetComponent<ColorTint>();
        shieldColorTint = shield.GetComponent<ColorTint>();
        shieldTintMaterial = shield.GetComponent<SpriteRenderer>().material;
    }

    void Start()
    {
        SetBoundaries();
        health.Initialize(maxHealth, maxHealth);
        armor.Initialize(maxArmor, maxArmor);
    }

    void Update()
    {
        Move();
        Shoot();
        HandleShield();
    }

    private void SetBoundaries()
    {
        Camera gameCam = Camera.main;

        xMin = gameCam.ViewportToWorldPoint(new Vector3(0,0,0)).x + padding;
        xMax = gameCam.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        yMin = gameCam.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCam.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    private void Move()
    {
        // Desktop movement
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed;

        // Mobile movement
        var joystickDeltaX = joystick.Horizontal * Time.deltaTime * movementSpeed;
        var joystickDeltaY = joystick.Vertical * Time.deltaTime * movementSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + joystickDeltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + joystickDeltaY, yMin, yMax);

        transform.position = new Vector2(newXPos, newYPos);
    }

    private void HandleShield()
    {
        if (armor.MyCurrentValue <= 0)
        {
            shield.GetComponent<SpriteRenderer>().material = shieldDissolveMaterial;
            //Fade Out
            fade -= Time.deltaTime;

            if (fade <= 0)
            {
                shield.gameObject.SetActive(false);
                fade = 0;
            }
            shieldDissolveMaterial.SetFloat("_Fade", fade);
        }

        if (pickedArmor)
        {
            shield.GetComponent<SpriteRenderer>().material = shieldDissolveMaterial;
            //Fade In
            shield.gameObject.SetActive(true);
            fade += Time.deltaTime;
            if (fade >= 1)
            {
                fade = 1;
                pickedArmor = false;
                shield.GetComponent<SpriteRenderer>().material = shieldTintMaterial;
            }
            shieldDissolveMaterial.SetFloat("_Fade", fade);
        }    
    }

    public void Shoot()
    {
        // Desktop controls
        /* if (Input.GetButtonDown("Fire1") && !hasFired)
         {
             firingCoroutine = StartCoroutine(FireContiniously());
         }
         if (Input.GetButtonUp("Fire1"))
         {
             StopCoroutine(firingCoroutine);
             hasFired = false;
         }*/

        if (HasFired && !isFiring)
        {
            firingCoroutine = StartCoroutine(FireContiniously());
            isFiring = true;
        }
        else if (!hasFired && isFiring)
        {
            StopCoroutine(firingCoroutine);
            isFiring = false;
        }


    }

    private IEnumerator FireContiniously()
    {
        //hasFired = true;
        while (HasFired)
        {
            for (int i = 0; i < shootingExits.Length; i++)
            {
                Instantiate(laserBeam, shootingExits[i].transform.position,
                shootingExits[i].transform.rotation);
                AudioSource.PlayClipAtPoint(laserSFX, Camera.main.transform.position,laserSFXvolume);
            }

            yield return new WaitForSeconds(firePeriod);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (damageDealer != null)
        {
            ProcessHit(damageDealer);
        }
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        if (armor.MyCurrentValue <= 0)
        {
            shield.GetComponent<SpriteRenderer>().material = shieldDissolveMaterial;
            health.MyCurrentValue -= damageDealer.GetDamage();
            MyColorTint.SetTintFadeSpeed(3);
            MyColorTint.SetTintColor(new Color(0, 1, 0, 1));

        }
        else
        {
            shieldColorTint.SetTintColor(shieldTint);
            armor.MyCurrentValue -= damageDealer.GetDamage();
        }
        
        if (damageDealer.gameObject.tag == "EnemyProjectile")
        {
            damageDealer.Hit();
        }

        if (health.MyCurrentValue <= 0)
        {
           Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<LevelManager>().GameOver();
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSFXvolume);
        MyColorTint.SetTintColor(new Color(0, 1, 0, 0));
        Destroy(gameObject);

        CameraShake.MyInstance.Shake();
        GameObject explosion = Instantiate(explosionVFX, transform.position, Quaternion.identity);
        Destroy(explosion, 1.5f);
    }

}
