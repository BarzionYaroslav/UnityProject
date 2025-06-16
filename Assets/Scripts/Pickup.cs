using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
public class Pickup : MonoBehaviour
{
    private GameObject cp;
    public GameObject CP { get => cp; set => cp = value; }
    private Vector3 startpos;
    public Vector3 Startpos { get => startpos; set => startpos = value; }
    [SerializeField] private string nextScene;
    void Start()
    {
        startpos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D colission)
    {
        if (colission.tag == "pickup")
        {
            StartCoroutine(PlayUntilDestroy(colission.gameObject));
        }
        if ((colission.tag == "Respawn") & (colission.gameObject != cp))
        {
            if (cp != null)
            {
                cp.SendMessage("inactive");
            }
            cp = colission.gameObject;
            cp.SendMessage("active");
        }
        if (colission.tag == "Death")
        {
            PlayerFunction pf = GetComponent<PlayerFunction>();
            StartCoroutine(pf.Death(cp, startpos));
        }
        if (colission.tag == "Finish")
        {
            SceneManager.LoadScene(nextScene);
        }
    }
    private void OnTriggerStay2D(Collider2D colission)
    {
        if (colission.tag == "damage")
        {
            PlayerFunction pf = GetComponent<PlayerFunction>();
            StartCoroutine(pf.Damage(8.0f));
        }
    }
    private void OnCollisionStay2D(Collision2D colission)
    {
        if (colission.gameObject.tag == "damage")
        {
            PlayerFunction pf = GetComponent<PlayerFunction>();
            StartCoroutine(pf.Damage(8.0f));
        }
    }

    IEnumerator PlayUntilDestroy(GameObject pickup)
    {
        AudioSource sound = pickup.GetComponent<AudioSource>();
        sound.Play();
        yield return new WaitForSeconds(0.1f);
        PlayerScript ps = GetComponent<PlayerScript>();
        ps.score += 50;
        Destroy(pickup);
    }
}
