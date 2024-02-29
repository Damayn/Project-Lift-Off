using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using GXPEngine;

public class ScoreManager : GameObject
{

    public ScoreManager() : base ()
    {
    }

    public void SaveScore(string name, int score)
    {
        try
        {
            List<string> existingScores = LoadScores();

            // Check if score with the same name exists
            bool scoreExists = false;
            for (int i = 0; i < existingScores.Count; i++)
            {
                string[] parts = existingScores[i].Split(':');
                if (parts.Length == 2 && parts[0].Trim() == name)
                {
                    scoreExists = true;
                    // Update score if the new one is higher
                    int existingScore = int.Parse(parts[1]);
                    if (score > existingScore)
                    {
                        existingScores[i] = $"{name}:{score}";
                    }
                    break;
                }
            }

            // If score doesn't exist or new score is higher, add it
            if (!scoreExists)
            {
                existingScores.Add($"{name}:{score}");
            }

            // Save the scores back to the file
            SaveScores(existingScores);

            Console.WriteLine($"Score saved: {name}:{score}");
        }
        catch (IOException e)
        {
            Console.WriteLine($"Failed to save score: {e.Message}");
        }
    }

    public List<string> GetTopScores()
    {
        List<string> topScores = new List<string>();

        try
        {
            List<string> scores = LoadScores();

            // Sort the scores by score value (descending order)
            scores.Sort((a, b) =>
            {
                string[] partsA = a.Split(':');
                string[] partsB = b.Split(':');
                int scoreA = int.Parse(partsA[1]);
                int scoreB = int.Parse(partsB[1]);
                return scoreB.CompareTo(scoreA); // Compare in descending order
            });

            // Limit to the top 5 scores
            if (scores.Count > 5)
            {
                topScores = scores.GetRange(0, 5);
            }
            else
            {
                topScores = scores;
            }

            return topScores;
        }
        catch (IOException e)
        {
            Console.WriteLine($"Failed to load scores: {e.Message}");
            return topScores; // Return empty list if an error occurs
        }
    }

    private List<string> LoadScores()
    {
        List<string> scores = new List<string>();

        try
        {
            // Read all scores from the file
            using (StreamReader reader = new StreamReader("Scores.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    scores.Add(line);
                }
            }

            // Sort the scores by score value (descending order)
            scores.Sort((a, b) =>
            {
                string[] partsA = a.Split(':');
                string[] partsB = b.Split(':');
                int scoreA = int.Parse(partsA[1]);
                int scoreB = int.Parse(partsB[1]);
                return scoreB.CompareTo(scoreA); // Compare in descending order
            });

            // Limit to the top 5 scores
            if (scores.Count > 5)
            {
                scores = scores.GetRange(0, 5);
            }

            // Overwrite the Scores.txt file with the top 5 scores
            SaveScores(scores);

            Console.WriteLine("Top 5 scores saved to Scores.txt");

            // Return the list of scores
            return scores;
        }
        catch (IOException e)
        {
            Console.WriteLine($"Failed to load or save scores: {e.Message}");
            return scores; // Return empty list if an error occurs
        }
    }

    private void SaveScores(List<string> scores)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter("Scores.txt", false))
            {
                foreach (string score in scores)
                {
                    writer.WriteLine(score);
                }
            }
        }
        catch (IOException e)
        {
            Console.WriteLine($"Failed to save scores: {e.Message}");
        }
    }
}
