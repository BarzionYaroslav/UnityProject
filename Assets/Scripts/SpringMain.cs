using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class SpringMain : MonoBehaviour
{
    private BoxCollider2D bc;
    private ContactFilter2D ContactFilter;
    private Rigidbody2D rb;
    private bool walled;
    private GameObject player;
    private RaycastHit2D touching;
    private bool dir;
    [SerializeField] private float speed;
    public LayerMask ignore;
    private GameObject hurter;
    public GameObject Hurter { get => hurter; set => hurter = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        ContactFilter.SetNormalAngle(45.0f, -45.0f);
        ContactFilter.useNormalAngle = true;
        dir = false;
        player = GameObject.FindWithTag("Player");
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), player.gameObject.GetComponent<Collider2D>(), true);
        Physics2D.queriesStartInColliders = false;
    }

    // Update is called once per frame
    void Update()
    {
        hurter.transform.position = transform.position;
        float facing = (dir ? 1 : -1);
        rb.linearVelocityX = speed * facing;
        walled = rb.IsTouching(ContactFilter);
        Vector2 checkpos = new Vector2(transform.position.x+1.2f*facing,transform.position.y-1.0f);
        Vector2 mainpos = new Vector2(transform.position.x+1.2f*facing,transform.position.y);
        touching = Physics2D.Linecast(mainpos, checkpos, ~ignore);
        if ((walled) || (!touching))
            {
                if (!dir)
                {
                    dir = true;
                    ContactFilter.SetNormalAngle(135.0f, 225.0f);
                }
                else
                {
                    dir = false;
                    ContactFilter.SetNormalAngle(-45.0f, 45.0f);
                }
            }
    }
}
