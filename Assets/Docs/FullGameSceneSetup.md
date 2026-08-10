# FullGameScreen setup

This editor utility creates a placeholder scene "FullGameScreen" populated with the main game objects as simple primitives (cubes, spheres, cylinders) sized and positioned roughly like the anime layout. It also creates UI placeholders (HUD, Dialogue UI) and a choice button prefab.

How to use
1. In Unity Editor, open the menu: Tools → Tempest → Setup FullGameScreen Scene (placeholder).
2. The script will create a new scene, add managers that exist in your project (it will skip missing types), create the player, NPCs, settlement buildings, production buildings, market area, trees, enemy spawn points, a Canvas with HUD and Dialogue UI, and save the scene to Assets/Scenes/FullGameScreen.unity.
3. Open the scene and inspect objects. Replace primitives with real models/textures and assign proper components in the Inspector. The utility attempts to wire some references automatically:
   - DialogueManager.uiController <- DialogueUIController (if both types exist)
   - WorldTimeManager.config <- Assets/Data/World/TimeOfDayConfigSO.asset (if present)
   - WeatherSystem.availableWeathers <- WeatherSO assets from Assets/Data/World (if present)
4. If some types are missing, the script logs warnings and skips adding those components.

Notes
- All created prefabs and objects are placeholders; set real art assets and tweak Inspector fields (stations, managers, NPC dialogues) after opening the scene.
- If you want additional specific placements or prefab templates, tell me and I will extend the script to add them.
