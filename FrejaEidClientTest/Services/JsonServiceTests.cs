using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Services.Tests
{
    class TestData
    {
        public TestData(string name, int age, long birthDate, List<Address> addresses, int[] array, DateTime date)
        {
            Name = name;
            Age = age;
            BirthDate = birthDate;
            Addresses = addresses;
            Array = array;
            Date = date;
        }
        [JsonProperty("name")]
        public string Name { get; }
        [JsonProperty("age")]
        public int Age { get; }
        [JsonProperty("birthDate")]
        public long BirthDate { get; }
        [JsonProperty("addresses")]
        public List<Address> Addresses { get; }
        [JsonProperty("array")]
        public int[] Array { get; }
        [JsonProperty("date")]
        public DateTime Date { get; }

    }

    class Address
    {
        public Address(string street)
        {
            Street = street;
        }
        [JsonProperty("street")]
        public string Street { get; }
    }
    [TestClass]
    public class JsonServiceTests
    {
        public JsonService jsonService = new JsonService();
        TestData testData;
        [TestInitialize]
        public void TestInitialize()
        {
            List<Address> addresses = new List<Address> { new Address("Francuska 29"), new Address("Dositejeva 64") };
            int[] array = { 1, 2, 3, 4, 5 };
            string name = "Milica";
            int age = 26;
            long birthday = 100000000000;
            DateTime date = new DateTime(2009, 2, 15, 0, 0, 0, DateTimeKind.Utc);
            testData = new TestData(name, age, birthday, addresses, array, date);
        }

        [TestMethod]
        public void SerializeToJsonTest()
        {
            string json = jsonService.SerializeToJson(testData);
            string expectedJson = "{\"name\":\"Milica\",\"age\":26,\"birthDate\":100000000000,\"addresses\":[{\"street\":\"Francuska 29\"},{\"street\":\"Dositejeva 64\"}],\"array\":[1,2,3,4,5],\"date\":\"2009-02-15T00:00:00Z\"}";

            Assert.AreEqual(expectedJson, json);
        }

        [TestMethod]
        public void DeserializeFromJsonTest()
        {
            string json = "{\"name\":\"Milica\",\"age\":26,\"birthDate\":100000000000,\"addresses\":[{\"street\":\"Francuska 29\"},{\"street\":\"Dositejeva 64\"}],\"array\":[1,2,3,4,5],\"date\":\"2009-02-15\"}";
            TestData deserializedTestData = jsonService.DeserializeFromJson<TestData>(json);
            AssertTestData(deserializedTestData);
        }

        [TestMethod]
        public void DeserializeFromJsonTest_missingMemberHandling()
        {
            string json = "{\"surname\":\"Milica\",\"name\":\"Milica\",\"age\":26,\"birthDate\":100000000000,\"addresses\":[{\"street\":\"Francuska 29\"},{\"street\":\"Dositejeva 64\"}],\"array\":[1,2,3,4,5],\"date\":\"2009-02-15\"}";
            TestData deserializedTestData = jsonService.DeserializeFromJson<TestData>(json);
            AssertTestData(deserializedTestData);
        }

        private void AssertTestData(TestData deserializedTestData)
        {
            Assert.AreEqual(testData.Name, deserializedTestData.Name);
            Assert.AreEqual(testData.Age, deserializedTestData.Age);
            Assert.AreEqual(testData.BirthDate, deserializedTestData.BirthDate);
            Assert.AreEqual(testData.Addresses.Count, deserializedTestData.Addresses.Count);
            Assert.AreEqual(testData.Addresses[0].Street, deserializedTestData.Addresses[0].Street);
            Assert.AreEqual(testData.Addresses[1].Street, deserializedTestData.Addresses[1].Street);
            CollectionAssert.AreEqual(testData.Array, deserializedTestData.Array);
            Assert.AreEqual(testData.Date, deserializedTestData.Date);
        }


    }
}