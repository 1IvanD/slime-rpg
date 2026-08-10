# Dialogue setup

How to test dialog system quickly:
1) In Unity: Tools -> Tempest -> Generate Example Dialogues. This will create sample DialogueNodeSO and DialogueTreeSO assets under Assets/Data/Dialogue.
2) Create Canvas/UI and add a UI panel with the following components referenced in DialogueUIController:
   - rootPanel (panel GameObject) - contains all UI
   - speakerNameText (Text)
   - speakerIconImage (Image)
   - dialogueText (Text)
   - choicesContainer (RectTransform)
   - choiceButtonPrefab (Button prefab with Text child)
3) Add DialogueManager to a GameObject in scene and assign the DialogueUIController reference.
4) Add NPCDialogueController to any NPC prefab, assign DialogueTreeSO and enable showActionMenuOnApproach.
5) Approach NPC and press E to open dialogue. Choose options — quest will be added via QuestManager.
