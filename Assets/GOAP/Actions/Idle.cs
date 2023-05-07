using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAction : ActionBase
{
    public override int ActionCost { get; set; }
    public override List<Planner.AllStates> Prerequisites { get; set; }
    public override List<Planner.AllStates> WorldPrerequisites { get; set; }
    public override List<Planner.AllStates> Effects { get; set; }

    public override void Setup()
    {
        ActionCost = 10;
        #region Add prerequisites
        Prerequisites = new List<Planner.AllStates>();
        #endregion

        #region Add effects
        Effects = new List<Planner.AllStates>();
        #endregion
    }

    public override bool DoAction(Agent agent)
    {
        Debug.Log("Idling");

        return true;
    }
}
