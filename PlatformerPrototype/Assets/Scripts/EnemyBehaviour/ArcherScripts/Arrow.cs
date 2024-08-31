using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Transform target;

    private Rigidbody2D rb;
    public float force;
    public float damage = 12f;

    private void OnEnable()
    {
        transform.localScale = new(-1, 1, 1);
        rb = GetComponent<Rigidbody2D>();

        if (target != null)
        {
            Vector3 direction = target.position - transform.position;

            rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

            float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.CompareTag(TagHandler.Player1) || collision.CompareTag(TagHandler.Player2))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
        }

        gameObject.SetActive(false);
    }


}
