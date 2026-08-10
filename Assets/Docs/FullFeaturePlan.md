# Tempest Full Feature - initial plan and files added

This commit adds starter systems and scaffolding for the full-feature work:

- ItemSO & ItemsDatabase (Resources/Items)
- Inventory UI controller (placeholder)
- Extended PlayerStats (attributes + leveling behavior)
- WorldMapManager for locations and scene travel
- EnemyBehaviour (basic AI/patrol/aggro) and LootSpawner
- PredatorForm (basic absorb/effects)
- SettlementManager + BuildingSO + WorkerRole
- Editor utility to generate starter ItemSO assets under Assets/Resources/Items

How to use
1. In Unity, run Tools → Tempest → Generate Starter Items to create basic Item assets.
2. Place an ItemsDatabase in the initial scene (GameObject Managers) so it loads items at Awake.
3. Open FullGameScreen scene (if generated earlier) and test: inventory UI, enemies, predator absorb and settlement add.

Notes
- All visuals are placeholders (primitives). Replace models & sprites and assign in Inspector.
- This is scaffolding for the full feature set; more polish and integrations will follow per your direction.
