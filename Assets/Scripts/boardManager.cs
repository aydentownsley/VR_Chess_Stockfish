using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class boardManager : MonoBehaviour
{
    /// <summary>
    /// Managers the movement
    /// and tracking of the pieces
    /// on the board.
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        other.transform.parent = this.transform;
    }
}
