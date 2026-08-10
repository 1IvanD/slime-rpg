using UnityEngine;

public enum WorkerRoleType { Gatherer, Alchemist, Blacksmith, Builder, Trader }

[System.Serializable]
public class WorkerRole
{
    public WorkerRoleType role;
    public int level = 1;
}
