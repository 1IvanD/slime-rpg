using UnityEngine;
using System.Collections.Generic;

public class Tensei_SlimeWorldMap : MonoBehaviour
{
    [SerializeField] private float mapScaleX = 100f;
    [SerializeField] private float mapScaleZ = 100f;

    private Dictionary<string, Vector3> worldLocations = new Dictionary<string, Vector3>();

    private void Awake()
    {
        InitializeWorldLocations();
    }

    private void InitializeWorldLocations()
    {
        // Основные локации из аниме "О моем перерождении в слизь"
        
        // Лес Чертовых (Forest of Jura)
        worldLocations["ForestOfJura"] = new Vector3(10, 0, 10);
        
        // Деревня Риммуру (Rimuru's Village)
        worldLocations["RimmuruVillage"] = new Vector3(5, 0, 5);
        
        // Город Ингрессия (Ingrassia)
        worldLocations["IngrassiaCity"] = new Vector3(30, 0, 0);
        
        // Вулканическая Пещера (Volcanic Cavern)
        worldLocations["VolcanicCavern"] = new Vector3(-15, 0, 20);
        
        // Поселение Вельдоры (Veldora's Domain)
        worldLocations["VeldoraDomain"] = new Vector3(-30, 0, 20);
        
        // Затопленный Храм (Submerged Temple)
        worldLocations["SubmergedTemple"] = new Vector3(25, 0, -10);
        
        // Королевство Гоблинов (Goblin Kingdom)
        worldLocations["GoblinKingdom"] = new Vector3(-20, 0, -15);
        
        // Башня Раймондса (Ramiris's Tower)
        worldLocations["RamirisTower"] = new Vector3(0, 0, -30);
        
        // Лабиринт (Labyrinth)
        worldLocations["Labyrinth"] = new Vector3(-40, 0, 0);
        
        // Демоническое царство (Demon Kingdom)
        worldLocations["DemonKingdom"] = new Vector3(50, 0, -40);
        
        // Торговый пост (Trade Post)
        worldLocations["TradePost"] = new Vector3(0, 0, 25);
    }

    public Vector3 GetLocationPosition(string locationName)
    {
        return worldLocations.TryGetValue(locationName, out var pos) ? pos : Vector3.zero;
    }

    public Dictionary<string, Vector3> GetAllLocations() => worldLocations;

    public void TeleportToLocation(string locationName)
    {
        if (worldLocations.TryGetValue(locationName, out var position))
        {
            GameObject player = FindObjectOfType<Player>()?.gameObject;
            if (player != null)
            {
                player.transform.position = position + Vector3.up;
                Debug.Log($"Teleported to {locationName}: {position}");
            }
        }
    }
}
