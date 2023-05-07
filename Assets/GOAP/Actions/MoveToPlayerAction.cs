using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayerAction : ActionBase
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
        WorldPrerequisites = new List<Planner.AllStates>();

        WorldPrerequisites.Add(Planner.AllStates.PlayerIsAlive);
        #endregion

        #region Add effects
        Effects = new List<Planner.AllStates>();
        Effects.Add(Planner.AllStates.AgentCloseToPlayer);
        #endregion
    }

    public override bool DoAction(Agent agent)
    {
        agent.gameObject.transform.position = Vector3.MoveTowards(agent.gameObject.transform.position,
            agent.Player.transform.position,
            agent.Speed * Time.deltaTime);

        if (Vector3.Distance(agent.gameObject.transform.position, agent.Player.transform.position) < 2)
        {
            return true;
        }
        else
            return false;
    }
}
