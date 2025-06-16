using UnityEngine;
using System;
using System.Collections;

public class SpringHitMain : MonoBehaviour
{
    private GameObject body;
    private GameObject player;
    [SerializeField] private GameObject BodyPrefab;
    [SerializeField] private int hp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = Instantiate(BodyPrefab, transform.position, transform.rotation);
        SpringMain maintemp = body.GetComponent<SpringMain>();
        maintemp.Hurter = gameObject;
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D colission)
    {
        if (colission.tag == "playerProj")
        {
            AudioSource sound = GetComponent<AudioSource>();
            sound.Play();
            PlayerScript ps = player.GetComponent<PlayerScript>();
            ps.score += 5;
            hp--;
            Destroy(colission.gameObject);
            if (hp == 0)
            {
                StartCoroutine(PlayUntilDestroy(ps));
            }
        }
    }

    IEnumerator PlayUntilDestroy(PlayerScript comp)
    {
        yield return new WaitForSeconds(0.25f);
        comp.score += 10;
        Destroy(body);
        Destroy(gameObject);
    }
}
