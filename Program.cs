using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snek
{
    internal class Program
    {
        class Coordinate
        {
            public int x;
            public int y;

            public Coordinate(int xPosition, int yPosition)
            {
                x = xPosition;
                y = yPosition;
            }
        }
        class Player
        {
            public Coordinate position;
            public Coordinate previousPosition;
            public int score = 0;
            public string direction = "Right";

            public Player()
            {
                position = new Coordinate(2, 1);
            }
            public Player(int x, int y)
            {
                position = new Coordinate(x, y);
            }
        }
        class Tail
        {
            public Coordinate position;
            public Tail(int x, int y)
            {
                position = new Coordinate(x, y);
            }
        }
        class Block
        {
            public Coordinate position;

            public Block()
            {
                position = new Coordinate(1, 1);
            }
            public Block(int x, int y)
            {
                position = new Coordinate(x, y);
            }
        }
        class Pill
        {
            public Coordinate position;

            public Pill()
            {
                position = new Coordinate(1, 1);
            }
            public Pill(int x, int y)
            {
                position = new Coordinate(x, y);
            }
            public void newPosition(int width, int height)
            {
                Random rng = new Random();
                position = new Coordinate(rng.Next(1, height - 1), rng.Next(1, width - 1));
            }
        }
        static void PlayerMovement(Player player1, string[][] map)
        {
            if (player1.direction == "Up" && map[player1.position.x][player1.position.y - 1] != "#")
            {
                player1.previousPosition = player1.position;
                player1.position.y--;
            }
            if (player1.direction == "Down" && map[player1.position.x][player1.position.y + 1] != "#")
            {
                player1.previousPosition = player1.position;
                player1.position.y++;
            }
            if (player1.direction == "Left" && map[player1.position.x - 1][player1.position.y] != "#")
            {
                player1.previousPosition = player1.position;
                player1.position.x--;
            }
            if (player1.direction == "Right" && map[player1.position.x + 1][player1.position.y] != "#")
            {
                player1.previousPosition = player1.position;
                player1.position.x++;
            }
        }
        static string[][] CreateMap(int height, int width, Player player, List<Block> blocks, Pill pill)
        {
            //Skapa en tom jagged array
            string[][] mapCoordinates = new string[width][];



            for (int i = 0; i < height; i++)
            {
                mapCoordinates[i] = new string[width];
            }

            //Fyll alla celler med -, förutom kanterna som istället fylls med #
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    mapCoordinates[i][j] = "-";

                    if (i == 0 || i == height - 1 || j == 0 || j == width - 1)
                    {
                        mapCoordinates[i][j] = "#";
                    }

                    if (i == player.position.x && j == player.position.y)
                    {
                        mapCoordinates[i][j] = "@";
                    }

                    for (int k = 0; k < blocks.Count; k++)
                    {
                        if (i == blocks[k].position.x && j == blocks[k].position.y)
                        {
                            mapCoordinates[blocks[k].position.x][blocks[k].position.y] = "#";
                        }
                    }

                    if (i == pill.position.x && j == pill.position.y)
                    {
                        while (mapCoordinates[i][j] == "#")
                        {
                            pill.newPosition(width, height);
                        }
                        if (mapCoordinates[i][j] == "@")
                        {
                            player.score++;
                            pill.newPosition(width, height);
                        }
                        mapCoordinates[pill.position.x][pill.position.y] = "$";
                    }
                }
            }
            Console.WriteLine("Score: " + player.score);
            return mapCoordinates;
        }
        static void DrawMap(int height, int width, string[][] map)
        {
            //Skriv ut koordinatsystemet i kommandoprompten
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Console.Write(map[i][j] + " ");
                }
                Console.WriteLine();
            }
        }
        static void ScreenUpdate(int height, int width, Player player1, List<Block> blocks, Pill pill)
        {
            Console.Clear();
            string[][] map = CreateMap(height, width, player1, blocks, pill);
            PlayerMovement(player1, map);
            DrawMap(height, width, map);
        }
        static void Main(string[] args)
        {
            //Bredd måste vara större än höjd
            ConsoleKeyInfo cki;
            Random rng = new Random();
            int speed = 50;
            int height = 10;
            int width = 20;
            int blocksCount = 10;
            List<Block> blocks = new List<Block>();

            for (int i = 0; i < blocksCount; ++i)
            {
                blocks.Add(new Block(rng.Next(1, height - 1), rng.Next(1, width - 1)));
            }

            Player player1 = new Player();

            Pill pill = new Pill(rng.Next(1, height - 1), rng.Next(1, width - 1));
            while (true)
            {

                cki = Console.ReadKey();
                while (Console.KeyAvailable == false)
                {
                    if (cki.Key == ConsoleKey.A)
                        player1.direction = "Up";
                    if (cki.Key == ConsoleKey.D)
                        player1.direction = "Down";
                    if (cki.Key == ConsoleKey.W)
                        player1.direction = "Left";
                    if (cki.Key == ConsoleKey.S)
                        player1.direction = "Right";

                    ScreenUpdate(height, width, player1, blocks, pill);
                    Thread.Sleep(speed);
                }
            }
        }
    }
}
