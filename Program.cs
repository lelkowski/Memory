using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;



namespace Memory
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.SetWindowSize(76, 30);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Dictionary<string, Slot> slots = new();


            string[] allwords = File.ReadAllLines("words.txt");
            Random rand = new();

            List<string> words = new();

            while (true)
            {
                Console.Clear();
                AsciiArt();
                Console.WriteLine("\n\t\t\tWelcome to the game of Memory");
                Console.WriteLine("\t\tWant to play the easy version or the hard version?");
                int howMany = 0;
                int lifes = 0;
                while (howMany == 0)
                {

                    Console.Write("\t\t\t\t    ");
                    string option = Console.ReadLine().ToLower();
                    Console.WriteLine(option);
                    switch (option)
                    {
                        case "easy":
                            howMany = 1;
                            lifes = 10;
                            break;
                        case "hard":
                            howMany = 2;
                            lifes = 15;
                            break;
                        default:
                            Console.WriteLine("\t\tSomething went wrong, try writing ''easy'' or ''hard''.");
                            break;

                    }

                }

                Stopwatch howLong = new();
                howLong.Start();
                while (words.Count < 8 * howMany)
                {
                    string word = allwords[rand.Next(allwords.Length)];
                    if (!words.Contains(word)) { words.Add(word); words.Add(word); }
                }


                for (int i = 0; i < 2 * howMany; i++)
                {
                    for (int j = 1; j < 5; j++)
                    {
                        char row = (char)('a' + i);
                        int x = rand.Next(words.Count);
                        slots.Add(row.ToString() + j, new Slot(words[x]));
                        words.RemoveAt(x);
                    }
                }
                int toBeat = howMany * 4;
                int chances = 2;
                int count = 1;
                string previous = "";
                string chosenSlot = "";
                bool playing = true;
                bool result = false;
                while (playing)
                {
                    Console.Clear();
                    Console.WriteLine("\n\t\t1\t\t2\t\t3\t\t4");
                    Console.Write("\t|---------------|---------------|---------------|---------------|\n");
                    for (int j = 0; j < 2 * howMany; j++)
                    {
                        char literka = (char)('a' + j);
                        string first = "" + literka + 1, second = "" + literka + 2, third = "" + literka + 3, fourth = "" + literka + 4;
                        Console.Write("   " + literka + "\t|" + slots[first].Status() + "|" + slots[second].Status() + "|" + slots[third].Status() + "|" + slots[fourth].Status() + "|\n");
                        Console.Write("\t|---------------|---------------|---------------|---------------|\n");
                    }
                    Console.WriteLine("\n      Choose slot by writing it's coordinates, for example: 'a1' or 'b3'.");
                    switch (chances)
                    {
                        case 0:
                            {
                                count++;
                                lifes--;
                                if (lifes == 0)
                                {
                                    playing = false;
                                    Console.Clear();
                                }
                                chances = 2;
                            }
                            break;
                        case 1:

                            Console.Write("\t\t\t\t    ");
                            chosenSlot = Console.ReadLine().ToLower();
                            if (!slots.ContainsKey(chosenSlot))
                            {
                                Console.WriteLine("\tChoose slot by writing its coordinates, for example: 'a1' or 'b3'");
                                System.Threading.Thread.Sleep(1000);
                                continue;
                            }
                            switch (slots[chosenSlot].IsOpen())
                            {
                                case true:
                                    Console.WriteLine("\t\tThis slot is already visible, choose another.");
                                    System.Threading.Thread.Sleep(2000);
                                    break;
                                case false:
                                    if (slots[chosenSlot].word == slots[previous].word)
                                    {
                                        toBeat--;
                                        slots[chosenSlot].Change();
                                        chances = 2;
                                        if (toBeat == 0)
                                        {
                                            playing = false;
                                            result = true;
                                        }
                                    }
                                    else
                                    {
                                        slots[chosenSlot].Change();

                                        Console.Clear();
                                        Console.WriteLine("\n\t\t1\t\t2\t\t3\t\t4");
                                        Console.Write("\t|---------------|---------------|---------------|---------------|\n");
                                        for (int j = 0; j < 2 * howMany; j++)
                                        {
                                            char literka = (char)('a' + j);
                                            string first = "" + literka + 1, second = "" + literka + 2, third = "" + literka + 3, fourth = "" + literka + 4;
                                            Console.Write("   " + literka + "\t|" + slots[first].Status() + "|" + slots[second].Status() + "|" + slots[third].Status() + "|" + slots[fourth].Status() + "|\n");
                                            Console.Write("\t|---------------|---------------|---------------|---------------|\n");
                                        }

                                        Console.WriteLine("\n\t\t  Sorry, you missed. You have " + (lifes - 1) + " more chances.");
                                        System.Threading.Thread.Sleep(2000);

                                        slots[chosenSlot].Change();
                                        slots[previous].Change();
                                        chances--;
                                    }
                                    break;
                            }
                            break;
                        case 2:

                            Console.Write("\t\t\t\t    ");
                            chosenSlot = Console.ReadLine().ToLower();
                            if (!slots.ContainsKey(chosenSlot))
                            {
                                Console.WriteLine("\tChoose slot by writing its coordinates, for example: 'a1' or 'b3'");
                                System.Threading.Thread.Sleep(1000);
                                continue;
                            }
                            switch (slots[chosenSlot].IsOpen())
                            {
                                case true:
                                    Console.WriteLine("\t\tThis slot is already visible, choose another");
                                    System.Threading.Thread.Sleep(2000);
                                    break;
                                case false:
                                    slots[chosenSlot].Change();
                                    previous = chosenSlot;
                                    chances--;
                                    break;
                            }
                            break;
                    }


                }
                howLong.Stop();
                if (result) //win
                {
                    Console.Clear();
                    Console.WriteLine("\n\t\t1\t\t2\t\t3\t\t4");
                    Console.Write("\t|---------------|---------------|---------------|---------------|\n");
                    for (int j = 0; j < 2 * howMany; j++)
                    {
                        char literka = (char)('a' + j);
                        string first = "" + literka + 1, second = "" + literka + 2, third = "" + literka + 3, fourth = "" + literka + 4;
                        Console.Write("   " + literka + "\t|" + slots[first].Status() + "|" + slots[second].Status() + "|" + slots[third].Status() + "|" + slots[fourth].Status() + "|\n");
                        Console.Write("\t|---------------|---------------|---------------|---------------|\n");
                    }

                    slots.Clear();
                    DateTime time = DateTime.Now;
                    Console.WriteLine("\t\tCongrats, you won! It took you {0:hh\\:mm\\:ss} and " + count + " tries.", howLong.Elapsed);
                    Console.Write("\t\t\t\tWhat is your name?\n\t\t\t");
                    
                    string name = Console.ReadLine();
                    StreamWriter highscore = File.AppendText("highscores.txt");
                    if(howMany==1)
                    highscore.WriteLine(name + " | Easy | " + time + " | {0:hh\\:mm\\:ss} | " + count, howLong.Elapsed);
                    else
                    highscore.WriteLine(name + " | Hard |" + time + " | {0:hh\\:mm\\:ss} | " + count, howLong.Elapsed);
                    highscore.Close();


                    Console.WriteLine("\n\t\t\t\tHighscores:");
                    string[] highscores = File.ReadAllLines("highscores.txt");
                    int length;
                    if (highscores.Length < 10)
                        length = highscores.Length;
                    else
                        length = 10;
                        string[] bestTen = new string[length];

                    //highscore
                    string help;
                    for (int i = 0; i < highscores.Length; i++)
                    {
                        string next = highscores[i];
                        for (int j = 0; j < bestTen.Length; j++)
                        {
                            if (bestTen[j] == null)
                            {
                                bestTen[j] = next;
                                break;
                            }
                            else
                            {
                                int newScore = int.Parse(next.Substring(next.LastIndexOf(" | ") + 2));
                                int oldScore = int.Parse(bestTen[j].Substring(bestTen[j].LastIndexOf(" | ") + 2));
                                if (newScore < oldScore)
                                {
                                    help = bestTen[j];
                                    bestTen[j] = next;
                                    next = help;
                                }
                            }


                        }

                    }
                    for (int i = 0; i < bestTen.Length; i++)
                    {
                        if (bestTen[i] == null) break;
                        Console.WriteLine("\t{0}" + ". " + bestTen[i], i + 1);
                    }
                    File.WriteAllLines("highscores.txt", bestTen);


                    Console.WriteLine("\n\tHow about one more game? Answer that with 'yes' or 'no'.");

                    bool another = true;
                    while (another)
                    {
                        Console.Write("\t\t\t\t    ");
                        string next = Console.ReadLine();
                        switch (next)
                        {
                            case "yes":
                                another = false;
                                break;
                            case "no":
                                return;
                            default:
                                Console.WriteLine("\n\t\t\tAnswer with 'yes' or 'no'.");
                                break;
                        }

                    }


                }
                else //lose
                {

                    string[] highscores = File.ReadAllLines("highscores.txt");
                    Console.WriteLine("\n\n\t\t\t\tHighscores");

                    Console.WriteLine("\t\tName | Mode | Date | Guessing time | Tries");
                    for (int i = 0; i < highscores.Length; i++)
                    {
                        if (highscores[i] == null) break;
                        Console.WriteLine("\t{0}" + ". " + highscores[i], i + 1);
                    }
                    Console.WriteLine("\n\t\tUnfortunately, you didn't make it this time. \n\t\t\tDo you want to try again?");
                    bool another = true;
                    while (another)
                    {
                        Console.Write("\t\t\t\t    ");
                        string next = Console.ReadLine();
                        switch (next)
                        {
                            case "yes":
                                another = false;
                                break;
                            case "no":
                                return;
                            default:
                                Console.WriteLine("\n\t\t\tAnswer with 'yes' or 'no'.");
                                break;
                        }

                    }
                }


            }
        }

        public static void AsciiArt()
        {

            Console.WriteLine("\t\t ___ ___    ___  ___ ___   ___   ____   __ __");
            Console.WriteLine("\t\t|   |   |  /  _]|   |   | /   " + (char)92 + " |    " + (char)92 + " |  |  |");
            Console.WriteLine("\t\t| _   _ | /  [_ | _   _ ||     ||  D  )|  |  |");
            Console.WriteLine("\t\t|  " + (char)92 + "_/  ||    _]|  " + (char)92 + "_/  ||  O  ||    / |  ~  |");
            Console.WriteLine("\t\t|   |   ||   [_ |   |   ||     ||    " + (char)92 + " |___, |");
            Console.WriteLine("\t\t|   |   ||     ||   |   ||     ||  .  " + (char)92 + "|     |");
            Console.WriteLine("\t\t|___|___||_____||___|___| " + (char)92 + "___/ |__|" + (char)92 + "_||____/");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("\t\t          ____   ____  ___ ___    ___ ");
            Console.WriteLine("\t\t         /    | /    ||   |   |  /  _]");
            Console.WriteLine("\t\t        |   __||  o  || _   _ | /  [_ ");
            Console.WriteLine("\t\t        |  |  ||     ||  " + (char)92+"_/  ||    _]");
            Console.WriteLine("\t\t        |  |_ ||  _  ||   |   ||   [_ ");
            Console.WriteLine("\t\t        |     ||  |  ||   |   ||     |");
            Console.WriteLine("\t\t        |___,_||__|__||___|___||_____|");
        }
    }



    class Slot
    {
        public string word;
        bool open=false;

        public Slot(string next)
        {
            word=next;
        }

        public string Status()
        {
            if (open == false)
            {
                return "XXXXXXXXXXXXXXX";
            }

            return word.Length switch
            {
                < 4 => ("      " + word + "\t"),
                < 6 => ("     " + word + "\t"),
                < 9 => ("    " + word + "\t"),
                < 11 => ("   " + word + "\t"),
                _ => ("" + word + "\t"),
            };
        }

        public void Change()
        {
            open = !open;
        }

        public bool IsOpen()
        {
            return open;
        }

        
    }



}
