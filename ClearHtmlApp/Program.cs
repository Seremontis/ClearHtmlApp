using System;

namespace ClearHtmlApp
{
    class Program
    {
        private static int select = 0;
        private static string[] answer = { "Odnośniki do strony", "Linki do obrazków ze strony" };
        static string uri;
        static void Main(string[] args)
        {
            Console.WriteLine("Witaj. Podaj stronę HTML do przefiltrowania");
            uri=Console.ReadLine();
            ChooseOption();    
        }

        private static void ChooseOption()
        {           
            SetOption();
            ClickHandler(Console.ReadKey());
        }

        private static void SetOption()
        {
            Console.Clear();
            Console.WriteLine("Witaj. Podaj stronę HTML do przefiltrowania");
            Console.WriteLine(uri);
            Console.WriteLine("Wybierz opcję:");

            for (int i = 0; i < answer.Length; i++)
            {
                if (i == select)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine(i + ") " + answer[i]);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.WriteLine(i + ") " + answer[i]);
                }
            }
        }

        private static void ClickHandler(ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    --select;
                    CkeckOverflow();
                    ChooseOption();
                    break;
                case ConsoleKey.DownArrow:
                    ++select;
                    CkeckOverflow();
                    ChooseOption();
                    break;
                case ConsoleKey.Enter:
                    SearchInPage();
                    break;
                default:
                    break;
            }           
        }

        private static void CkeckOverflow()
        {
            if (select < 0)
            {
                select = answer.Length - 1;
            }
            else if (select > answer.Length - 1)
            {
                select = 0;
            }
        }

        private static void SearchInPage()
        {
            HtmlInfo html = new HtmlInfo(select,uri);
            foreach (var item in html.ResultOfPage())
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }      
    }
}
