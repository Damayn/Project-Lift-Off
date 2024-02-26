using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GXPEngine;
/*
[Serializable]
public class GameState
{
    // Define properties to represent the state of your game
    public int playerScore;
    // Add more properties as needed
    public GameState()
    {
        // Initialize default values
        playerScore = 0;

    }

    public void IncreaseScore(int pointsToAdd)
    {

        playerScore += pointsToAdd;

        Console.WriteLine(playerScore);

    }

}

public class SavingExample : Game
{
    private GameState gameState;
    private string saveFilePath = "savegame.dat";

    public SavingExample() : base(800, 600, false, true, -1, -1, false)
    {
        // Load saved game state if available
        if (File.Exists(saveFilePath))
        {
            LoadGameState();
        }
        else
        {
            // Initialize new game state if no save file exists
            gameState = new GameState();
        }

        // Your game initialization code goes here
    }

    void Update()
    {
        // Your game update logic goes here

        // Example: Save the game state when the player presses a key (e.g., "S" key)
        if (Input.GetKeyDown(Key.S))
        {

            SaveGameState();

        }

        if (Input.GetKeyDown(Key.D))
        {

            DeleteSaveFile();

        }

        if (Input.GetKeyDown(Key.A))
        {

            gameState.IncreaseScore(10); // Increase the player's score by 10 points



        }


    }

    void SaveGameState()
    {
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = File.Open(saveFilePath, FileMode.Create);
            formatter.Serialize(fileStream, gameState);
            fileStream.Close();
            Console.WriteLine("Game state saved.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to save game state: " + e.Message);
        }
    }

    void LoadGameState()
    {
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = File.Open(saveFilePath, FileMode.Open);
            gameState = (GameState)formatter.Deserialize(fileStream);
            fileStream.Close();
            Console.WriteLine("Game state loaded.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to load game state: " + e.Message);
            gameState = new GameState(); // Initialize new game state
        }
    }

    void DeleteSaveFile()
    {
        try
        {
            if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
                Console.WriteLine("Save file deleted.");
            }
            else
            {
                Console.WriteLine("No save file exists.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to delete save file: " + e.Message);
        }
    }

    static void Main(string[] args)
    {
        new SavingExample().Start();
    }
}
*/