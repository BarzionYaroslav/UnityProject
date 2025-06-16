using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DropperMain : MonoBehaviour
{
    private BoxCollider2D bc;
    private Rigidbody2D rb;
    private AudioSource aud;
    private List<GameObject> chains = new List<GameObject>();
    private GameObject player;
    [SerializeField] GameObject chainPrefab;
    [SerializeField] GameObject dropcol;
    private GameObject chain;
    private bool drop;
    private Vector2 pos;
    private bool grounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        aud = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        drop = false;
        pos = transform.position;
        player = GameObject.FindWithTag("Player");
        chain = Instantiate(chainPrefab, pos, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        dropcol.transform.position = transform.position;
        chain.transform.position = transform.position;
        SpriteRenderer chainRend = chain.GetComponent<SpriteRenderer>();
        BoxCollider2D chainCol = chain.GetComponent<BoxCollider2D>();
        DropperCol temp = dropcol.GetComponent<DropperCol>();
        grounded = temp.grounded;

        chainRend.size = new Vector2(0.375f, (pos.y - transform.position.y));
        chainCol.size = chainRend.size;
        chainCol.offset = new Vector2(chainRend.size.x, chainRend.size.y / 2);

        if (drop)
        {
            if (!grounded)
            {
                rb.linearVelocityY = -6.0f;
            }
            else
            {
                aud.Play();
                drop = false;
            }
        }
        else
        {
            if (transform.position.y < pos.y)
            {
                rb.linearVelocityY = 2.0f;
            }
            else
            {
                rb.linearVelocityY = 0.0f;
                transform.position = pos;
                if ((Mathf.Abs(player.transform.position.x - transform.position.x) <= 2.5f) &
                Mathf.Abs(player.transform.position.y - transform.position.y) <= 16)
                {
                    drop = true;
                }
            }
        }
    }
}
