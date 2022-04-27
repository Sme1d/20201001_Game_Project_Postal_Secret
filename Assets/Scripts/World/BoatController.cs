using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    [HideInInspector] public MainController  main;
    public                   List<Transform> routeToIsland0;
    public                   List<Transform> routeToIsland1;
    public                   List<Transform> routeToIsland2;
    public                   List<Transform> routeToIsland3;
    public                   List<Transform> routeToIsland4;
    public                   Animator        animator;
    public                   ParticleSystem  smoke;
    public                   bool            isMoving;

    private int _currentIslandIndex;
    private int _pathIndex;
    private int _pathLength;

    private List<List<Transform>> _routesToIslands;

    public void Awake()
    {
        _routesToIslands = new List<List<Transform>> {routeToIsland0, routeToIsland1, routeToIsland2, routeToIsland3, routeToIsland4};
    }

    public void ShipToIsland(int islandIndex)
    {
        StartStopBoat(true);
        _currentIslandIndex = islandIndex;
        _pathIndex          = 1;
        _pathLength         = _routesToIslands[_currentIslandIndex].Count;

        if (_currentIslandIndex != 0)
            _routesToIslands[0] = new List<Transform>(new Transform[_pathLength]);

        StartCoroutine(RotateTowardsNextWayPoint(true));
    }

    private IEnumerator RotateTowardsNextWayPoint(bool isFirstRotation)
    {
        var startRotY        = transform.eulerAngles.y;
        var direction        = _routesToIslands[_currentIslandIndex][_pathIndex].position - transform.position;
        var rotationLength   = Vector3.SignedAngle(transform.right, direction, Vector3.up);
        var startTime        = Time.time;
        var rotationFraction = 0f;

        while (rotationFraction < 1)
        {
            rotationFraction = (Time.time - startTime) * Constants.BoatRotatingSpeed / Mathf.Abs(rotationLength);
            var rot = new Vector3(0, Mathf.SmoothStep(startRotY, startRotY + rotationLength, rotationFraction), 0);
            transform.eulerAngles = rot;
            yield return null;
        }

        if (isFirstRotation)
            StartCoroutine(Move2NextWayPoint());
    }

    private IEnumerator Move2NextWayPoint()
    {
        if (_currentIslandIndex != 0)
            _routesToIslands[0][_pathLength - _pathIndex] = _routesToIslands[_currentIslandIndex][_pathIndex - 1];

        var startPos = transform.position;
        var endPos   = _routesToIslands[_currentIslandIndex][_pathIndex].position;

        var startTime = Time.time;
        var distance  = Vector3.Distance(startPos, endPos);
        var fraction  = 0f;

        while (fraction < 1)
        {
            fraction           = (Time.time - startTime) * Constants.BoatMovingSpeed / distance;
            transform.position = Vector3.Lerp(startPos, endPos, fraction);
            yield return null;
        }

        if (_pathIndex < _pathLength - 1)
        {
            _pathIndex++;
            StartCoroutine(Move2NextWayPoint());
            StartCoroutine(RotateTowardsNextWayPoint(false));
        }
        else
        {
            StartStopBoat(false);
            main.ArrivalOnIsland(_currentIslandIndex);
        }
    }

    public IEnumerator ShipTo(Vector3 endPos, float speed, bool notifyMain = false, float waitTime = 0f, bool restartGame = false)
    {
        var startPos  = transform.position;
        var startTime = Time.time;
        while (Time.time - startTime < waitTime) yield return null;

        StartStopBoat(true);

        var startRotY      = transform.eulerAngles.y;
        var direction      = endPos - startPos;
        var rotationLength = Vector3.SignedAngle(transform.right, direction, Vector3.up);
        startTime = Time.time;
        var rotationFraction = 0f;

        while (rotationFraction < 1)
        {
            rotationFraction = (Time.time - startTime) * Constants.BoatRotatingSpeed / Mathf.Abs(rotationLength);
            var rot = new Vector3(0, Mathf.SmoothStep(startRotY, startRotY + rotationLength, rotationFraction), 0);
            transform.eulerAngles = rot;
            yield return null;
        }

        startTime = Time.time;
        var distance = Vector3.Distance(startPos, endPos);
        var fraction = 0f;

        while (fraction < 1)
        {
            fraction           = (Time.time - startTime) * speed / distance;
            transform.position = Vector3.Lerp(startPos, endPos, fraction);
            yield return null;
        }

        StartStopBoat(false);

        if (!notifyMain) yield break;

        if (restartGame)
            main.StartCredits();
        else
            main.BeginNewGame();
    }

    private void StartStopBoat(bool isMoving)
    {
        this.isMoving = isMoving;
        animator.SetTrigger(isMoving ? Constants.TriggerStartBoatMoving : Constants.TriggerStartBoatResting);

        if (isMoving)
        {
            smoke.Play();
            SoundController.StartBoatSound();
        }
        else
        {
            smoke.Stop();
            SoundController.StopBoatSound();
        }
    }

    public void ResetBoatToDefault()
    {
        transform.rotation = Quaternion.Euler(Constants.BoatDefaultOrientation);
        transform.position = Constants.BoatDefaultPosition;
    }

    public void ResetBoatToStart()
    {
        transform.rotation = Quaternion.Euler(Constants.BoatDefaultOrientation);
        transform.position = Constants.BoatStartPosition;
    }
}