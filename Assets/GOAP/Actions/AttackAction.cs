using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : ActionBase
{
    public override int ActionCost { get; set; }
    public override List<Planner.AllStates> Prerequisites { get; set; }
    public override List<Planner.AllStates> WorldPrerequisites { get; set ; }
    public override List<Planner.AllStates> Effects { get; set; }

    public override void Setup()
    {
        ActionCost = 10;
        #region Add prerequisites
        Prerequisites = new List<Planner.AllStates>();
        WorldPrerequisites = new List<Planner.AllStates>();

        Prerequisites.Add(Planner.AllStates.AgentCloseToPlayer);
        WorldPrerequisites.Add(Planner.AllStates.PlayerIsAlive);
        #endregion

        #region Add effects
        Effects = new List<Planner.AllStates>();

        Effects.Add(Planner.AllStates.PlayerAttacked);
        #endregion
    }

    public override bool DoAction(Agent agent)
    {
        Debug.Log("Attack");
        agent.Attackcount++;

        return true;
    }
}
