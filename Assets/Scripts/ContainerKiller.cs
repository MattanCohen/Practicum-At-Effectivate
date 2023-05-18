using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerKiller : MonoBehaviour
{
    void KillContainer()
    {
        Destroy(transform.parent.gameObject);
    }
}
