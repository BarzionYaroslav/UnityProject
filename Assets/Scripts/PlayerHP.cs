using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sprRend;
    private BoxCollider2D bc;
    private float hp;
    public float HP { get => hp; set => hp = value; }
    [SerializeField] private float mhp = 28.0f;
    public float MHP {get => mhp; set => mhp = value; }
    [SerializeField] private Image healthbar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        sprRend = GetComponent<SpriteRenderer>();
        hp = mhp;
    }

    // Update is called once per frame
    void Update()
    {
        healthbar.fillAmount = hp / mhp;
    }
}
