using UnityEngine;
using System.Collections.Generic;

public class SettlementSystem : MonoBehaviour
{
    public static SettlementSystem Instance { get; private set; }

    private Dictionary<string, string> roles = new Dictionary<string, string>();
    private List<string> createdSettlements = new List<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void CreateSettlement(string id, string displayName)
    {
        if (!createdSettlements.Contains(id)) createdSettlements.Add(id);
        Debug.Log($"SettlementSystem: created settlement {displayName} ({id})");
    }

    public void AssignRole(string npcName, string role)
    {
        if (string.IsNullOrEmpty(npcName)) return;
        roles[npcName] = role;
        Debug.Log($"SettlementSystem: Assigned role {role} to {npcName}");
    }

    public string GetRole(string npcName)
    {
        return roles.TryGetValue(npcName, out var r) ? r : null;
    }

    public Dictionary<string,string> GetAllRoles() => new Dictionary<string,string>(roles);

    public List<string> GetCreatedSettlements() => new List<string>(createdSettlements);
}
