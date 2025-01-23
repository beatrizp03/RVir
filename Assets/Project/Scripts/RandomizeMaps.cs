using UnityEngine;
using TMPro;

public class RandomizeMaps : MonoBehaviour
{
    [Header("Architectures, Maps, and User Positions Setup")]
    public GameObject[] architectures; 
    public Vector3[] userPositions; 
    public GameObject[] timeUpCanvas; 

    [Header("User Setup")]
    public Transform cameraTransform; 

    [Header("Timer Setup")]
    public float trialDuration = 10f; 

    public int[] randomizedOrder; 
    public int currentIndex = 0; 

    private float timer; 
    private bool isTrialActive = false; 

    void Start()
    {
        if (architectures.Length != userPositions.Length)
        {
            return;
        }

        randomizedOrder = GenerateRandomOrder(architectures.Length);

        ActivateCurrentTrial();
    }

    void Update()
    {
        if (isTrialActive)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                EndCurrentTrial();
            }
        }
    }

    public void ActivateCurrentTrial()
    {
        DisableAll();

        int currentTrialIndex = randomizedOrder[currentIndex];
        architectures[currentTrialIndex].SetActive(true);
        SetUserPosition(userPositions[currentTrialIndex]);
    }

    public void StartTimer()
    {
        timer = trialDuration;
        isTrialActive = true;
    }

    public void StopTimer()
    {
        isTrialActive = false;
    }

    private void EndCurrentTrial()
    {
        if (isTrialActive)
        {
            timeUpCanvas[randomizedOrder[currentIndex]].SetActive(true);
            isTrialActive = false;
        }
    }

    private void DisableAll()
    {
        foreach (GameObject architecture in architectures)
        {
            architecture.SetActive(false);
        }
    }

    private void SetUserPosition(Vector3 position)
    {
        if (cameraTransform != null)
        {
            cameraTransform.position = position;
        }
    }

    private int[] GenerateRandomOrder(int count)
    {
        int[] order = new int[count];
        for (int i = 0; i < count; i++)
        {
            order[i] = i;
        }

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, count);
            int temp = order[i];
            order[i] = order[randomIndex];
            order[randomIndex] = temp;
        }

        return order;
    }
}
