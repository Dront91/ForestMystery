using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritUp : MonoBehaviour
{
    [SerializeField] private float _multiplierY;

    private void Update()
    {
        transform.Translate(0, _multiplierY * Time.deltaTime, 0);
    }
}
