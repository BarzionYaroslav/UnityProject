using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim=GetComponent<Animator>();
    }

    void active()
    {
        anim.SetBool("trigger", true);
    }

    void inactive()
    {
        anim.SetBool("trigger", false);
    }
}
