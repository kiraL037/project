using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Vector3 direction;
    private RaycastHit2D hit; 

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        direction = new Vector3(x, y, 0);

        if (direction.x < 0)
            transform.localScale = Vector3.one;
        else if (direction.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0,
            new Vector2(0, direction.y), Mathf.Abs(direction.y * Time.deltaTime),
            LayerMask.GetMask("Seungmin", "Blocking"));

        if (hit.collider == null)
        {
            transform.Translate(0, direction.y * Time.deltaTime, 0);
        }

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0,
           new Vector2(direction.x, 0), Mathf.Abs(direction.x * Time.deltaTime),
           LayerMask.GetMask("Seungmin", "Blocking"));

        if (hit.collider == null)
        {
            transform.Translate(direction.x * Time.deltaTime, 0, 0);
        }
    }
}
