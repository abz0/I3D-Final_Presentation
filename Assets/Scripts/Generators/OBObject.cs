using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class OGObject
{
    [SerializeField] private GameObject _obj;
    [SerializeField] private int _amount = 1;
    [SerializeField] [Range(0f, 1f)] private float _randomChance = 1; // used for the random generation

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

    public float randomChance
    {
        get { return _randomChance; }
        private set { _randomChance = value; }
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

    public void SetRadius(float radius) { this.radius = radius; }

    public void SetHeight(float height) { this.height = height; }
}
