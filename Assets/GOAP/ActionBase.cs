using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class ActionBase
{
    public abstract int ActionCost { get; set; }
    public abstract List<Planner.AllStates> Prerequisites { get; set; }
    public abstract List<Planner.AllStates> WorldPrerequisites { get; set; }
    public abstract List<Planner.AllStates> Effects { get; set; }

    public abstract void Setup();
    public abstract bool DoAction(Agent agent);
}
