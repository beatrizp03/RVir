using UnityEngine;

public class ChangeTrial : MonoBehaviour
{
    public RandomizeMaps randomizeMaps; 

   public void OnNextButtonPressed()
   {
        randomizeMaps.currentIndex++;

        // Check if there are more trials
        if (randomizeMaps.currentIndex < randomizeMaps.randomizedOrder.Length)
        {
            // Activate the next trial
            randomizeMaps.ActivateCurrentTrial();    
        }
   }
}
