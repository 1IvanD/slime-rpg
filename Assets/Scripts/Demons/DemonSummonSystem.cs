using UnityEngine;
using System.Collections.Generic;

public class DemonSummonSystem : MonoBehaviour
{
    private PlayerStats playerStats;
    private List<DemonLord> summonedDemons = new List<DemonLord>();
    private int maxSummonedDemons = 3;
    private float summonEnergyCost = 50f;

    private float currentSummonEnergy = 100f;
    private float maxSummonEnergy = 100f;

    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            playerStats = player.GetStats();
        }
    }

    public bool SummonDemon(DemonLord demon)
    {
        if (currentSummonEnergy < summonEnergyCost)
        {
            Debug.Log("Not enough summon energy!");
            return false;
        }

        if (summonedDemons.Count >= maxSummonedDemons)
        {
            Debug.Log("Cannot summon more demons!");
            return false;
        }

        summonedDemons.Add(demon);
        currentSummonEnergy -= summonEnergyCost;
        Debug.Log($"Summoned: {demon.name}!");
        return true;
    }

    public void DismissDemon(DemonLord demon)
    {
        if (summonedDemons.Remove(demon))
        {
            currentSummonEnergy += summonEnergyCost * 0.5f;
            Debug.Log($"Dismissed: {demon.name}");
        }
    }

    public void DismissAllDemons()
    {
        summonedDemons.Clear();
        currentSummonEnergy = maxSummonEnergy;
        Debug.Log("All demons dismissed");
    }

    public List<DemonLord> GetSummonedDemons() => summonedDemons;
    public float GetSummonEnergy() => currentSummonEnergy;
    public float GetMaxSummonEnergy() => maxSummonEnergy;

    private void Update()
    {
        // Регенерация энергии призыва
        if (currentSummonEnergy < maxSummonEnergy)
        {
            currentSummonEnergy += Time.deltaTime * 5f;
        }
    }
}
