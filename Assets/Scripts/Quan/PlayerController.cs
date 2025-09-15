using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public Animator anim;

    [Header("Movement")]
    public float speed = 6f;

    [Header("Grounding")]
    public float groundDist = 0.1f;
    public LayerMask terrainLayer;

    [Header("Refs")]
    public Rigidbody rb;
    public SpriteRenderer sr;

    Vector3 input;       
    float groundY;      

    void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        bool isMoving = (x != 0f || z != 0f);
        anim.SetBool("isMoving", isMoving);

        input = new Vector3(x, 0f, z).normalized;

        if (sr)
        {
            if (x < 0) sr.flipX = true;
            else if (x > 0) sr.flipX = false;
        }

        var castPos = transform.position + Vector3.up;
        if (Physics.Raycast(castPos, Vector3.down, out var hit, Mathf.Infinity, terrainLayer))
        {
            groundY = hit.point.y + groundDist;
        }
    }

    void FixedUpdate()
    {
        Vector3 move = input * speed;
        Vector3 target = rb.position + new Vector3(move.x, 0f, move.z) * Time.fixedDeltaTime;
        target.y = groundY;
        rb.MovePosition(target);
    }
}
