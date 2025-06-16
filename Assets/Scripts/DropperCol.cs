using UnityEngine;

public class DropperCol : MonoBehaviour
{
    private BoxCollider2D bc;
    public ContactFilter2D ContactFilter;
    private Rigidbody2D rb;
    public bool grounded;
    private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), player.gameObject.GetComponent<Collider2D>(), true);
        ContactFilter.SetNormalAngle(45.0f, 135.0f);
        ContactFilter.useNormalAngle = true;
        grounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = rb.IsTouching(ContactFilter);
    }
}
