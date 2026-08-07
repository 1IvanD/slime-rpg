# Slime RPG — quick test setup

This repository now includes editor utilities and runtime helpers to quickly create a test scene and main menu for development.

How to use:

1. Open Unity and import the project.
2. (Optional) Install TextMeshPro package if you want TMP UI elements to render correctly.
3. In the Unity Editor go to Tools → Scene Setup → Create TestScene and MainMenu. This will create two scenes in Assets/Scenes and add them to Build Settings.
4. Open Assets/Scenes/TestScene and press Play. CanvasSetup will create MainCanvas at runtime; GameManager and AudioManager are created automatically.

Files added by the assistant:
- Assets/Scripts/GameManager.cs (previous commit)
- Assets/Scripts/UI/CanvasSetup.cs (modified)
- Assets/Scripts/Player/Player.cs (new)
- Assets/Scripts/Player/PlayerStats.cs (new)
- Assets/Scripts/Audio/AudioManager.cs (new)
- Assets/Scripts/Save/SaveManager.cs (new)
- Assets/Editor/SceneSetupUtility.cs (new) — editor utility to create scenes

Notes:
- If TextMeshPro is not present in your project, replace TextMeshProUGUI references with UnityEngine.UI.Text or import TextMeshPro via Window → Package Manager.
- The created Player is a placeholder capsule with a Player component. Replace with your real player prefab when available.
