namespace _1.Points
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    internal class Program
    {
        private static void Main()
        {
            string input = Console.ReadLine();
            string commands = Console.ReadLine();

            string pattern = @"[^0-9]+";
            Regex regex = new Regex(pattern);

            string[] points = regex.Split(input).Where(s => !string.IsNullOrEmpty(s)).ToArray();
            bool hasReversed = false;
            int xCoord = int.Parse(points[0]);
            int yCoord = int.Parse(points[1]);

            for (int i = 0; i < commands.Length; i++)
            {
                char command = commands[i];
                switch (command)
                {
                    case '>':
                        if (!hasReversed)
                        {
                            xCoord++;
                        }
                        else
                        {
                            xCoord--;
                        }

                        break;
                    case '<':
                        if (!hasReversed)
                        {
                            xCoord--;
                        }
                        else
                        {
                            xCoord++;
                        }

                        break;
                    case '^':
                        if (!hasReversed)
                        {
                            yCoord--;
                        }
                        else
                        {
                            yCoord++;
                        }

                        break;
                    case 'V':
                        if (!hasReversed)
                        {
                            yCoord++;
                        }
                        else
                        {
                            yCoord--;
                        }

                        break;
                    case '~':
                        if (!hasReversed)
                        {
                            hasReversed = true;
                        }
                        else
                        {
                            hasReversed = false;
                        }

                        break;
                }
            }
            Console.WriteLine(String.Format("({0}, {1})", xCoord, yCoord));
        }
    }
}