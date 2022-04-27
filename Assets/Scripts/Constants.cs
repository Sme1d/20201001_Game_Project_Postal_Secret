using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    // System
    public const string DBName                  = "LettersDB.db";
    public const int    PostIndex               = 0;
    public const int    TobbeIndex              = 1;
    public const int    LouIndex                = 2;
    public const int    QuinnIndex              = 3;
    public const int    OleIndex                = 4;
    public const int    OverviewIndex           = 5;
    public const float  MsgPerSeason            = 20;
    public const int    MaxSeasons              = 4;
    public const int    MaxInfo                 = 10;
    public const double ChanceForRelevantLetter = 0.65;

    // Animation Trigger & Bools
    public const string TriggerFade                 = "Fade";
    public const string TriggerCloseIslandUi        = "CloseIslandUi";
    public const string TriggerDeactivateOutline    = "DeactivateOutline";
    public const string TriggerActivateOutline      = "ActivateOutline";
    public const string TriggerStartBoatMoving      = "StartBoatMoving";
    public const string TriggerStartBoatResting     = "StartBoatResting";
    public const string BoolIsNewGame               = "IsNewGame";
    public const string TriggerCloseMenu            = "CloseMenu";
    public const string TriggerSpawnNotifier        = "SpawnNotifier";
    public const string BoolMsgIsInfo               = "MsgIsInformation";
    public const string TriggerStartFlashingOutline = "StartFlashingOutline";
    public const string TriggerStopFlashingOutline  = "StopFlashingOutline";
    public const string TriggerCloseMsg             = "CloseMsg";
    public const string TriggerCloseTutorialBubble  = "CloseTutorialBubble";
    public const int    ReactionInfoIndex           = 3;

    // Main

    // Camera
    public const float CamPanSpeed        = 5.0f;
    public const float CamZoomSpeed       = 60.0f;
    public const float CamPanOutroSpeed   = 0.7f;
    public const float CamYValueZoomedIn  = 9f;
    public const float CamYValueZoomedOut = 23f;

    // Boat
    public const float BoatMovingSpeed      = 3.0f;
    public const float BoatMovingOutroSpeed = 0.7f;
    public const float BoatRotatingSpeed    = 350f;

    // UI
    public const float SeasonPointerRotationStart = -6f;
    public const float SeasonPointerRotationEnd   = -90f;
    public const float SeasonPointerRotationRange = SeasonPointerRotationEnd - SeasonPointerRotationStart;
    public const float SeasonPointerRotationTime  = 0.6f;
    public const float SeasonWheelRotationValue   = -90f;
    public const float SeasonWheelRotationTime    = 0.5f;
    public const float FadeTime                   = 5f; // Is defined in the animation
    public const int   NumberOfDisplayedMsg       = 4;
    public const float CreditsTime                = 23f;

    // Rain 
    public const float RainFadeTime = 2f;

    // Questions
    public const int NumOfQuestions = 4;

    // Sound Parameters
    public const string ParameterSoundscapeTransition = "SoundscapeTransition";
    public const string ParameterSoundscapeVolume     = "SoundscapeVolume";
    public const string ParameterScoreVolume          = "ScoreVolume";
    public const string ParameterScoreTransition      = "ScoreTransition";
    public const string ParameterSfxVolume            = "SFXVolume";
    public const string ParameterRain                 = "Rain";

    // Sound File Paths
    public const string BoatMovingSound      = "event:/SFX/Boat/BoatMoving";
    public const string BoatLoadingSound     = "event:/SFX/Boat/BoatLoading";
    public const string IslandPierSound      = "event:/SFX/Island/IslandPier";
    public const string IslandZoomInSound    = "event:/SFX/Island/IslandZoomIn";
    public const string IslandZoomOutSound   = "event:/SFX/Island/IslandZoomOut";
    public const string LetterOpenSound      = "event:/SFX/Letters/LetterOpen";
    public const string LetterInBarSound     = "event:/SFX/Letters/LetterInBar";
    public const string LetterOnIslandSound  = "event:/SFX/Letters/LetterOnIsland";
    public const string LetterPSSound        = "event:/SFX/Letters/LetterPS";
    public const string MenuButtonSound      = "event:/SFX/UI/MenuButton";
    public const string MenuWhooshSound      = "event:/SFX/UI/MenuWhoosh";
    public const string ReportCheckSound     = "event:/SFX/UI/ReportCheckbox";
    public const string InformationOpenSound = "event:/SFX/UI/InformationOpen";
    public const string TutorialBubbleSound  = "event:/SFX/UI/TutorialBubble";
    public const string SeasonWheelSound     = "event:/SFX/SeasonWheel/SeasonWheel";
    public const string SeasonPointerSound   = "event:/SFX/SeasonWheel/SeasonPointer";
    public const string Score                = "event:/Score/Score";
    public const string Soundscape           = "event:/Soundscape/Soundscape";

    // Score Index
    public const int ReportScoreIndex = 4;
    public const int OutroScoreIndex  = 5;
    public const int MenuScoreIndex   = 6;

    // TutorialSteps
    public const int TutorialStepWelcome    = 0;
    public const int TutorialStepFirstMsg   = 1;
    public const int TutorialStepOpenMsg    = 2;
    public const int TutorialStepManipulate = 3;
    public const int TutorialStepReport     = 4;

    // SavePath
    public const string SaveFilePath = "/gamesave.save";

    // tag
    public static readonly List<string> CursorTags = new List<string> {"Untagged", "Clickable", "Shipping"};

    public static readonly List<string> TriggerReactions = new List<string>
        {"SpawnNeutralReaction", "SpawnAngryReaction", "SpawnHappyReaction", "SpawnInfoReaction"};

    public static readonly List<Color> CharacterColors = new List<Color>
    {
        new Color(0.57f, 0.63f, 0.24f, 1f),
        new Color(1f, 0.7f, 0.35f, 1f),
        new Color(1f, 0.43f, 0.27f, 1f),
        new Color(0.42f, 0.63f, 0.94f, 1f)
    };

    public static readonly Vector3      CamDefaultPosition     = new Vector3(0, CamYValueZoomedOut, -CamYValueZoomedOut);
    public static readonly Vector3      CamStartPosition       = new Vector3(0, CamYValueZoomedIn, -20);
    public static readonly Vector3      CamEndPosition         = new Vector3(0, CamYValueZoomedOut, -131f);
    public static readonly Vector3      BoatDefaultPosition    = new Vector3(1.41f, 0f, -3.22f);
    public static readonly Vector3      BoatStartPosition      = new Vector3(1.7f, 0f, -12f);
    public static readonly Vector3      BoatDefaultOrientation = new Vector3(0f, -90f, 0f);
    public static readonly Vector3      BoatEndPosition        = new Vector3(0f, 0f, -120f);
    public static readonly List<string> CorrectAnswers         = new List<string> {"Lou", "Drowned", "Fear", "Oles Island"};

    public static readonly List<string> AccomplishedSounds = new List<string>
    {
        "event:/SFX/UI/Accomplished0", "event:/SFX/UI/Accomplished1", "event:/SFX/UI/Accomplished2", "event:/SFX/UI/Accomplished3",
        "event:/SFX/UI/Accomplished4"
    };

    public static readonly Dictionary<bool, string> LetterOnIslandSounds = new Dictionary<bool, string>
        {{true, LetterOnIslandSound}, {false, BoatLoadingSound}};

    public static readonly List<string> ReactionSounds = new List<string>
    {
        "event:/SFX/Reactions/ReactionNeutral", "event:/SFX/Reactions/ReactionAngry", "event:/SFX/Reactions/ReactionHappy",
        "event:/SFX/Reactions/ReactionInfo"
    };

    //Strings
    public static readonly List<string> Characters = new List<string> {"Mail Carrier", "Tobbe", "Lou", "Quinn", "Ole"};
    public static readonly List<string> Seasons    = new List<string> {"Winter", "Spring", "Summer", "Autumn"};
}