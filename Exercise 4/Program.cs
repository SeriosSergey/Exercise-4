using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;

namespace Exercise_4
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите адрес сайта:");
            string url = Console.ReadLine();
            string answer="";
            try
            {
                WebClient wc = new WebClient();
                answer = wc.DownloadString(url);
            }
            catch (Exception e)
            {
                Console.WriteLine("Не удалось загрузить данные: " + e.Message+"\r\nПриложение будет закрыто. Нажмите клавишу Enter.");
                Console.ReadLine();
                Environment.Exit(0);
            }
            Console.WriteLine(answer);
            Regex regex = new Regex("<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(answer);
            int count = 1;
            if (matches.Count > 0)
            {
                Console.WriteLine($"Найдено {matches.Count} картинок на сайте. Их адреса:");
                foreach (Match match in matches)
                {
                    Regex regex_picture_url = new Regex("(https?:)?//?[^\'\"<>]+?\\.(jpg|jpeg|gif|png)", RegexOptions.IgnoreCase);
                    Match match_picture_url = regex_picture_url.Match(match.Value);
                    Console.WriteLine(match_picture_url.Value);
                    string target = Path.Combine(Directory.GetCurrentDirectory(), count++.ToString()+"."+ match_picture_url.Value.Substring(match_picture_url.Value.LastIndexOf(".")+1));;
                    Console.WriteLine(target);
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile(match_picture_url.Value, target);
                    }
                }
            }
            else
                Console.WriteLine("Картинок не найдено");


            Console.ReadLine();
        }
    }
}
