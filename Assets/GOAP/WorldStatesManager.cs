using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldStatesManager
{
    public static List<Planner.AllStates> worldStates = new List<Planner.AllStates>
        {
            Planner.AllStates.PlayerIsAlive,
            Planner.AllStates.CanSpecial
        };

    public static void UpdateWorldStates(Agent agent)
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            if (!agent.planner.AquiredStates.Contains(Planner.AllStates.PlayerIsAlive))
                agent.planner.AquiredStates.Add(Planner.AllStates.PlayerIsAlive);
        }
        else if (agent.planner.AquiredStates.Contains(Planner.AllStates.PlayerIsAlive))
            agent.planner.AquiredStates.Remove(Planner.AllStates.PlayerIsAlive);

        if(agent.Attackcount >= 3)
        {
            if (!agent.planner.AquiredStates.Contains(Planner.AllStates.CanSpecial))
                agent.planner.AquiredStates.Add(Planner.AllStates.CanSpecial);
        }
        else if (agent.planner.AquiredStates.Contains(Planner.AllStates.CanSpecial))
            agent.planner.AquiredStates.Remove(Planner.AllStates.CanSpecial);
    }
}
