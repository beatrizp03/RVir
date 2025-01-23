using UnityEngine;
using System.IO;

public class LogInitial : MonoBehaviour
{
    public string logFilePath;

    void Start()
    {
        // Generate a unique file name with incremental numbering
        logFilePath = GenerateUniqueLogFileName();

        // Write the header for the log file
        string header = "Timestamp,Event,UserPosX,UserPosY,UserPosZ,SelectionResult";
        File.WriteAllText(logFilePath, header + "\n");
    }

    private string GenerateUniqueLogFileName()
    {
        string folderPath = Application.persistentDataPath; 
        string baseFileName = "wim_log";
        string fileExtension = ".csv";
        int fileIndex = 1;

        string fileName = $"{baseFileName}{fileIndex}{fileExtension}";
        string fullPath = Path.Combine(folderPath, fileName);

        // Increment the file index until we find a non-existing file
        while (File.Exists(fullPath))
        {
            fileIndex++;
            fileName = $"{baseFileName}{fileIndex}{fileExtension}";
            fullPath = Path.Combine(folderPath, fileName);
        }

        return fullPath;
    }
}
