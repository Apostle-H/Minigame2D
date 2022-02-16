using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEntity : MonoBehaviour
{
    public bool facingRight = true;

    public virtual void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
}
