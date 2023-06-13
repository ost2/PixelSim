using System;
namespace MovementTest
{
    public class Program
    {
        public static void Main(string[]args)
        {
            Application testGame = new Application(false);

            var rand = new Random();
            var enemySymbol = '*';
            var runnerSymbol = '∆';
            var followerSymbol = 'Ω';
            List<char> zombieSymbols = new List<char>(7);
            zombieSymbols.Add('˚');
            zombieSymbols.Add('.');
            zombieSymbols.Add('°');
            zombieSymbols.Add('•');
            zombieSymbols.Add('•');
            zombieSymbols.Add('˙');
            zombieSymbols.Add('*');

            GameObject enemy = new GameObject(testGame, enemySymbol, 1, true, true, true);

            GameObject runner = new GameObject(testGame, runnerSymbol, 1, true, false, false, true);

            GameObject follower = new GameObject(testGame, followerSymbol, 1, true, false, false, false, true);

            GameObject zombie = new GameObject(testGame, zombieSymbols[testGame.Rand.Next(0, zombieSymbols.Count)], 1, true, false, false, false, true, true);

            void zombieApocalypse(int zN, int rN)
            {
                for (int i = 0; i < zN; i++)
                {
                    testGame.addGameObject(new GameObject(zombie, testGame.ScreenHeight/2, testGame.ScreenWidth/2, zombieSymbols));
                }
                for (int i = 0; i < rN; i++)
                {
                    testGame.addGameObject(new GameObject(runner, rand.Next(1, testGame.ScreenHeight), rand.Next(1, testGame.ScreenWidth)));
                }
            }

            void runnerChaser(int rN, int cN)
            {
                for (int i = 0; i < rN; i++)
                {
                    testGame.addGameObject(new GameObject(runner, testGame.ScreenHeight / 2, testGame.ScreenWidth / 2));
                }
                for (int i = 0; i < cN; i++)
                {
                    testGame.addGameObject(new GameObject(follower, rand.Next(1, testGame.ScreenHeight), rand.Next(1, testGame.ScreenWidth)));
                }
            }


            //zombieApocalypse(3, 28);

            runnerChaser(20, 20);

            testGame.runGame();
        }
    }
}


