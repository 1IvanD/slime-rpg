using UnityEngine;

// Ogre implementation — a named magical beast
public class Ogre : MagicalBeast
{
    public override void Initialize(string type, int lvl)
    {
        base.Initialize(type, lvl);
        // Enhance base stats for ogre
        health = Mathf.Max(health, 100f + lvl * 20f);
        displayName = type;
        // add simple visual tweaks if needed
        var r = GetComponent<Renderer>();
        if (r != null) r.material.color = new Color(0.5f, 0.35f, 0.2f);
    }
}
