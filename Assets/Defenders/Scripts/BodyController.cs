using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{

    public List<Rigidbody2D> bodyParts;

    public void ActiveRigidBodys()
    {
        for (int i = 0; i < bodyParts.Count; i++)
        {
            bodyParts[i].bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
