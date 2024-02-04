using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Gun_shoot : MonoBehaviour
{

    [SerializeField]
    private bool aiGun = false;
    [SerializeField]
    int fireRate = 2;//shots per second;
    [SerializeField]
    public float bulletSpread = 0.2f;
    [SerializeField]
    float gunDamage = 25f;
    [SerializeField]
    GameObject fireParticles;
    [SerializeField]
    GameObject hitParticles;
    [SerializeField]
    public LayerMask obstructionMask;

    [SerializeField]
    private int magCap = 7;
    [SerializeField]
    private int magBullets = 7;
    [SerializeField]
    private float reloadTime = 2;

    [SerializeField]
    private AudioClip gunShotSound;
    [SerializeField]
    private AudioClip emptyGunSound;
    [SerializeField]
    private AudioClip reloadStartSound;
    [SerializeField]
    private AudioClip reloadEndSound;
    [SerializeField]
    private GameObject magCapGO;

    [SerializeField]
    private GameObject laserPointer;

    private TextMeshProUGUI magCapText;

    private PlayerControls playerControls;

    private float timeBetween;

    private bool reloading = false;

    private bool firstShot = true;

    Vector2 playerRotation;


    AudioSource audioData;

    private void Awake()
    {
        if (!aiGun) playerControls = new PlayerControls();
    }
    private void OnEnable()
    {
        if(!aiGun) playerControls.Enable();
    }

    private void OnDisable()
    {
        if (!aiGun) playerControls.Disable();
    }
    void Start()
    {
        if (!aiGun) { 
            magCapText = magCapGO.GetComponent<TextMeshProUGUI>();
            //laserPointer = Instantiate(hitParticles, transform.position, transform.rotation);
        }
        audioData = GetComponent<AudioSource>();
    }

    public void reload()
    {
        if (!reloading)
        {
            reloading = true;
            Invoke("doReload", reloadTime);
            audioData.clip = reloadStartSound;
            audioData.Play();
            firstShot = true;
        }
    }

    private void doReload()
    {
        reloading = false;
        magBullets = magCap;
        audioData.clip = reloadEndSound;
        audioData.Play();
    }

    public void AIShoot() {
        timeBetween += Time.deltaTime;
        float timeBetweenShots = 1f / fireRate;



        if ((timeBetween >= timeBetweenShots || firstShot == true) && !reloading)
        {
            //GameObject bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0, 0, 0.1f), transform.rotation);
            //bullet.GetComponent<Rigidbody2D>().velocity = playerRotation * bulletSpeed + new Vector2(Random.Range(-bulletSpread, bulletSpread), Random.Range(-bulletSpread, bulletSpread));
            //bullet.GetComponent<BulletScript>().bulletDamage = gunDamage;
            if (magBullets > 0)
            {
                audioData.clip = gunShotSound;
                audioData.Play();

                magBullets--;
                var hitPoint = GetComponentInParent<Transform>().up.normalized;

                hitPoint.x += Random.Range(-bulletSpread, bulletSpread);
                hitPoint.y += Random.Range(-bulletSpread, bulletSpread);

                RaycastHit2D hit = Physics2D.Raycast(transform.position, hitPoint, 20f, obstructionMask);

                //Debug.Log((playerRotation * Random.Range(-bulletSpread, bulletSpread)).normalized);

                if (hit)
                {
                    GameObject hitPart = Instantiate(hitParticles, hit.point, hit.transform.rotation);
                    //Debug.Log(hit.transform.name);
                    try
                    {
                        hit.transform.GetComponent<HealthScript>().doDamage(gunDamage);
                        var color = hitPart.GetComponent<ParticleSystem>().main;
                        color.startColor = new ParticleSystem.MinMaxGradient(Color.red, Color.red);
                        hitPart.GetComponent<ParticleSystem>().Play();
                    } //try catch dla przypadków nie maj¹cych komponentu healthScript
                    catch
                    {
                        hitPart.GetComponent<ParticleSystem>().Play();
                    }
                    fireParticles.GetComponent<ParticleSystem>().Stop();
                    fireParticles.GetComponent<ParticleSystem>().Play();
                    //LineRenderer
                }
                timeBetween = 0f;
                firstShot = false;
            }
            else
            {
                audioData.clip = emptyGunSound;
                audioData.Play();
                timeBetween = 0f;
                firstShot = false;
                reload();
            }

        }
    }
    [SerializeField]
    public float shotAlertRadius = 10;
    [SerializeField]
    public LayerMask targetMaskAlert;

    private void PlayerShoot() {
        playerRotation = playerControls.Player.LookShoot.ReadValue<Vector2>();
        float distance = Vector2.Distance(new Vector2(0, 0), playerRotation);

        if (distance < 0.9)
        {
            firstShot = true;
            magCapText.text = magBullets + "/" + magCap;
            return;
        }


        timeBetween += Time.deltaTime;
        float timeBetweenShots = 1f / fireRate;


        if ((timeBetween >= timeBetweenShots || firstShot == true) && !reloading)
        {
            //GameObject bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0, 0, 0.1f), transform.rotation);
            //bullet.GetComponent<Rigidbody2D>().velocity = playerRotation * bulletSpeed + new Vector2(Random.Range(-bulletSpread, bulletSpread), Random.Range(-bulletSpread, bulletSpread));
            //bullet.GetComponent<BulletScript>().bulletDamage = gunDamage;
            Collider2D[] rangeChecks = Physics2D.OverlapCircleAll(transform.position, shotAlertRadius, targetMaskAlert);
            foreach (Collider2D c in rangeChecks)
            {
                RaycastHit2D hitAlert = Physics2D.Raycast(transform.position, c.transform.position - transform.position, shotAlertRadius, obstructionMask);
                if (hitAlert && hitAlert.transform.tag == "Enemy")
                {
                    c.GetComponent<AIScript>().sawPlayer = true;
                }
            }

            if (magBullets > 0)
            {
                audioData.clip = gunShotSound;
                audioData.Play();

                magBullets--;
                var hitPoint = playerRotation;

                hitPoint.x += Random.Range(-bulletSpread, bulletSpread);
                hitPoint.y += Random.Range(-bulletSpread, bulletSpread);

                RaycastHit2D hit = Physics2D.Raycast(transform.position, hitPoint, 20f, obstructionMask);

                //Debug.Log((playerRotation * Random.Range(-bulletSpread, bulletSpread)).normalized);

                if (hit)
                {
                    GameObject hitPart = Instantiate(hitParticles, hit.point, hit.transform.rotation);
                    //Debug.Log(hit.transform.name);
                    try { 
                        hit.transform.GetComponent<HealthScript>().doDamage(gunDamage);
                        var color = hitPart.GetComponent<ParticleSystem>().main;
                        color.startColor = new ParticleSystem.MinMaxGradient(Color.red, Color.red);
                        hitPart.GetComponent<ParticleSystem>().Play();
                    } //try catch dla przypadków nie maj¹cych komponentu healthScript
                    catch {
                        hitPart.GetComponent<ParticleSystem>().Play();
                    }
                    fireParticles.GetComponent<ParticleSystem>().Stop();
                    fireParticles.GetComponent<ParticleSystem>().Play();
                    //LineRenderer
                }
                timeBetween = 0f;
                firstShot = false;
            }
            else
            {
                audioData.clip = emptyGunSound;
                audioData.Play();
                timeBetween = 0f;
                firstShot = false;
            }

        }

        magCapText.text = magBullets + "/" + magCap;

    }

    // Update is called once per frame
    void Update()
    {
        if (!aiGun) { 
            PlayerShoot();
            if (laserPointer != null) {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 100f, obstructionMask);
                laserPointer.transform.position = hit.point;
                laserPointer.transform.rotation = transform.rotation;
            }
        }

    }
}
