﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {
    [SerializeField] Vector3 movementvector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;

    float movementfactor;
    Vector3 startingpos;

	// Use this for initialization
	void Start () { // move -10
        startingpos = transform.position; 
	}
	
	// Update is called once per frame
	void Update () {
        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2f;
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementfactor = rawSinWave / 2f + 0.5f;
        Vector3 offset = movementfactor * movementvector;
        transform.position = startingpos + offset;
	}
}
