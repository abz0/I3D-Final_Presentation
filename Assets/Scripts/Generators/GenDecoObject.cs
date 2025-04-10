using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GenDecoObject
{
    [SerializeField] private GameObject _obj;
    [SerializeField] private int _amount;

    [Header("Capsule Cast")]
    [SerializeField] private float _radius;
    [SerializeField] private float _height;

    // Getter and Setters // // // //
    public GameObject obj
    {
        get { return _obj; }
        private set { _obj = value; }
    }

    public int amount
    {
        get { return _amount; }
        private set { _amount = value; }
    }

    public float radius
    {
        get { return _radius; }
        private set { _radius = value; }
    }

    public float height
    {
        get { return _height; }
        private set { _height = value; }
    }
}
