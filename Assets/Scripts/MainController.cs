using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class MainController : MonoBehaviour
{
    public SeasonController season;

    public CameraController       cam;
    public BoatController         boat;
    public List<IslandController> islands;

    // UI
    public  GameUIController        gameUi;
    public  IslandUIController      islandUi;
    public  PostUIController        postUi;
    public  InformationUIController infoUi;
    public  UIBase                  menuUi;
    public  TutorialUIController    tutorialUi;
    public  OutroUIController       outro;
    public  FadeUI                  fade;
    public  GameObject              creditsGO;
    private DataService             _data;

    private bool                       _gameHasStarted;
    private InformationQueueController _infoQueue;
    private LoadSaveController         _loadSave;
    private int                        _msgAppearedCounter;
    private int                        _msgDeliveredCounter;
    private MessageQueueController     _msgQueue;
    private bool                       _newPickedUpMsg;
    private RelationController         _relations;
    private int                        _seasonCounter;
    private bool                       _tutorialIsActive = true;

    private void Awake()
    {
        var main = GetComponent<MainController>();
        islands.ForEach(island => island.main = main);
        boat.main = main;
        _loadSave = new LoadSaveController();
        _data     = new DataService();
    }

    public void Start()
    {
        _gameHasStarted = false;

        outro.DeactivateUI();
        menuUi.ActivateUI();

        SoundController.StartScore(Constants.MenuScoreIndex);
        SoundController.StartSoundscape();
        ChangeMenu();
        PrepareNewGame();
    }

    private void PrepareNewGame()
    {
        _msgQueue  = new MessageQueueController();
        _infoQueue = new InformationQueueController();
        _relations = new RelationController();

        gameUi.DeactivateUI();
        postUi.DeactivateUI();
        infoUi.DeactivateUI();
        infoUi.DeactivateReport();
        tutorialUi.DeactivateUI();
        islandUi.DeactivateUI();
        tutorialUi.ResetTutorial();
        creditsGO.SetActive(false);

        _tutorialIsActive = true;

        gameUi.SetSeasonWheel(0, 0);
        season.SetSeason(0);

        cam.transform.position = Constants.CamStartPosition;
        boat.ResetBoatToStart();
    }

    private IEnumerator InitiateNewGame()
    {
        if (_gameHasStarted)
        {
            var startTime = Time.time;
            fade.Fade();
            while (Time.time - startTime < Constants.FadeTime / 2) yield return null;

            PrepareNewGame();
            foreach (var islandController in islands) islandController.SetNotifier(false);
            postUi.DisplayMsg(_msgQueue.PickedUpMsg);

            while (Time.time - startTime < Constants.FadeTime) yield return null;
        }

        _seasonCounter       = 0;
        _msgAppearedCounter  = 0;
        _msgDeliveredCounter = 0;
        _gameHasStarted      = true;
        islands[Constants.PostIndex].SetFlashingActive(false);
        _loadSave.DeleteSave();
        Invoke(nameof(ChangeMenu), 2);

        SoundController.StartScore(_seasonCounter);
        StartCoroutine(boat.ShipTo(Constants.BoatDefaultPosition, Constants.BoatMovingSpeed, true));
        StartCoroutine(cam.MoveCam(Constants.CamDefaultPosition, Constants.CamPanSpeed));
    }

    public void BeginNewGame()
    {
        gameUi.ActivateUI();
        postUi.ActivateUI();
        tutorialUi.NextTutorialStep();
    }

    public void LoadFirstMsg()
    {
        LoadNewInformation(_data.GetInformation(1));
        StartCoroutine(AddNewMsg(_data.GetLetter(Constants.LouIndex, Constants.TobbeIndex, _msgQueue.AlreadyLoadedLetterKeys, true)));
        tutorialUi.NextTutorialStep();
    }

    private void EndOfSeason()
    {
        SoundController.StartScore(Constants.ReportScoreIndex);
        infoUi.ActivateReport();
        islands[Constants.PostIndex].SetFlashingActive(true);

        if (_seasonCounter != 0) return;

        tutorialUi._currentStep = Constants.TutorialStepReport - 1;
        tutorialUi.NextTutorialStep(!cam.isZoomedIn);
        _tutorialIsActive = true;
    }

    private IEnumerator InitiateNewSeason(int seasonIndex)
    {
        _seasonCounter       = seasonIndex;
        _msgDeliveredCounter = 0;
        _msgAppearedCounter  = 0;
        _tutorialIsActive    = false;

        fade.Fade();

        var startTime = Time.time;
        while (Time.time - startTime < Constants.FadeTime / 2) yield return null;

        season.SetSeason(_seasonCounter);
        SoundController.StartScore(_seasonCounter);

        gameUi.ActivateUI();
        postUi.ActivateUI();
        infoUi.DeactivateUI();
        infoUi.DeactivateReport();
        tutorialUi.DeactivateUI();
        boat.ResetBoatToDefault();

        while (Time.time - startTime < Constants.FadeTime) yield return null;

        gameUi.RotateSeasonWheel();
        foreach (var msg in _msgQueue.NextSeasonMsg) StartCoroutine(AddNewMsg(msg));
        _msgQueue.NextSeasonMsg = new List<Message>();

        _loadSave.SaveGame(_seasonCounter, _msgQueue, _infoQueue, _relations, _msgAppearedCounter, _msgDeliveredCounter,
                           _newPickedUpMsg, _tutorialIsActive, tutorialUi._currentStep);

        if (_infoQueue.UnveiledInfo.Count == Constants.MaxInfo)
            EndOfSeason();
    }

    private IEnumerator InitiateLoadedGame()
    {
        var save = _loadSave.LoadGame();
        _gameHasStarted         = true;
        _seasonCounter          = save.SeasonCounter;
        _msgAppearedCounter     = save.MsgAppearedCounter;
        _msgDeliveredCounter    = save.MsgDeliveredCounter;
        _tutorialIsActive       = save.TutorialIsActive;
        _newPickedUpMsg         = save.NewPickedUpMsg;
        tutorialUi._currentStep = save.TutorialStep;
        _msgQueue.LoadMsgQueue(save);
        _infoQueue.LoadInfoQueue(save);
        _relations.LoadRelations(save);
        gameUi.SetSeasonWheel(save.SeasonCounter, save.MsgDeliveredCounter);

        fade.Fade();
        var startTime = Time.time;
        while (Time.time - startTime < Constants.FadeTime / 2) yield return null;

        season.SetSeason(_seasonCounter);

        SoundController.StartScore(_seasonCounter);

        gameUi.ActivateUI();
        postUi.ActivateUI();
        postUi.DisplayMsg(_msgQueue.PickedUpMsg);
        tutorialUi.DeactivateUI();
        infoUi.DeactivateUI();
        infoUi.DeactivateReport();
        ChangeMenu();

        foreach (var msg in _msgQueue.WaitingMsg)
        {
            islands[msg.Sender].SetNotifier(false);
            islands[msg.Sender].SetNotifier(true);
        }

        if (_tutorialIsActive)
        {
            tutorialUi._currentStep--;
            tutorialUi.NextTutorialStep(!cam.isZoomedIn);
        }

        boat.ResetBoatToDefault();
        cam.transform.position = Constants.CamDefaultPosition;
        cam.isZoomedIn         = false;

        while (Time.time - startTime < Constants.FadeTime) yield return null;

        if (_infoQueue.UnveiledInfo.Count == Constants.MaxInfo || _msgDeliveredCounter + 1 == (int) Constants.MsgPerSeason)
            EndOfSeason();
    }

    private void EndOfGame(int numOfCorrectAnswers)
    {
        SoundController.StartScore(Constants.OutroScoreIndex);
        outro.PrepareOutro(_infoQueue.UnveiledInfo, numOfCorrectAnswers == 4);
        gameUi.DeactivateUI();
        postUi.DeactivateUI();
        StartCoroutine(cam.MoveCam(Constants.CamEndPosition, Constants.CamPanOutroSpeed, 1));
        StartCoroutine(boat.ShipTo(Constants.BoatEndPosition, Constants.BoatMovingOutroSpeed, true, 1, true));
    }

    public void StartCredits()
    {
        outro.DeactivateUI();
        creditsGO.SetActive(true);
        StartCoroutine(BackToMenu(Constants.CreditsTime));
    }

    private IEnumerator BackToMenu(float waitingTime = 0)
    {
        var startTime = Time.time;
        while (Time.time - startTime < waitingTime) yield return null;

        startTime = Time.time;
        fade.Fade();
        while (Time.time - startTime < Constants.FadeTime / 2) yield return null;

        Start();
    }

    // Interaction with MenuUI

    public void ClickOnContinue()
    {
        menuUi.GetComponent<Animator>().SetTrigger(Constants.TriggerCloseMenu);

        if (_gameHasStarted) return;

        StartCoroutine(InitiateLoadedGame());
    }

    public void ClickOnNewGame()
    {
        menuUi.GetComponent<Animator>().SetTrigger(Constants.TriggerCloseMenu);
        StartCoroutine(InitiateNewGame());
    }

    public void ClickOnQuit()
    {
        if (!_gameHasStarted)
        {
            Application.Quit();
        }
        else
        {
            _loadSave.SaveGame(_seasonCounter, _msgQueue, _infoQueue, _relations, _msgAppearedCounter, _msgDeliveredCounter,
                               _newPickedUpMsg, _tutorialIsActive, tutorialUi._currentStep);

            StartCoroutine(BackToMenu());
        }
    }

    // Interaction with environment

    public void ClickOnIsland(int islandIndex, Vector3 islandPos)
    {
        if (cam.isMoving) return;

        if (cam.isZoomedIn)
        {
            ZoomOutRoutine();
        }
        else
        {
            postUi.DeactivateUI();
            gameUi.DeactivateUI();
            islands[islandIndex].outlineIsActive = false;
            cam.ZoomIn(islandPos);
            tutorialUi.DeactivateUI();

            SoundController.PlaySound(Constants.IslandZoomInSound);
            SoundController.SetSoundscape(islandIndex);

            if (islandIndex != Constants.PostIndex)
            {
                islandUi.ActivateUI(islandIndex, _relations.GetRelationsWithIsland(islandIndex));
            }
            else
            {
                infoUi.ActivateUI(_infoQueue.UnveiledInfo);
                if (_tutorialIsActive && _msgDeliveredCounter + 1 == Constants.MsgPerSeason)
                    _tutorialIsActive = false;
            }
        }
    }

    public void ClickOnPier(int islandIndex)
    {
        if (boat.isMoving) return;

        SoundController.PlaySound(Constants.IslandPierSound);
        boat.ShipToIsland(islandIndex);
        SetShipping(false);
    }

    public void ClickOnOcean()
    {
        if (!cam.isZoomedIn) return;

        ZoomOutRoutine();
    }

    private void SetShipping(bool shippingIsPossible)
    {
        for (var i = Constants.TobbeIndex; i <= Constants.OleIndex; i++)
        {
            var pier = islands[i].GetComponentInChildren<PierController>();
            pier.outlineIsActive = shippingIsPossible;
            pier.gameObject.tag  = shippingIsPossible ? Constants.CursorTags[2] : Constants.CursorTags[0];
        }

        postUi.SetProceedButtonInteractable(shippingIsPossible);
    }

    // World Events

    public void ArrivalOnIsland(int islandIndex)
    {
        if (islandIndex != Constants.PostIndex)
        {
            // Loading Messages
            _newPickedUpMsg = _msgQueue.PickUpMsg(islandIndex);
            if (_newPickedUpMsg)
                islands[islandIndex].SetNotifier(false,false,true);

            // Unloading Messages
            if (_msgQueue.ProceededMsg != null && _msgQueue.ProceededMsg.Receiver == islandIndex)
                MsgArrival();

            boat.ShipToIsland(Constants.PostIndex);
        }
        else
        {
            SetShipping(true);
            if (!_newPickedUpMsg) return;

            _newPickedUpMsg = false;
            SoundController.PlaySound(Constants.LetterInBarSound);
            postUi.DisplayMsg(_msgQueue.PickedUpMsg);

            if (_tutorialIsActive && tutorialUi._currentStep == Constants.TutorialStepFirstMsg)
                tutorialUi.NextTutorialStep(!cam.isZoomedIn);
        }
    }

    private void MsgArrival()
    {
        var msg = _msgQueue.DeliverMsg();
        _msgDeliveredCounter++;
        gameUi.RotateSeasonPointer();

        switch (msg)
        {
            case Letter letter:
            {
                var sender          = letter.Sender;
                var receiver        = letter.Receiver;
                var letterEffect    = letter.MsgEffect;
                var markerIsCrossed = _relations.UpdateRelation(sender, receiver, letterEffect);
                var reactionIndex   = 0;

                if (letterEffect != 0)
                    reactionIndex = Mathf.Min(Mathf.Max(letterEffect, 1), 2);

                islands[receiver].TriggerReaction(reactionIndex);

                // Information is triggered if marker was crossed by change
                if (markerIsCrossed)
                    StartCoroutine(AddNewMsg(_infoQueue.GetInfo(sender, receiver)));
                else
                    LookForNewLetter();

                break;
            }
            case Information _:
                LookForNewLetter();
                break;
        }
    }

    // Interactions with Post UI

    public void ClickOnMsg(int msgIndex)
    {
        if (_msgQueue.OpenedMsg != null) return;

        var msg         = _msgQueue.OpenMsg(msgIndex);
        var msgRelation = _relations.GetRelation(msg.Sender, msg.Receiver);

        postUi.OpenMsg(msg, msgRelation);
        postUi.SetFreezeOnMsg(true);
        SoundController.PlaySound(Constants.LetterOpenSound);

        if (_tutorialIsActive && tutorialUi._currentStep == Constants.TutorialStepOpenMsg)
            tutorialUi.NextTutorialStep();
    }

    public void ClickOnPS(int psIndex)
    {
        _msgQueue.SelectPs(psIndex);
    }

    public void ClickOnProceed()
    {
        postUi.SetFreezeOnMsg(false);
        var openedMsg = _msgQueue.OpenedMsg;
        var receiver  = openedMsg.Receiver;

        if (_tutorialIsActive && tutorialUi._currentStep == Constants.TutorialStepManipulate)
        {
            tutorialUi.CloseCurrentBubble(false);
            _tutorialIsActive = false;
            LookForNewInfo();
            LookForNewLetter();
        }

        if (openedMsg is Information info)
        {
            islands[Constants.PostIndex].TriggerReaction(Constants.ReactionInfoIndex, info.Key);
            if (_infoQueue.UnveilInfo(info.Key))
                EndOfSeason();
            else
                LookForNewInfo();
        }

        _msgQueue.ProceedMsg();
        postUi.ProceedMsg();
        postUi.DisplayMsg(_msgQueue.PickedUpMsg);
        SetShipping(false);
        boat.ShipToIsland(receiver);
    }

    // Interaction with InformationUI

    public void ClickOnInformation(int informationIndex)
    {
        infoUi.OpenInfo(_data.GetInformation(informationIndex));
    }

    // Interaction with ReportUI

    public void ClickOnShowResults()
    {
        var numOfCorrectAnswers =
            infoUi.AnalyzeResult(_seasonCounter);

        SoundController.PlaySound(Constants.AccomplishedSounds[numOfCorrectAnswers]);
        SoundController.StopScore();
    }

    public void ClickOnSubmitReport()
    {
        var numOfCorrectAnswers = infoUi.givenAnswers.Count(givenAnswer => Constants.CorrectAnswers.Contains(givenAnswer));
        islands[Constants.PostIndex].SetFlashingActive(false);

        ZoomOutRoutine();

        if (numOfCorrectAnswers == 4 || _seasonCounter == Constants.MaxSeasons - 1)
            EndOfGame(numOfCorrectAnswers);
        else
            StartCoroutine(InitiateNewSeason(_seasonCounter + 1));
    }

    // Routines
    private void ZoomOutRoutine()
    {
        cam.ZoomOut();
        foreach (var island in islands) island.outlineIsActive = true;
        SoundController.PlaySound(Constants.IslandZoomOutSound);
        SoundController.SetSoundscape(Constants.OverviewIndex);

        islandUi.LeaveIsland();
        infoUi.DeactivateUI();
        postUi.ActivateUI();
        gameUi.ActivateUI();

        if (_tutorialIsActive)
            tutorialUi.ActivateUI();
    }

    private void LookForNewInfo()
    {
        var newInformation = _data.GetUnknownInformation(_infoQueue.GetPairsWithWaitingInfo(), _infoQueue.AlreadyLoadedInfoKeys);
        if (newInformation != null)
            LoadNewInformation(newInformation);
    }

    private void LookForNewLetter()
    {
        var    random = new Random();
        Letter newLetter;
        var    pairsWithWaitingInfo = _infoQueue.GetPairsWithWaitingInfo();

        if (random.NextDouble() <= Constants.ChanceForRelevantLetter && pairsWithWaitingInfo.Count > 0)
            newLetter = LoadRelevantLetter(random, pairsWithWaitingInfo);
        else
            newLetter = LoadRandomLetter(random);

        StartCoroutine(AddNewMsg(newLetter));
    }

    private Letter LoadRelevantLetter(Random random, IReadOnlyList<(int, int)> pairsWithWaitingInfo)
    {
        var (item1, item2) = pairsWithWaitingInfo[random.Next(0, pairsWithWaitingInfo.Count)];
        return _data.GetLetter(item1, item2, _msgQueue.AlreadyLoadedLetterKeys);
    }

    private Letter LoadRandomLetter(Random random)
    {
        var p1 = random.Next(Constants.TobbeIndex, Constants.OleIndex);
        var p2 = random.Next(Constants.TobbeIndex, Constants.OleIndex);
        while (p1 == p2)
            p2 = random.Next(Constants.TobbeIndex, Constants.OleIndex);

        return _data.GetLetter(p1, p2, _msgQueue.AlreadyLoadedLetterKeys);
    }

    private void LoadNewInformation(Information newInfo)
    {
        _infoQueue.AddWaitingInfo(newInfo);
        _relations.GetRelation(newInfo.Sender, newInfo.Receiver).ActivateMarker(newInfo.RelationValue);
    }

    private IEnumerator AddNewMsg(Message newMsg)
    {
        var startTime = Time.time;
        while (Time.time - startTime < 1) yield return null;

        if (_msgAppearedCounter + 1 < Constants.MsgPerSeason)
        {
            _msgAppearedCounter++;
            _msgQueue.AddWaitingMsg(newMsg);
            islands[newMsg.Sender].SetNotifier(true, newMsg is Information);
        }
        else
        {
            StoreMsgForNextSeason(newMsg);
        }
    }

    private void StoreMsgForNextSeason(Message msgForNextSeason)
    {
        _msgQueue.NextSeasonMsg.Add(msgForNextSeason);
        if (_msgDeliveredCounter + 1 == (int) Constants.MsgPerSeason)
            EndOfSeason();
    }

    private void ChangeMenu()
    {
        menuUi.GetComponent<Animator>().SetBool(Constants.BoolIsNewGame, !_loadSave.GameSaveExists() || _gameHasStarted);
        menuUi.transform.GetChild(1).GetChild(0).GetChild(1).tag =
            _loadSave.GameSaveExists() || _gameHasStarted ? Constants.CursorTags[1] : Constants.CursorTags[0];

        menuUi.transform.GetChild(1).GetChild(0).GetComponentInChildren<Text>().text = _gameHasStarted ? "Continue" : "Load Game";
        menuUi.transform.GetChild(1).GetChild(3).GetComponentInChildren<Text>().text =
            _gameHasStarted ? "Save and Quit" : "Exit Game";
    }
}