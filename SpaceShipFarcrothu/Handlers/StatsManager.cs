using SpaceShipFartrothu.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShipFartrothu.Handlers
{
    public static class StatsManager
    {
        private static bool player1HasEnoughToNextLevel;
        private static bool player2HasEnoughToNextLevel;

        private static int player1PointsToNextLevel = 150;
        private static int player2PointsToNextLevel = 150;

        private static int player1CurrentScores;
        private static int player2CurrentScores;

        private static int currentPlayerID;

        public static void UpdatePlayersStats(List<Player> players)
        {

            for (int i = 0; i < players.Count; i++)
            {
                currentPlayerID = players[i].Id;

                if (currentPlayerID == 1)
                {
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
                        player1HasEnoughToNextLevel = false;
                    }
                }
            }
        }
    }
}
