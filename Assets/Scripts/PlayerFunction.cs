using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerFunction : MonoBehaviour
{
    [SerializeField] private AudioClip snd_death;
    [SerializeField] private AudioClip snd_hurt;
    [SerializeField] private GameObject obj_circles;
    private Animator animator;
    private SpriteRenderer spr;
    private AudioSource aud;
    private PlayerScript ps;
    private Rigidbody2D rb;
    private PlayerHP php;
    private bool inv;
    void Start()
    {
        animator = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
        aud = GetComponent<AudioSource>();
        ps = GetComponent<PlayerScript>();
        rb = GetComponent<Rigidbody2D>();
        php = GetComponent<PlayerHP>();
        inv = false;
    }

    void Update()
    {
        if (inv)
        {
            spr.color = new Color(1f, 1f, 1f, (Mathf.Sin(Time.time * 20) * 0.4f) + 0.6f);
        }
        else
        {
            spr.color = new Color(1f, 1f, 1f, 1f);
        }
    }

    public IEnumerator Damage(float dmg)
    {
        if (!inv)
        {
            if (php.HP - dmg <= 0)
            {
                Pickup pu = GetComponent<Pickup>();
                StartCoroutine(Death(pu.CP, pu.Startpos));
            }
            else
            {
                aud.PlayOneShot(snd_hurt);
                php.HP -= dmg;
                ps.CanInput = false;
                int dir = (spr.flipX ? -1 : 1);
                rb.linearVelocityX = 0.0f;
                rb.linearVelocityY = 0.0f;
                rb.AddForceX(dir * 1500.0f);
                inv = true;
                animator.SetBool("hurt", true);
                yield return new WaitForSeconds(0.5f);
                ps.CanInput = true;
                animator.SetBool("hurt", false);
                yield return new WaitForSeconds(2.0f);
                inv = false;
            }
        }
    }
    public IEnumerator Death(GameObject cp, Vector3 startpos)
    {
        php.HP = 0.0f;
        rb.simulated = false;
        ps.Camfollow = false;
        spr.enabled = false;
        AudioSource cmr_aud = ps.cmr.GetComponent<AudioSource>();
        cmr_aud.Stop();
        List<GameObject> cirs = new List<GameObject>();
        for (int i = 0; i < 8; i++)
        {
            for (int n = 0; n < 2; n++)
            {
                float angle = i * 2f * Mathf.PI / 8f;
                GameObject cir = Instantiate(obj_circles, transform.position, transform.rotation);
                Rigidbody2D cir_rb = cir.GetComponent<Rigidbody2D>();
                cir_rb.linearVelocity = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * (n + 1f) * 3f;
                cir_rb.linearVelocity.Normalize();
                cirs.Add(cir);
            }
        }
        aud.PlayOneShot(snd_death);
        if (cp != null)
        {
            transform.position = cp.transform.position;
        }
        else
        {
            transform.position = startpos;
        }
        yield return new WaitForSeconds(3.0f);
        foreach (GameObject n in cirs)
        {
            Destroy(n);
        }
        php.HP = php.MHP;
        cirs.Clear();
        cmr_aud.Play();
        spr.enabled = true;
        rb.simulated = true;
        ps.Camfollow = true;
    }
}
