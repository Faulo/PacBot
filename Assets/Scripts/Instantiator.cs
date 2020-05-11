﻿using UnityEngine;

public class Instantiator : MonoBehaviour {
    public void InstantiateAt(Vector3 position) {
        Instantiate(gameObject, position, Quaternion.identity);
    }
    public void InstantiateAt(Component component) => InstantiateAt(component.transform.position);
}
