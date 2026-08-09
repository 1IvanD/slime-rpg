# Tempest - World setup

Этот файл содержит инструкции по созданию и инициализации базовых ассетов мира (время суток, погода, динамические события).

Как использовать
1) Откройте проект в Unity Editor (рекомендуется версия 2020.3+ или совместимая со скриптами).
2) В меню Unity выберите Tools -> Tempest -> Generate World Assets (Time/Weather/Events).
   - Это создаст в репозитории папку Assets/Data/World и примеры ScriptableObject-ассетов:
     - TimeOfDayConfigSO.asset
     - Weather_Clear.asset
     - Weather_Rain.asset
     - Weather_Fog.asset
     - Weather_Storm.asset
     - Event_Merchant.asset
     - Event_Ambush.asset
     - Event_ForestSpirit.asset
3) В сцене создайте пустой GameObject и добавьте компоненты:
   - WorldTimeManager
   - WeatherSystem
   - DynamicEventManager
4) В инспекторе компонента WorldTimeManager присвойте TimeOfDayConfigSO (Assets/Data/World/TimeOfDayConfigSO.asset).
5) В инспекторе компонента WeatherSystem добавьте созданные Weather_* ассеты в список Available Weathers.
6) В инспекторе DynamicEventManager добавьте Event_* ассеты в список Events.
7) Запустите сцену — система начнёт прогресс времени, погоду и случайные события.

Дальше
- Я подготовлю интерфейсы для интеграции с NPC, спавном врагов и торговцами. После базовой проверки запрошу разрешение на добавление производственных и крафтовых SO (ключевые рецепты).