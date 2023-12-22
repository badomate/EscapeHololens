using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchReactor : MonoBehaviour
{
    Color defaultColor;

    [SerializeField]
    Color highlightColor;

    private void Start() {
        defaultColor = GetComponent<MeshRenderer>().material.color;
    }

    private void OnTriggerEnter(Collider other) {
        this.GetComponent<MeshRenderer>().material.color = highlightColor;
    }
    private void OnTriggerExit(Collider other) {
        this.GetComponent<MeshRenderer>().material.color = defaultColor;
    }
}
