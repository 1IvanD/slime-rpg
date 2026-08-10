# Tempest - Crafting setup

Этот документ описывает, как сгенерировать ключевые рецепты 1 сезона (алхимия и кузнечное дело) и как протестировать станции.

Шаги:
1) В Unity Editor откройте меню Tools → Tempest → Generate Key Crafting Items & Recipes (Season1). Это создаст Assets/Data/Crafting/Items и Recipes с базовыми .asset файлами:
   - Items: Item_Hipokute, Item_Hipokute_Extract, Item_Potion, Item_FullPotion, Item_StickyThread, Item_SteelThread, Item_StickySteelThread, Item_MagicalOre, Item_MagisteelIngot, Item_MagisteelSword
   - Recipes: Recipe_Hipokute_to_Extract, Recipe_Extract_to_Potion, Recipe_Extract_to_FullPotion, Recipe_Make_StickyThread, Recipe_Sticky_to_SteelThread, Recipe_Combine_StickySteel, Recipe_MagOre_to_MagIngot, Recipe_MagIngot_to_MagSword

2) В сцене создайте объекты:
   - AlchemyStation (плейсхолдер игровой объект с компонентом Assets/Scripts/Crafting/AlchemyStation.cs)
   - BlacksmithStation (плейсхолдер с компонентом BlacksmithStation.cs)

3) Откройте созданные Recipe SO в инспекторе и при необходимости отредактируйте inputs/outputs и craftTime.

4) Тестинг:
   - В режиме Play вызовите метод EnqueueRecipe на компоненте AlchemyStation / BlacksmithStation (например через Debug/привязку в инспекторе или вызов из другого тестового скрипта) и следите за логами — по завершению рецепты будут логироваться как произведённые.

Дальше: интеграция с инвентарем и системой поселения будет реализована в следующих итерациях. Сейчас станции выводят результаты в лог — это точка интеграции для того, чтобы подключить добавление предметов в хранилище Темпеста/игрока.
