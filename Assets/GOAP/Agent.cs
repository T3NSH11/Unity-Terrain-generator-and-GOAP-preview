using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [NonSerialized]public int Attackcount;
    [NonSerialized] public GameObject Player;
    [SerializeField]Planner.AllStates Goal;
    public Planner planner;
    public float Speed;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        planner = new Planner();
        planner.SetupActions();
    }

    void Update()
    {
        planner.ReachGoal(Goal, this);
    }
}
