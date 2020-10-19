using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneController : MonoBehaviour
{

    [SerializeField]
    private float moveSmoothness; // How fast it tries to follow Iris' drone anchor.

    GameObject anchorPoint; // Anchor that stays behind Iris.
    GameObject spriteObject;
    Animator animator;
    public GameObject bulletPrefab;
    public GameObject laserPrefab;
    public LineRenderer laserPreview;
    private Text specialBullets;

    //following variables keep track of whether the player is going to shoot a bullet or a laser, and whether a preview would appear
    private float chargeTime;
    private float floatLoopTime;
    private Vector3 recoilOffset;
    private float previewCounter;
    public float longPress = 0.1f;
    private bool canShoot;
    private bool canPreview;

    //keep track of the time between two consecutive bullets
    private float previousBullet;
    private float thisBullet;
    public float timeAsConsecutive = 0.2f;
    public int maxConsecutiveBulletsNum=3;
    private int bulletCounter=1;
    private bool lockBullet;
    private float timeEnable = 0.8f; //how long it takes to shoot bullets again

    //bonus ablitilies variables
    public bool speedUp;
    public bool knockBack;
    public bool pierce;
    public bool powerUp;
    public int specialBulletsNum=0;
    private int laserConsumeNum = 5;
    public static DroneController D;

    static public Vector3 dronePosition;


    public AudioSource Laser1;
    public AudioSource shoot;

    // Start is called before the first frame update
    void Start()
    {
        anchorPoint = transform.parent.Find("Iris").Find("DroneAnchor").gameObject;
        spriteObject = transform.Find("DroneSprite").gameObject;
        animator = transform.Find("DroneSprite").GetComponent<Animator>();

        chargeTime = 0.0f;
        floatLoopTime = 0.0f;
        recoilOffset = Vector3.zero;
        previewCounter = 0;
        canShoot = false;
        canPreview = false;
        laserPreview.SetWidth(.3f, .3f);
        laserPreview.enabled = false;

        previousBullet = Time.time;
        lockBullet = false;

        D = this;
        DroneController.D.speedUp = false;
        DroneController.D.knockBack = false;
        DroneController.D.pierce = false;
        DroneController.D.powerUp = false;

        laserPreview = GameObject.Find("laserPreview").GetComponent<LineRenderer>();
        specialBullets = GameObject.Find("UI/SpecialBullets/RemainingBullets").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();

        specialBullets.text = specialBulletsNum.ToString();

        dronePosition = transform.GetChild(0).position;
        if (Input.GetMouseButton(0)) {
            chargeTime += Time.deltaTime;
        }
        if (Input.GetMouseButton(0))
        {
            previewCounter += Time.deltaTime;
            if (previewCounter >= longPress)
                canPreview = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            
            canShoot = true;
            previewCounter = 0;
            canPreview = false;
        }
        if (canPreview)
        {
            laserPreview.enabled = true;
            Vector3 mouse = Input.mousePosition;
            mouse = Camera.main.ScreenToWorldPoint(mouse);
            Vector3 previewDirection = mouse - dronePosition;
            Vector3 lastPoint = dronePosition;
            lastPoint.z = 0;
            int vertexCounter = 1;
            laserPreview.SetVertexCount(1);
            laserPreview.SetPosition(0, lastPoint);
            bool loop = true;
            

            while (loop)
            {
                RaycastHit2D hit = Physics2D.Raycast(lastPoint, previewDirection, 30f, 1 << LayerMask.NameToLayer("IceWall"));
                

                if (hit.collider != null)
                {
                    vertexCounter += 1;
                    laserPreview.SetVertexCount(vertexCounter);


                    laserPreview.SetPosition(vertexCounter - 1, Vector3.MoveTowards(hit.point, lastPoint, 0.01f));
                    //laserPreview.SetPosition(vertexCounter - 1, hit.point);
                    lastPoint = hit.point;
                    lastPoint.z = 0;
                    previewDirection = Vector3.Reflect(previewDirection, hit.normal);
                }
                else
                {
                    vertexCounter++;
                    laserPreview.SetVertexCount(vertexCounter);
                    laserPreview.SetPosition(vertexCounter-1, lastPoint+(previewDirection.normalized*100));
                    loop = false;
                }
            }

        }

        if(canShoot)
        {
            
            canShoot = false;
            Vector3 mouse = Input.mousePosition;
            mouse = Camera.main.ScreenToWorldPoint(mouse);
            mouse.z = 0;
            if(chargeTime < longPress&&!lockBullet)
            {
                thisBullet = Time.time;
                if (thisBullet - previousBullet < timeAsConsecutive)
                {
                    bulletCounter++;
                    if (bulletCounter == maxConsecutiveBulletsNum)
                        stopConsecutiveBullets();
                }
                else
                    bulletCounter = 1;

                previousBullet = thisBullet;
                shoot.Play();
                GameObject bulletObject = Instantiate(bulletPrefab, dronePosition, Quaternion.identity) as GameObject;
                Bullets bullet = bulletObject.GetComponent<Bullets>();
                bullet.tag = "Bullet-Ally";
                bullet.setDirection(mouse - dronePosition);
                
                if (speedUp || knockBack || pierce || powerUp)
                {
                    specialBulletsNum--;
                    //specialBullets.text = specialBulletsNum.ToString();
                    if (speedUp)
                    {
                        bullet.speed *= 2;
                    }
                    else if (knockBack)
                    {
                        bullet.knockBack = true;
                    }
                    else if (pierce)
                    {
                        bullet.pierce = true;
                    }
                    else
                    {
                        bullet.damage *= 2;
                    }

                    if (specialBulletsNum == 0)
                    {
                        DroneController.D.speedUp = false;
                        DroneController.D.knockBack = false;
                        DroneController.D.pierce = false;
                        DroneController.D.powerUp = false;
                    }
                }

                
                float bulletAngle = Vector2.Angle(mouse - dronePosition, Vector2.right);

                if (mouse.y < dronePosition.y)
                {
                    bulletAngle = -bulletAngle;
                }

                bullet.transform.eulerAngles = new Vector3(0, 0, bulletAngle);
            }
            else if (chargeTime >= longPress)
            {
                laserPreview.enabled = false;
                Laser1.Play();
                GameObject laserObject = Instantiate(laserPrefab, dronePosition, Quaternion.identity) as GameObject;
                Laser laser = laserObject.GetComponent<Laser>();
                laser.tag = "Laser-Ally";
                laser.setDirection(mouse - dronePosition);
                recoilOffset = (dronePosition - mouse).normalized;

                if (specialBulletsNum>4&&(speedUp || knockBack || pierce || powerUp))
                {
                    specialBulletsNum-=5;
                    //specialBullets.text = specialBulletsNum.ToString();
                    if (speedUp)
                    {
                        laser.speed *= 2;
                    }
                    else if (knockBack)
                    {
                        laser.knockBack = true;
                    }
                    else if (pierce)
                    {
                        laser.pierce = true;
                    }
                    else
                    {
                        laser.damage *= 2;
                    }

                    if (specialBulletsNum == 0)
                    {
                        DroneController.D.speedUp = false;
                        DroneController.D.knockBack = false;
                        DroneController.D.pierce = false;
                        DroneController.D.powerUp = false;
                    }
                }

            }

            chargeTime = 0.0f;
        }

        UpdateAnimator();
    }

    private void UpdatePosition()
    {
        transform.position = Vector2.Lerp(transform.position, anchorPoint.transform.position, moveSmoothness * Time.deltaTime);

        Vector3 lookAt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Transform droneSprite = transform.GetChild(0);
        lookAt.z = droneSprite.position.z;
        Vector3 dir = lookAt - droneSprite.position;
        droneSprite.GetComponent<SpriteRenderer>().flipX = dir.x < 0.0f;
        if (dir.x < 0.0f)
        {
            dir.x *= -1.0f;
            dir.y *= -1.0f;
        }
        droneSprite.right = dir;

    }

    private void UpdateAnimator()
    {
        float chargeProgress = chargeTime / longPress;
        animator.SetFloat("chargeProgress", chargeProgress);

        floatLoopTime += Time.deltaTime;
        float distance = 0.03f + 0.35f * (1.0f - Mathf.Min(chargeProgress, 1.0f));
        Vector3 newPos = new Vector3(0.0f, Mathf.Sin(floatLoopTime * 0.8f * Mathf.PI) * distance, 0.0f) + recoilOffset * 2.5f;
        spriteObject.transform.localPosition = Vector3.Lerp(spriteObject.transform.localPosition, newPos, 2.0f * Time.deltaTime);
        recoilOffset = Vector3.Lerp(recoilOffset, Vector3.zero, 5.0f * Time.deltaTime);

    }

    private void stopConsecutiveBullets()
    {
        bulletCounter = 1;
        lockBullet = true;
        Invoke("enableBullets", timeEnable);
    }

    private void enableBullets()
    {
        lockBullet = false;
    }

}
