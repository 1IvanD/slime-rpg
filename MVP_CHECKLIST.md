# 🎮 slime-rpg MVP Checklist

**Статус**: Planning → Development  
**Целевая дата**: 30 дней (примерно)  
**Главная цель**: Создать "играбельный" релиз с основными системами

---

## 📋 MVP - Минимальный набор функций для "играбельного" релиза

### ✅ GAMEPLAY CORE

- [x] **Боевая система (CombatManager)**
  - [x] Атаки (расчет урона, попадания)
  - [x] Здоровье (HP система)
  - [x] XP и уровни (автоматический левелап)
  - [x] Лут (таблица дропа, награды)
  - [x] Анимации (Attack, Hit, Death, Dodge)
  - [x] VFX (заготовки для эффектов)
  - [ ] **TODO**: Интегрировать с врагами и NPC

- [ ] **Система квестов (QuestManager)** ✅ Создана
  - [x] Prerequisites (проверка зависимостей)
  - [x] Rewards (XP, золото, предметы)
  - [x] Отслеживание прогресса (objectives)
  - [x] События (OnQuestAccepted, OnQuestCompleted)
  - [ ] **TODO**: Автоматический старт квестов при встречи с NPC
  - [ ] **TODO**: Диалоговые варианты зависят от состояния квеста

- [ ] **Система диалогов (DialogueSystem)** ❌ Не создана
  - [ ] Простой диалоговый граф (текст, варианты ответов)
  - [ ] Связь с QuestManager (принять квест через диалог)
  - [ ] Связь с NPCManager (аффинитет влияет на опции)
  - [ ] UI в центре экрана с выбором ответов
  - [ ] Логирование диалогов в журнал

- [ ] **UI для квестов и диалогов**
  - [ ] **Quest Panel** (в углу: активные квесты, прогресс)
  - [ ] **Quest Journal** (полный журнал, завершенные, активные)
  - [ ] **Dialogue UI** (по центру экрана, варианты ответов как кнопки)
  - [ ] **NPC Nameplate** (имя NPC при диалоге)
  - [ ] Плавные переходы и анимации

### 🎮 GAME WORLD

- [ ] **Стартовый уровень + Hub**
  - [ ] Сцена: Hub/Village (стартовая область, спавн игрока)
  - [ ] Стартовое взаимодействие: обучение управлению
  - [ ] Спавнеры врагов (простые враги для первых боев)

- [ ] **5-7 квестов для MVP**
  - [x] Структура квестов создана (QuestDef ScriptableObject)
  - [ ] **Квест 1**: "Собрать 5 ягод" (сбор)
  - [ ] **Квест 2**: "Победить 3 гоблина" (боевой)
  - [ ] **Квест 3**: "Поговорить с кузнецом" (диалог)
  - [ ] **Квест 4**: Зависит от Квеста 1 (prerequisite)
  - [ ] **Квест 5-7**: Боевые/диалоговые миксы
  - [ ] Автоматическое завершение при выполнении условий

- [ ] **NPC с диалогами и вербовкой**
  - [ ] **минимум 3 NPC** (дают квесты, можно завербовать в команду)
  - [ ] Система аффинитета (отношение 0-100)
  - [ ] Диалоговые опции зависят от аффинитета (>50 = новые опции)
  - [ ] UI показывает текущий аффинитет
  - [ ] Вербовка: после определенного аффинитета NPC присоединяется

- [ ] **Карта мира с переходами**
  - [ ] Минимум 2 локации (Hub + первая подземелье/лес)
  - [ ] Переходы между сценами (двери, портали)
  - [ ] Сохранение состояния при переходе
  - [ ] Визуальная навигация (карта или иконки)

- [ ] **Базовый бой**
  - [ ] Спавнер врагов (простые враги)
  - [ ] Боевой AI (атаки в свою очередь или по таймеру)
  - [ ] Победа над врагом = XP + лут
  - [ ] Поражение = Game Over или откат на спавн

### 💾 TECHNICAL

- [x] **Система сохранения/загрузки** ✅ Создана (SaveManager)
  - [x] JSON сериализация
  - [x] Полное состояние игры (Player, Quests, NPC affinity, War, Settlements)
  - [x] Версионирование
  - [x] Backup системa
  - [ ] **TODO**: Интегрировать GatherWorldState() со всеми системами

- [x] **Namespaces (Tempest.*)**
  - [x] SaveManager: `Tempest.Save`
  - [x] CombatManager: `Tempest.Combat`
  - [x] QuestManager: `Tempest.Quest`
  - [ ] **TODO**: Остальные системы (Dialogue, NPC, World, Player, War)
  - [ ] **TODO**: Переименовать существующие классы

- [ ] **Build & Scenes**
  - [ ] Все сцены добавлены в Build Settings
  - [ ] Scene Manager работает (загрузка сцен)
  - [ ] Persistent managers (SaveManager, QuestManager, CombatManager)

### 🎨 VISUALS & UX (Минимум для MVP)

- [ ] **UI Полировка**
  - [ ] Единообразные шрифты, отступы, цвета
  - [ ] HUD (HP bar, XP bar, статус)
  - [ ] Подсказки (кнопки, управление)
  - [ ] Меню паузы (Resume, Save, Load, Settings, Quit)

- [ ] **Базовые анимации** (для игрока и врагов)
  - [ ] Idle, Walk, Attack, Hit, Death
  - [ ] Animator Controller with BlendTrees
  - [ ] Transition smooth

- [ ] **Минимум VFX**
  - [ ] Урон (damage numbers)
  - [ ] Атака (цвет/вспышка)
  - [ ] Смерть (частицы)
  - [ ] Лечение (зеленые частицы)

- [ ] **Звук (базовый)**
  - [ ] Щелчки интерфейса
  - [ ] Звуки шагов
  - [ ] Звуки атак
  - [ ] Фоновая музыка (1 трек)

### 🧪 TESTING & POLISH

- [ ] **Тестирование ключевых систем**
  - [ ] Боевая система (все случаи: попадание, промах, смерть)
  - [ ] Квесты (принять, выполнить, получить награду)
  - [ ] Диалоги (выбор опций, влияние на аффинитет)
  - [ ] Сохранение/загрузка (все системы синхронизированы)

- [ ] **Основные баги исправлены**
  - [ ] Missing Scripts (если есть)
  - [ ] Null references в системах
  - [ ] Неправильные переходы между сценами

- [ ] **Минимальный туториал**
  - [ ] Первые 5-10 минут: как двигаться, атаковать, разговаривать
  - [ ] Интерактивные подсказки при первых действиях

---

## 📊 НЕДЕЛЯ ЗА НЕДЕЛЕЙ (30-дневный план)

### **Неделя 1: Стабилизация кодовой базы** 
*(Завершение infrastructure)*

- [ ] Завершить namespacing (Tempest.* для всех)
- [ ] Исправить Missing Scripts
- [ ] Обновить SaveManager: GatherWorldState(), ApplyWorldState()
- [ ] Integrate SaveManager с CombatManager (XP, Health)
- [ ] Integrate SaveManager с QuestManager (quests, objectives)

**Commit**: SaveManager полностью интегрирован  
**Deliverable**: Game загружается без ошибок, сохранение работает

---

### **Неделя 2: Квесты + Диалоги** 
*(Core gameplay loop)*

- [ ] Создать DialogueSystem (граф, варианты, события)
- [ ] Integrate Dialogue ← Quest (принять квест через NPC)
- [ ] Integrate Dialogue ← NPC Affinity (опции зависят от отношения)
- [ ] Создать 5 квестов (QuestDef ScriptableObjects)
- [ ] Создать 3 NPC с диалогами
- [ ] UI: Quest Panel + Dialogue UI (по центру)

**Commit**: Полный игровой цикл: встреча NPC → диалог → принять квест → выполнить → завершить  
**Deliverable**: Хотя бы 1 квест полностью проходим

---

### **Неделя 3: Враги + Балансировка боя** 
*(Combat Polish)*

- [ ] Создать несколько типов врагов (ScriptableObject with stats)
- [ ] Enemy AI (атаки в свою очередь)
- [ ] Спавнер врагов
- [ ] Балансировка урона, XP, уровней
- [ ] UI боя: HP bars, damage numbers
- [ ] VFX минимум: атака, урон, смерть
- [ ] Звуки боя: атака, попадание, смерть

**Commit**: Враги спавнятся, можно их бить, получать XP  
**Deliverable**: Полная боевая сцена (спавн врагов → бой → лут → XP)

---

### **Неделя 4: Сцены + Полировка + Релиз** 
*(Finish line)*

- [ ] Hub сцена (стартовое место)
- [ ] Первое подземелье/лес (спавнеры врагов)
- [ ] Переходы между сценами
- [ ] Меню паузы / загрузки сохранений
- [ ] Туториал (первые 5 минут)
- [ ] Общая полировка UI/UX
- [ ] Bug fixes + оптимизация
- [ ] Первый тестовый build (Windows/Mac)

**Commit**: Первый MVP build готов к демонстрации  
**Deliverable**: Игра проходима от начала к концу (25-30 мин)

---

## 🎯 ЧТО УЖЕ ГОТОВО (реализовано на текущий момент)

```
✅ CombatManager.cs — полная боевая система
✅ QuestManager.cs — система квестов с prerequisites
✅ SaveManager.cs — сохранения с JSON + versioning
✅ WorldState.cs — структура состояния игры
✅ SaveFileInfo.cs — метаданные сохранений
```

---

## 🔴 КРИТИЧЕСКИЕ ЗАДАЧИ ДЛЯ MVP

**1. DialogueSystem** (2-3 часа)
   - Простой граф диалога
   - Варианты ответов
   - Связь с Quest и NPC Affinity

**2. NPC Manager + Affinity System** (2-3 часа)
   - NPCDef (ScriptableObject)
   - Аффинитет (0-100)
   - Вербовка

**3. Интеграция систем** (3-4 часа)
   - GatherWorldState() / ApplyWorldState()
   - Quest автозавершение
   - Dialogue → Quest acceptance

**4. UI для Quests + Dialogue** (4-5 часов)
   - Quest Journal
   - Dialogue Panel (по центру)
   - Quest log

**5. Враги + Сцены** (6-8 часов)
   - Enemy AI
   - Спавнеры
   - Hub + одно подземелье
   - Переходы между сценами

**6. Полировка + Аудио/VFX** (6-8 часов)
   - Анимации (Attack, Death и т.д.)
   - VFX (урон, атака, лечение)
   - Звуки

---

## 🎁 ДЛЯ POST-MVP (Nice to Have)

- Локализация (RU/EN)
- Advanced Enemy AI (патрули, агро)
- Боевая система turn-based vs real-time
- Инвентарь + система предметов
- Микротранзакции / магазин
- Достижения
- Онлайн лидерборд

---

## 📞 ОСНОВНЫЕ ИНТЕГРАЦИИ ТРЕБУЕМЫЕ

```csharp
// SaveManager ← Quest progress
WorldState.quests = QuestManager.Instance.GetAllActiveQuests()

// SaveManager ← Combat stats
WorldState.playerState.health = player.combatEntity.stats.health
WorldState.playerState.level = player.combatEntity.stats.level

// Dialogue → Quest
if (dialogueOption.triggersQuestId != null)
    QuestManager.Instance.AcceptQuest(dialogueOption.triggersQuestId)

// Dialogue → Affinity
NPC.affinity += dialogueOption.affinityModifier

// Quest completion → Save
OnQuestCompleted → SaveManager.SaveGame("auto")

// Scene transition → Save world state
OnSceneChange → GatherWorldState()
```

---

## 🚀 NEXT STEP

**Первая задача:** Создать **DialogueSystem.cs** (граф, UI, интеграция)  
**Вторая задача:** Создать **NPCManager.cs** + **NPCDef** (аффинитет, вербовка)  
**Третья задача:** Создать **QuestUI.cs** + **DialogueUI.cs**

**Хотите я создам эти файлы сейчас?** ✅/❌
