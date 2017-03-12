using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numcs
{
    class Program
    {
        static int Main(string[] args)
        {
            int args_count = 0, position = 0;
            bool verbosemode = false;
            string R2Ergebnis = String.Empty;

            // Input checking
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: numc input-number [input number system] target-number-system [-v]");
                return -1;
            }
            if (args[0].Length > 18  || args[0][0] == '-')
            {
                Console.WriteLine("Only positive numbers of up to 18 digits are supported currently.");
                return -1;
            }
            if (Int64.Parse(args[1]) < 2 || Int64.Parse(args[1]) > 36)
            {
                Console.WriteLine("Only number systems on base 2 - 36 are supported currently.");
                return -1;
            }
            if (args.Length == 2 || args.Length > 1 && args[2] == "-v")
            {
                for (position = 0; position != args[0].Length; position++)
                {
                    if (args[0][position] - 48 >= 10)
                    {
                        Console.WriteLine("{0} is not a valid number in the decimal number system.", args[0]);
                        return -1;
                    }
                }
            }
            if (args.Length > 2 && args[2] != "-v")
            {
                if (Int64.Parse(args[2]) < 2 || Int64.Parse(args[2]) > 36)
                {
                    Console.WriteLine("Only number systems on base 2 - 36 are supported currently.");
                    return -1;
                }
                for (position = 0; position != args[0].Length; position++)
                {
                    if (args[0][position] > 57)
                    {
                        if (args[0][position] - 55 >= Int64.Parse(args[1]))
                        {
                            Console.WriteLine("{0} is not a valid number in the number system specified.", args[0]);
                            return -1;
                        }
                    }
                    else if (args[0][position] - 48 >= Int64.Parse(args[1]))
                    {
                        Console.WriteLine("{0} is not a valid number in the number system specified.", args[0]);
                        return -1;
                    }
                }
            }
            // Check for verbose mode
            if (args.Length > 2) for (args_count = 1; args_count < args.Length; args_count++) if (args[args_count] == "-v") verbosemode = true;
            // Hauptprogramm
            long dezimalzahl = 0;
            if (verbosemode == true)
            {
                for (args_count = 0; args_count < args.Length; args_count++) Console.Write("\nargs[{0}]: {1}", args_count, args[args_count]);
            }
            if (verbosemode == true) Console.WriteLine();

            if (args.Length > 2 && args[2] != "-v" && args[1] != "10")
            {
                // Berechnung andere Zahlensysteme zu Dezimal (Methode 1)
                dezimalzahl = Routine1(args, verbosemode);
                // Ausgabe 
                if (verbosemode == true) Console.Write("{0}({1}) in decimal is: ", args[0], args[1]);
                if (args[2] == "10") Console.Write(dezimalzahl);
                else if (verbosemode == true) Console.WriteLine(dezimalzahl);
            }

            if (args.Length == 2 || args.Length > 2 && args[2] != "10")
            {
                // Berechnung Dezimal zu anderen Zahlensystemen (Methode 2)
                if (args.Length > 2 && args[2] != "-v" && args[1] != "10") R2Ergebnis = Routine2(args, dezimalzahl, verbosemode);
                else R2Ergebnis = Routine2(args, Int64.Parse(args[0]), verbosemode);
            }
            // Ausgabe
            if (verbosemode == true)
            {
                Console.WriteLine();
                Console.Write("\n{0}({1}) on base {2} is: ", args[0], args[1], args[2]);
            }
            Console.Write(R2Ergebnis);
            // END OF MAIN
            return 0;
        }

        // Routine 1 (andere zu dezimal)
        private static long Routine1(string[] args, bool verbosemode)
        {
            long endzahl2 = 0;
            long divergebnis = 1;
            for (int position = 0; position != args[0].Length; position++)
            {
                divergebnis = args[0][position] - '0';
                if (divergebnis > 9) divergebnis -= 7; // Buchstaben werden in ihre Zahlenwerte konvertiert
                endzahl2 += divergebnis * (long)Math.Pow(Int64.Parse(args[1]), args[0].Length - position - 1);
                if (verbosemode == true)
                {
                    Console.WriteLine();
                    Console.Write("{0} ^ {1}: {2}", args[1], args[0].Length - position - 1, (long)Math.Pow(Int64.Parse(args[1]), args[0].Length - position - 1));
                    Console.Write("\nPosition: {0} --- Value: {1}", position, divergebnis);
                    Console.WriteLine("\n>> Result so far: {0}", endzahl2);
                }
            }
            return endzahl2;
        }

        // Routine 2 (dezimal zu andere)
        private static string Routine2(string[] args, long dezimalzahl, bool verbosemode)
        {
            string endzahl = String.Empty;
            for (int i = 0; dezimalzahl != 0; i++)
            {
                if (args.Length > 2 && args[2] != "-v")
                {
                    if (dezimalzahl % Int64.Parse(args[2]) > 9) endzahl += (char)(dezimalzahl % Int64.Parse(args[2]) + 55);
                    else endzahl += (char)(dezimalzahl % Int64.Parse(args[2]) + 48);
                }
                else
                {
                    if (dezimalzahl % Int64.Parse(args[1]) > 9) endzahl += (char)(dezimalzahl % Int64.Parse(args[1]) + 55);
                    else endzahl += (char)(dezimalzahl % Int64.Parse(args[1]) + 48);
                }
                if (verbosemode == true)
                {
                    Console.Write("\ni: {0} ----------- calculation:\t{1}", i, dezimalzahl % Int64.Parse(args[1]));
                    Console.Write("\tremainder:\t{0}\tquotient:\t{1}", endzahl[i], dezimalzahl);
                }
                if (args.Length > 2 && args[2] != "-v") dezimalzahl = dezimalzahl / Int64.Parse(args[2]);
                else dezimalzahl = dezimalzahl / Int64.Parse(args[1]);
            }
            // Reverse generated string before return
            char[] charArray = endzahl.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}