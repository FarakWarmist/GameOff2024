using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SqueakyPlace : MonoBehaviour
{
    [SerializeField] private float _noiseToMake = 20f;
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            NoiseManager.Instance.MakeNoise(_noiseToMake, transform.position);
    }
}
