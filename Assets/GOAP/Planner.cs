using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Planner
{
    #region Action Declarations
    ActionBase idleAction;
    ActionBase attackAction;
    ActionBase moveToPlayerAction;
    ActionBase specialAttackAction;
    #endregion
    public enum AllStates
    {
        PlayerIsAlive,
        PlayerAttacked,
        AgentCloseToPlayer,
        AttackReady,
        CanSpecial
    }

    public List<ActionBase> Actions = new List<ActionBase>();
    public List<AllStates> AquiredStates = new List<AllStates>();

    // check if action is doable when GetNextAction(goal) if not doable get prereq in result action that isnt in aprereqs then run GetNextAction(result)
    public void ReachGoal(AllStates Goal, Agent agent)
    {
        WorldStatesManager.UpdateWorldStates(agent);

        ActionBase currentAction = GetNextAction(Goal, agent);

        currentAction = GetNextAction(GetRequiredPrereqs(currentAction, agent).FirstOrDefault(), agent);

        while (!CheckIfActionDoable(currentAction))
        {
            currentAction = GetNextAction(GetRequiredPrereqs(currentAction, agent).FirstOrDefault(), agent);
        }

        if (CheckIfActionDoable(currentAction))
        {
            if (currentAction.DoAction(agent) == true)
            {
                foreach (AllStates s in currentAction.Effects)
                {
                    if (!AquiredStates.Contains(s))
                        AquiredStates.Add(s);
                }
            }
            else
            {
                foreach (AllStates s in currentAction.Effects)
                {
                    if (AquiredStates.Contains(s))
                        AquiredStates.Remove(s);
                }
            }
        }
    }

    ActionBase GetNextAction(AllStates effect, Agent agent)
    {
        List<ActionBase> PossibleActions = new List<ActionBase>();
        foreach (ActionBase a in Actions)
        {
            foreach (AllStates e in a.Effects)
            {
                if (e == effect && !a.WorldPrerequisites.Except(AquiredStates).Any())
                {
                    PossibleActions.Add(a);
                }
            }
        }

        if (PossibleActions.Count <= 0)
            return idleAction;
        else
        {
            ActionBase bestAction = null;
            foreach (ActionBase a in PossibleActions)
            {
                if (bestAction == null || a.ActionCost < bestAction.ActionCost)
                {
                    bestAction = a;
                }
            }
            return bestAction;
        }
    }

    List<AllStates> GetRequiredPrereqs(ActionBase action, Agent agent)
    {
        List<AllStates> prereqs = action.Prerequisites;

        foreach (AllStates e in action.Prerequisites)
        {
            if(AquiredStates.Contains(e))
                prereqs.Remove(e);
        }

        return prereqs;
    }

    bool CheckIfActionDoable(ActionBase action)
    {
        if (!action.Prerequisites.Except(AquiredStates).Any())
        {
            return true;
        }
        else
            return false;
    }

    public void SetupActions()
    {
        #region Action Declarations
        idleAction = new IdleAction();
        attackAction = new AttackAction();
        moveToPlayerAction = new MoveToPlayerAction();
        specialAttackAction = new SpecialAttackAction();
        #endregion

        #region Add Actions to list
        Actions.Add(idleAction);
        Actions.Add(attackAction);
        Actions.Add(moveToPlayerAction);
        Actions.Add(specialAttackAction);
        #endregion

        foreach (ActionBase a in Actions)
        {
            a.Setup();
        }
    }
}
