using UnityEngine;
using System.Collections;

public class BusterShot : MonoBehaviour
{
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
