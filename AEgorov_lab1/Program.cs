using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AEgorov_lab1
{
    class RealEstate
    {
        public string Owner { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int ApproxCost { get; set; }
    }
    class RuralRealEstate : RealEstate
    {
        public string Street { get; set; }
        public int HouseNumber { get; set; }
    }
    class UrbanRealEstate : RuralRealEstate
    {
        public string District { get; set; }
        public string Shore { get; set; }
    }

    class Program
    {
        public static List<string> CleaningString(string data)
        {
            List<string> result = new List<string>();
            string pattern = @"""[^""]*""|\S+";

            MatchCollection matches = Regex.Matches(data, pattern);

            foreach (Match match in matches)
            {
                string value = match.Value;
                if (value.StartsWith("\"") && value.EndsWith("\""))
                {
                    value = value.Substring(1, value.Length - 2);
                }
                result.Add(value);
            }

            return result;
        }

        public static RealEstate REInfoConverter(List<string> data)
        {
            string owner = data[0];
            DateTime registrationDate = DateTime.Parse(data[1]);
            int approxCost = int.Parse(data[2]);

            RealEstate realEstate = new RealEstate
            {
                Owner = owner,
                RegistrationDate = registrationDate,
                ApproxCost = approxCost
            };

            return realEstate;
        }

        public static RuralRealEstate RuralREInfoConverter(List<string> data)
        {
            RealEstate Source = REInfoConverter(data);

            string street = data[3];
            int houseNumber = int.Parse(data[4]);

            RuralRealEstate ruralRealEstate = new RuralRealEstate
            {
                Owner = Source.Owner,
                RegistrationDate = Source.RegistrationDate,
                ApproxCost = Source.ApproxCost,
                Street = street,
                HouseNumber = houseNumber
            };

            return ruralRealEstate;
        }

        public static UrbanRealEstate UrbanREInfoConverter(List<string> data)
        {
            RuralRealEstate Source = RuralREInfoConverter(data);

            string district = data[5];
            string shore = data[6];

            UrbanRealEstate urbanRealEstate = new UrbanRealEstate
            {
                Owner = Source.Owner,
                RegistrationDate = Source.RegistrationDate,
                ApproxCost = Source.ApproxCost,
                Street = Source.Street,
                HouseNumber = Source.HouseNumber,
                District = district,
                Shore = shore
            };

            return urbanRealEstate;
        }

        public static List<string> ReadDataFromFile(string filePath)
        {
            List<string> allInfo = new List<string>();

            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        List<string> objectsInLine = CleaningString(line.Trim());
                        allInfo.AddRange(objectsInLine);
                    }
                }
            }

            return allInfo;
        }

        static void Main(string[] args)
        {
            List<string> AllInfo = new List<string>();
            List<RealEstate> REI_lst = new List<RealEstate>();
            List<RealEstate> RuralREI_lst = new List<RealEstate>();
            List<RealEstate> UrbanREI_lst = new List<RealEstate>();

            string projectDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(projectDirectory, "file.txt");
            AllInfo = ReadDataFromFile(filePath);

            if (AllInfo.Count == 0)
            {
                AllInfo = new List<string>() {
                    "\"Егоров А.Р.\"     2025.09.05 15000000",
                    "\"Емельянов В. И.\"     2025.12.17             4700000  \"Добрая\"    18",
                    "\"Трусов Н. А.\"     2010.05.01     7000000  \"Ленина\"   9  \"Октябрьский\"        \"Левый\""
                };
            }

            for (int i = 0; i < AllInfo.Count; i++)
            {
                List<string> result = CleaningString(AllInfo[i]);
                if (result.Count == 3)
                {
                    REI_lst.Add(REInfoConverter(result));
                }
                else if (result.Count == 5)
                {
                    RuralREI_lst.Add(RuralREInfoConverter(result));
                }
                else if (result.Count == 7)
                {
                    UrbanREI_lst.Add(UrbanREInfoConverter(result));
                }
            }

            Console.ReadKey();
        }
    }
}