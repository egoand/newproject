using AEgorov_lab1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using System.Xml;

namespace AEgorov_lab1.Tests
{
    [TestClass]
    public class RealEstateTests
    {
        [TestMethod]
        public void TestCleaningString_ValidData()
        {
            // Arrange
            string input = "\"Егоров А.Р.\" 2025.09.05 15000000";

            // Act
            List<string> result = Program.CleaningString(input);

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Егоров А.Р.", result[0]);
            Assert.AreEqual("2025.09.05", result[1]);
            Assert.AreEqual("15000000", result[2]);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestCleaningString_EmptyString()
        {
            // Act
            Program.CleaningString("");
        }

        [TestMethod]
        public void TestREInfoConverter_ValidData()
        {
            // Arrange
            List<string> data = new List<string> { "Егоров А.Р.", "2020.09.05", "15000000" };

            // Act
            RealEstate result = Program.REInfoConverter(data);

            // Assert
            Assert.AreEqual("Егоров А.Р.", result.Owner);
            Assert.AreEqual(new DateTime(2020, 9, 5), result.RegistrationDate);
            Assert.AreEqual(15000000, result.ApproxCost);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestREInfoConverter_InvalidDataCount()
        {
            // Arrange
            List<string> data = new List<string> { "Егоров А.Р.", "2020.09.05" };

            // Act
            Program.REInfoConverter(data);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestREInfoConverter_InvalidDate()
        {
            // Arrange
            List<string> data = new List<string> { "Егоров А.Р.", "неправильная_дата", "15000000" };

            // Act
            Program.REInfoConverter(data);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestREInfoConverter_InvalidCost()
        {
            // Arrange
            List<string> data = new List<string> { "Егоров А.Р.", "2020.09.05", "не число" };

            // Act
            Program.REInfoConverter(data);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestREInfoConverter_FutureDate()
        {
            // Arrange
            List<string> data = new List<string> { "Егоров А.Р.", "2030.09.05", "16000000" };

            // Act
            Program.REInfoConverter(data);
        }

        [TestMethod]
        public void TestRuralREInfoConverter_ValidData()
        {
            // Arrange
            List<string> data = new List<string> { "Емельянов В.И.", "2015.12.17", "4700000", "Добрая", "18" };

            // Act
            RuralRealEstate result = Program.RuralREInfoConverter(data);

            // Assert
            Assert.AreEqual("Емельянов В.И.", result.Owner);
            Assert.AreEqual("Добрая", result.Street);
            Assert.AreEqual(18, result.HouseNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestRuralREInfoConverter_InvalidHouseNumber()
        {
            // Arrange
            List<string> data = new List<string> { "Емельянов В.И.", "2015.12.17", "4700000", "Добрая", "не число" };

            // Act
            Program.RuralREInfoConverter(data);
        }

        [TestMethod]
        public void TestUrbanREInfoConverter_ValidData()
        {
            // Arrange
            List<string> data = new List<string> { "Трусов Н.А.", "2010.05.01", "7000000", "Ленина", "9", "Октябрьский", "Левый" };

            // Act
            UrbanRealEstate result = Program.UrbanREInfoConverter(data);

            // Assert
            Assert.AreEqual("Трусов Н.А.", result.Owner);
            Assert.AreEqual("Ленина", result.Street);
            Assert.AreEqual(9, result.HouseNumber);
            Assert.AreEqual("Октябрьский", result.District);
            Assert.AreEqual("Левый", result.Shore);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestUrbanREInfoConverter_InvalidShore()
        {
            // Arrange
            List<string> data = new List<string> { "Трусов Н.А.", "2010.05.01", "7000000", "Ленина", "9", "Октябрьский", "Неизвестный" };

            // Act
            Program.UrbanREInfoConverter(data);
        }

        [TestMethod]
        public void TestErrorHandler_ValidateOwner()
        {
            // Valid cases
            Errors.ValidateOwner("Егоров А.Р.");

            // Invalid cases
            Assert.ThrowsException<ArgumentException>(() => Errors.ValidateOwner(""));
            Assert.ThrowsException<ArgumentException>(() => Errors.ValidateOwner("А"));
        }

        [TestMethod]
        public void TestErrorHandler_ValidateCost()
        {
            // Valid cases
            Errors.ValidateCost(1000000);

            // Invalid cases
            Assert.ThrowsException<ArgumentException>(() => Errors.ValidateCost(0));
            Assert.ThrowsException<ArgumentException>(() => Errors.ValidateCost(-1000));
            Assert.ThrowsException<ArgumentException>(() => Errors.ValidateCost(2000000000));
        }

        [TestMethod]
        public void TestIntegration_CompleteWorkflow()
        {
            // Arrange
            List<string> testData = new List<string> {
                "\"Егоров А.Р.\" 2020.09.05 15000000",
                "\"Емельянов В. И.\" 2015.12.17 4700000 \"Добрая\" 18",
                "\"Трусов Н. А.\" 2010.05.01 7000000 \"Ленина\" 9 \"Октябрьский\" \"Левый\""
            };

            List<RealEstate> basicList = new List<RealEstate>();
            List<RealEstate> ruralList = new List<RealEstate>();
            List<RealEstate> urbanList = new List<RealEstate>();

            // Act
            foreach (string data in testData)
            {
                List<string> cleaned = Program.CleaningString(data);

                if (cleaned.Count == 3)
                {
                    basicList.Add(Program.REInfoConverter(cleaned));
                }
                else if (cleaned.Count == 5)
                {
                    ruralList.Add(Program.RuralREInfoConverter(cleaned));
                }
                else if (cleaned.Count == 7)
                {
                    urbanList.Add(Program.UrbanREInfoConverter(cleaned));
                }
            }

            // Assert
            Assert.AreEqual(1, basicList.Count);
            Assert.AreEqual(1, ruralList.Count);
            Assert.AreEqual(1, urbanList.Count);
        }
    }
}
