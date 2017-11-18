using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Destructible2D;
public class PlateformController : MonoBehaviour
{
    [Tooltip("The amount of times you want the clicked object to fracture")]
    public int FractureCount = 5;

    [Tooltip("The amount of outward force added to each fractured part")]
    public float Force;

    D2dDestructible destructible;
    Rigidbody2D rigidbody;
    Vector2 explosionPosition;

    private void Awake()
    {
        destructible = GetComponent<D2dDestructible>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        explosionPosition = gameObject.transform.position;
    }

    public void Break()
    {
        rigidbody.bodyType = RigidbodyType2D.Dynamic;

        // Register split event
        destructible.OnEndSplit.AddListener(OnEndSplit);

        // Split via fracture
        D2dQuadFracturer.Fracture(destructible, FractureCount, 0.5f);

        // Unregister split event
        destructible.OnEndSplit.RemoveListener(OnEndSplit);
    }

    private void OnEndSplit(List<D2dDestructible> clones)
    {
        // Go through all clones in the clones list
        for (var i = clones.Count - 1; i >= 0; i--)
        {
            var clone = clones[i];
            var rigidbody = clone.GetComponent<Rigidbody2D>();

            // Does this clone have a Rigidbody2D?
            if (rigidbody != null)
            {
                // Get the local point of the explosion that called this split event
                var localPoint = (Vector2)clone.transform.InverseTransformPoint(explosionPosition);

                // Get the vector between this point and the center of the destructible's current rect
                var vector = clone.AlphaRect.center - localPoint;

                // Apply relative force
                rigidbody.AddRelativeForce(vector * Force, ForceMode2D.Impulse);
            }
        }
    }
}
