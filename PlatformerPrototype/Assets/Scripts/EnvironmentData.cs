using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentData : MonoBehaviour
{
    public ContactFilter2D contactFilter;
    // Start is called before the first frame update
    public float groundDst = 0.08f;
    public float wallDst = 0.2f;
    public float ceilingDst = 0.08f;

    //Entity object
    private CapsuleCollider2D entityCollider;
    private Animator entityAnim;

    private readonly RaycastHit2D[] groundRaycastHit = new RaycastHit2D[4];
    private readonly RaycastHit2D[] wallRaycastHit = new RaycastHit2D[4];
    private readonly RaycastHit2D[] ceilingRaycastHit = new RaycastHit2D[4];

    private Vector2 WallDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    //flags
    private bool _hitWall;
    private bool _touchGround;
    private bool _touchCeiling;

    public bool HitWall
    {
        get { return _hitWall; }
        set
        {
            _hitWall = value;
        }

    }
    public bool TouchGround
    {
        get { return _touchGround; }
        set
        {
            _touchGround = value;
            entityAnim.SetBool("isGrounded", value);
        }
    }

    public bool TouchCeiling
    {
        get { return _touchCeiling; }
        set
        {
            _touchCeiling = value;
        }
    }

    private void Awake()
    {
        //Retrieve components

        entityCollider = GetComponent<CapsuleCollider2D>();
        entityAnim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    { /*
        if (gameObject.transform.localScale.x > 0)
        {
            wallDirection = Vector2.right;
        }
        else if (gameObject.transform.localScale.x < 0) 
        {
            wallDirection=Vector2.left;
        }
       */
        TouchGround = entityCollider.Cast(Vector2.down, contactFilter, groundRaycastHit, groundDst) > 0;
        HitWall = entityCollider.Cast(WallDirection, contactFilter, wallRaycastHit, wallDst) > 0;
        TouchCeiling = entityCollider.Cast(Vector2.up, contactFilter, ceilingRaycastHit, ceilingDst) > 0;
    }

}
