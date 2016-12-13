namespace SpaceShipFartrothu.Handlers
{
    using System.Collections.Generic;
    using Interfaces;

    public class StatsManager
    {
        private bool player1HasEnoughToNextLevel;
        private bool player2HasEnoughToNextLevel;

        private int player1PointsToNextLevel = 150;
        private int player2PointsToNextLevel = 150;

        private int player1CurrentScores;
        private int player2CurrentScores;

        private bool isPlayer1Loaded = true;
        private bool isPlayer2Loaded = true;

        private int currentPlayerID;

        public void UpdatePlayersStats(IList<IPlayer> players)
        {

            for (int i = 0; i < players.Count; i++)
            {
                currentPlayerID = players[i].Id;

                if (currentPlayerID == 1)
                {
                    if (isPlayer1Loaded && players[i].Score > player1PointsToNextLevel)
                    {
                        
                        int loadLevelPoint = 0;

                        int multiplier = players[i].Level;

                        for (int j = 0; j < multiplier; j++)
                        {
                            player1PointsToNextLevel += loadLevelPoint;
                            loadLevelPoint = player1PointsToNextLevel;
                        }

                        isPlayer1Loaded = false;
                    }

                    player1CurrentScores = players[i].Score;
                    if (player1CurrentScores >= player1PointsToNextLevel)
                    {
                        player1HasEnoughToNextLevel = true;
                    }
                    if (player1HasEnoughToNextLevel)
                    {
                        players[i].Level++;

                        player1PointsToNextLevel += player1PointsToNextLevel;
                        players[i].MaxHealth = 100 * players[i].Level;
                        if (players[i].Level % 5 == 0)
                        {
                            players[i].Health = players[i].MaxHealth;
                        }
                        players[i].BulletDamage++;
                        player1HasEnoughToNextLevel = false;
                    }
                }
                else if (currentPlayerID == 2)
                {
                    if (isPlayer2Loaded && players[i].Score > player2PointsToNextLevel)
                    {
                        int loadLevelPoint = 0;

                        int multiplier = players[i].Level;

                        for (int j = 0; j < multiplier; j++)
                        {
                            player2PointsToNextLevel += loadLevelPoint;
                            loadLevelPoint = player2PointsToNextLevel;
                        }

                        isPlayer2Loaded = false;
                    }

                    player2CurrentScores = players[i].Score;
                    if (player2CurrentScores >= player2PointsToNextLevel)
                    {
                        player2HasEnoughToNextLevel = true;
                    }
                    if (player2HasEnoughToNextLevel)
                    {
                        players[i].Level++;

                        player2PointsToNextLevel += player2PointsToNextLevel;
                        players[i].MaxHealth = 100 * players[i].Level;
                        if (players[i].Level % 5 == 0)
                        {
                            players[i].Health = players[i].MaxHealth;
                        }
                        players[i].BulletDamage++;
                        player2HasEnoughToNextLevel = false;
                    }
                }
            }
        }
    }
}
