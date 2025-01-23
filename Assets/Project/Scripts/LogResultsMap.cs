using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.XR.Interaction.Toolkit;

public class LogResultsMap : MonoBehaviour
{
    //Order is: 2D, 3D, 3D_interactive
    public GameObject[] maps;
    public GameObject[] miniMaps;
    public int currentTest;

    public Transform cameraTransform;
    public CounterScript counterScript;
    private List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable> paintings;  // List of interactable paintings (including correct and wrong ones)
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable correctPainting;  
    
    // Get the name of the file we are writing
    public LogInitial logInitial;

    private float startTime; 

    // Track the previous state of the map
    private bool wasMapOpen = false;  

    void Start()
    {
       startTime = Time.time;
       paintings = counterScript.wrongPaintings;


       if(maps[0] != null){
           currentTest = 0;
           log2dMap();
       } 
       else if(maps[1] != null){
           currentTest = 1;
           log3dMap();
       } else {
           currentTest = 2;
           log3dInteractiveMap();
            if (miniMaps[2] != null)
            {
                RegisterChildInteractables(miniMaps[2]);
            }
       }
        correctPainting.selectEntered.AddListener(OnCorrectPaintingSelected);
        foreach (var painting in paintings)
        {
            painting.selectEntered.AddListener(OnWrongPaintingSelected);
        }
    }

    private void RegisterChildInteractables(GameObject parentObject)
    {
        foreach (Transform child in parentObject.transform)
        {
            if (child.TryGetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>(out var interactable))
            {
                interactable.selectEntered.AddListener(OnMiniMapSelected);
            }
        }
    }

    private void log2dMap(){

        string mapType = "2D Map";
        File.AppendAllText(logInitial.logFilePath, mapType+ "\n");
    
        StartCoroutine(LogCameraAndObjectsData());
    }
    
    private void log3dMap(){

        string mapType = "3D Map";
        File.AppendAllText(logInitial.logFilePath, mapType+ "\n");
    
        StartCoroutine(LogCameraAndObjectsData());
    }

    private void log3dInteractiveMap(){

        string mapType = "3D Interactive Map";
        File.AppendAllText(logInitial.logFilePath, mapType+ "\n");
    
        StartCoroutine(LogCameraAndObjectsData());
    }

    IEnumerator LogCameraAndObjectsData()
    {
        while (true)
        {
            float elapsedTime = Time.time - startTime;

            Vector3 camPosition = cameraTransform.position;

            string logEntry = $"{elapsedTime:F2},User Position,{camPosition.x:F2},{camPosition.y:F2},{camPosition.z:F2}";
            File.AppendAllText(logInitial.logFilePath, logEntry + "\n");

            CheckMapState(elapsedTime, camPosition);

            yield return new WaitForSeconds(1f);
        }
    }

    private void OnMiniMapSelected(SelectEnterEventArgs args)
    {
        float elapsedTime = Time.time - startTime;

        Vector3 camPosition = cameraTransform.position;
        GameObject selectedObject = args.interactableObject.transform.gameObject;

        string logEntry = $"{elapsedTime:F2},MiniMap Room Selected,{camPosition.x:F2},{camPosition.y:F2},{camPosition.z:F2},{selectedObject.name}";
        File.AppendAllText(logInitial.logFilePath, logEntry + "\n");
    }

    private void OnDestroy()
    {
        if (miniMaps[2] != null)
        {
            foreach (Transform child in miniMaps[2].transform)
            {
                if (child.TryGetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>(out var interactable))
                {
                    interactable.selectEntered.RemoveListener(OnMiniMapSelected);
                }
            }
        }

        correctPainting.selectEntered.RemoveListener(OnCorrectPaintingSelected);
        foreach (var painting in paintings)
        {
            painting.selectEntered.RemoveListener(OnWrongPaintingSelected);
        }
    }

    private void CheckMapState(float elapsedTime, Vector3 camPosition)
    {
        if (miniMaps[currentTest].activeSelf && !wasMapOpen)
        {
            // Map just opened
            wasMapOpen = true;
            string logEntry = $"{elapsedTime:F2},Map Opened,{camPosition.x:F2},{camPosition.y:F2},{camPosition.z:F2}";
            File.AppendAllText(logInitial.logFilePath, logEntry + "\n");
        }
        else if (!miniMaps[currentTest].activeSelf && wasMapOpen)
        {
            // Map just closed
            wasMapOpen = false;
            string logEntry = $"{elapsedTime:F2},Map Closed,{camPosition.x:F2},{camPosition.y:F2},{camPosition.z:F2}";
            File.AppendAllText(logInitial.logFilePath, logEntry + "\n");
        }
    }

    private void OnCorrectPaintingSelected(SelectEnterEventArgs args)
    {
        LogSelectionResult(args, "Correct");
    }

    private void OnWrongPaintingSelected(SelectEnterEventArgs args)
    {
        LogSelectionResult(args, "Wrong");
    }

    private void LogSelectionResult(SelectEnterEventArgs args, string result)
    {
        float elapsedTime = Time.time - startTime;

        Vector3 camPosition = cameraTransform.position;

        string logEntry = $"{elapsedTime:F2},Object Selected,{camPosition.x:F2},{camPosition.y:F2},{camPosition.z:F2},{result}";
        File.AppendAllText(logInitial.logFilePath, logEntry + "\n");
    }
}
