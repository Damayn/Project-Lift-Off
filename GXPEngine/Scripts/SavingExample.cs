using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using GXPEngine;

[Serializable]
public class GameState
{
    // Define properties to represent the state of your game
    public int playerScore;

    public string username;
    // Add more properties as needed
    public GameState(string username)
    {
        // Initialize default values
        this.username = username;

        playerScore = 0;

    }

    public void IncreaseScore(int pointsToAdd)
    {

        playerScore += pointsToAdd;

        Console.WriteLine($"{username}'s score: {playerScore}");

    }

}

public class SavingExample : GameObject
{
    //naming save file
    // private GameState gameState;

    private Dictionary<string, GameState> savedGames = new Dictionary<string, GameState>();

    private string saveFolderPath = "savegames";

    private string selectedUsername = null;

    public SavingExample() : base()
    {
        // Create savegames folder if it doesn't exist
        if (!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
        }

        // Load saved game states
        LoadSavedGames();
        //options menu
        PresentMenu();

    }

    void Update()
    {
        // Your game update logic goes here

        // Example: Save the game state when the player presses a key (e.g., "S" key)
        if (Input.GetKeyDown(Key.S))
        {

            SaveGameState();

        }
        //test subject of the saved data
        if (Input.GetKeyDown(Key.A))
        {

            IncreaseScore(selectedUsername, 10);

        }

    }

    void PresentMenu()
    {
        Console.WriteLine("Welcome to the SavingExample game!");
        Console.WriteLine("Choose an option:");
        Console.WriteLine("1. Create a new username");
        Console.WriteLine("2. Display saved usernames with scores");

        int choice;
        if (int.TryParse(Console.ReadLine(), out choice))
        {
            switch (choice)
            {
                case 1:
                    CreateNewUsername();
                    break;
                case 2:
                    DisplaySavedUsernames();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    PresentMenu();
                    break;
            }
        }
        else
        {
            Console.WriteLine("Invalid choice. Please try again.");
            PresentMenu();
        }

    }

    void DisplaySavedUsernames()
    {
        List<GameState> sortedList = savedGames.Values.OrderByDescending(g => g.playerScore).ToList();
        int count = Math.Min(sortedList.Count, 3);

        Console.WriteLine("Top 3 usernames with their scores:");
        for (int i = 0; i < count; i++)
        {
            Console.WriteLine($"{sortedList[i].username}: {sortedList[i].playerScore}");
        }

        DeleteFilesNotInTop3();

        //PresentMenu();

    }
    void CreateNewUsername()
    {
        Console.WriteLine("Please enter your new username:");
        string newUsername = Console.ReadLine();

        // Check if the username already exists
        if (savedGames.ContainsKey(newUsername))
        {
            Console.WriteLine("Username already exists. Please choose another one.");
            CreateNewUsername();
            return;
        }

        // Create a new game state for the user
        savedGames.Add(newUsername, new GameState(newUsername));
        selectedUsername = newUsername;
        Console.WriteLine($"New username '{newUsername}' created.");

        //PresentMenu();

    }
    void SaveGameState()
    {
        // Save the game state for the selected username
        if (savedGames.ContainsKey(selectedUsername))
        {
            string saveFilePath = Path.Combine(saveFolderPath, $"{selectedUsername}.dat");
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream fileStream = File.Open(saveFilePath, FileMode.Create);
                formatter.Serialize(fileStream, savedGames[selectedUsername]);
                fileStream.Close();
                Console.WriteLine($"Game state saved for {selectedUsername}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to save game state for {selectedUsername}: " + e.Message);
            }
        }
        else
        {
            Console.WriteLine($"No save file found for {selectedUsername}");
        }

    }

    void LoadSavedGames()
    {
        string[] saveFiles = Directory.GetFiles(saveFolderPath, "*.dat");
        foreach (string saveFile in saveFiles)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream fileStream = File.Open(saveFile, FileMode.Open);
                GameState gameState = (GameState)formatter.Deserialize(fileStream);
                fileStream.Close();
                savedGames.Add(gameState.username, gameState);
                Console.WriteLine($"Loaded save file for {gameState.username}");
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to load save file: " + e.Message);
            }
        }
    }
    void DeleteFilesNotInTop3()
    {
        List<GameState> sortedList = savedGames.Values.OrderByDescending(g => g.playerScore).ToList();
        List<string> top3Usernames = sortedList.Take(3).Select(g => g.username).ToList();

        // Create a copy of the keys
        List<string> usernamesToDelete = savedGames.Keys.ToList();

        // Delete files not in top 3
        foreach (string username in usernamesToDelete)
        {
            if (!top3Usernames.Contains(username))
            {
                string saveFilePath = Path.Combine(saveFolderPath, $"{username}.dat");
                try
                {
                    if (File.Exists(saveFilePath))
                    {
                        File.Delete(saveFilePath);
                        savedGames.Remove(username);
                        Console.WriteLine($"Save file for {username} deleted.");
                    }
                    else
                    {
                        Console.WriteLine($"No save file exists for {username}.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to delete save file for {username}: " + e.Message);
                }
            }
        }
    }

    void IncreaseScore(string username, int pointsToAdd)
    {
        if (username != null && savedGames.ContainsKey(username))
        {
            savedGames[username].IncreaseScore(pointsToAdd);
        }
        else
        {
            Console.WriteLine("No username selected or username does not exist.");
        }
    }

}