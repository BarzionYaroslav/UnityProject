using UnityEngine;
using System;
using System.Collections;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sprRend;
    public GameObject cmr;
    private BoxCollider2D bc;
    private ContactFilter2D ContactFilter;
    private bool grounded => rb.IsTouching(ContactFilter);
    private float camX;
    private float camY;
    private float maxX;
    private float minX;
    private float maxY;
    private float minY;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jump = 1300.0f;
    private float jbuffer;
    private bool canShoot;
    private GameObject bul;
    private bool canInput;
    private bool camfollow;
    public int score;
    public bool Camfollow { get => camfollow; set => camfollow = value; }
    public bool CanInput { get => canInput; set => canInput = value; }
    private bool soundpause;
    [SerializeField] AudioClip snd_land;
    [SerializeField] GameObject buster;
    [SerializeField] GameObject firstScreen;
    [SerializeField] GameObject scoreboard;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        sprRend = GetComponent<SpriteRenderer>();
        cmr = GameObject.FindWithTag("MainCamera");
        ContactFilter.SetNormalAngle(45.0f, 135.0f);
        ContactFilter.useNormalAngle = true;
        jbuffer = 0.0f;
        canShoot = true;
        camfollow = true;
        BorderCalculus(firstScreen);
        canInput = true;
        soundpause = false;
    }

    private void BorderCalculus(GameObject trans)
    {
        Vector2 bordpos = trans.transform.position;
        Vector2 bordscale = trans.transform.localScale;
        Vector2 pos = transform.position;
        if ((pos.x > bordpos.x - bordscale.x / 2 & pos.x < bordpos.x + bordscale.x / 2)
        & (pos.y > bordpos.y - bordscale.y / 2 & pos.y < bordpos.y + bordscale.y / 2))
        {
            minX = bordpos.x - bordscale.x / 2;
            minY = bordpos.y - bordscale.y / 2;
            maxX = bordpos.x + bordscale.x / 2;
            maxY = bordpos.y + bordscale.y / 2;
        }
    }
    private void OnTriggerStay2D(Collider2D colission)
    {
        if (colission.tag == "screenTrans")
        {
            BorderCalculus(colission.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        score = Math.Clamp(score, 0, 99999999);
        TMP_Text TMP_scoreboard = scoreboard.GetComponent<TMP_Text>();
        string lastscore = new String('0', Math.Max(8 - score.ToString().Length, 0)) + score.ToString();
        TMP_scoreboard.text = lastscore;
        AudioSource sound = GetComponent<AudioSource>();
        PauseControl pause = cmr.GetComponent<PauseControl>();
        if (pause.ispaused & sound.isPlaying & !soundpause)
        {
            sound.Pause();
            soundpause = true;
        }
        else if (!pause.ispaused & soundpause)
        {
            sound.Play();
            soundpause = false;
        }
        if (camfollow)
            {
                Camera cam = cmr.GetComponent<Camera>();
                float cam_width = cam.orthographicSize * cam.aspect;
                float cam_height = cam.orthographicSize;
                camX = Mathf.Clamp(transform.position.x, minX + cam_width, maxX - cam_width);
                camY = Mathf.Clamp(transform.position.y + 2.0f, minY + cam_height, maxY - cam_height);
                cmr.transform.position = new Vector3(camX, camY, -10);
            }
        if (canInput & !pause.ispaused)
        {
            rb.linearVelocityX = Input.GetAxis("Horizontal") * speed;
            rb.linearVelocityY = Mathf.Clamp(rb.linearVelocityY, -20.0f, 20.0f);
            rb.linearVelocityX = Mathf.Clamp(rb.linearVelocityX, -20.0f, 20.0f);
            if (Input.GetKeyDown("z"))
            {
                jbuffer = 0.1f;
            }
            if (jbuffer > 0)
            {
                jbuffer -= Time.deltaTime;
            }
            if (grounded)
            {
                if (jbuffer > 0)
                {
                    rb.AddForceY(jump * 3.0f);
                    jbuffer = 0;
                }
            }
            else if (!Input.GetKey("z") & rb.linearVelocityY > 0)
            {
                rb.linearVelocityY = 0;
            }
            if (Input.GetAxis("Horizontal") < 0)
            {
                sprRend.flipX = false;
                animator.SetBool("walk", true);
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                sprRend.flipX = true;
                animator.SetBool("walk", true);
            }
            else
            {
                animator.SetBool("walk", false);
            }
            if (!grounded)
            {
                animator.SetBool("fall", true);
            }
            else
            {
                animator.SetBool("fall", false);
            }
            if (Input.GetKey("x") && canShoot && sprRend.enabled)
            {
                StartCoroutine(ShotBuster());
            }
        }
    }

    private void OnCollisionStay2D(Collision2D colission)
    {
        if (colission.collider.tag == "mover" & rb.IsTouching(colission.collider, ContactFilter))
        {
            GameObject conveyor = colission.gameObject;
            conveyorVar convar = conveyor.GetComponent<conveyorVar>();
            if (canInput)
                rb.AddForceX(convar.force * Time.deltaTime * 1000);
            else
                rb.linearVelocityX = convar.force/10;
        }
    }
    private void OnCollisionEnter2D(Collision2D colission)
    {
        if (rb.IsTouching(colission.collider, ContactFilter))
        {
            AudioSource sound = GetComponent<AudioSource>();
            sound.PlayOneShot(snd_land);
        }
    }

    IEnumerator ShotBuster()
    {
        canShoot=false;
        AudioSource sound = GetComponent<AudioSource>();
        int dir = (sprRend.flipX ? -1 : 1);
        Vector2 shootPos = new Vector2(transform.position.x - ((85.0f/80) * dir), transform.position.y + 65.0f/80);
        bul = Instantiate(buster, shootPos, transform.rotation);
        SpriteRenderer bulspr = bul.GetComponent<SpriteRenderer>();
        bulspr.flipX = sprRend.flipX;
        Rigidbody2D bulrd = bul.GetComponent<Rigidbody2D>();
        bulrd.linearVelocityX = -dir * 15.0f;
        sound.Play();
        animator.SetBool("shoot", true);
        yield return new WaitForSeconds(0.4f);
        animator.SetBool("shoot", false);
        canShoot = true;
    }
}
