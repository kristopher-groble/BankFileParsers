using BankFileParsers;
using NUnit.Framework;

namespace BaiFileParsers.Tests
{
    [TestFixture]
    public class AccountFundTypeFactoryTests
    {
        [Test]
        public void BofASampleFileTest()
        {
            AccountFundTypeFactory factory = new AccountFundTypeFactory(new[]
            {
                "03,122110446088,USD/",
                "88,030,917668634,,Z/",
                "88,060,917495434,,Z/",
                "88,072,173200,,Z/",
                "88,074,0,,Z/",
                "88,100,349587360,5,S,324102900,0,0/",
                "88,190,324102900,2,S,324102900,0,0/"
            });

            Assert.AreEqual("USD", factory.CurrencyCode);
            Assert.AreEqual("122110446088", factory.CustomerAccountNumber);
            Assert.AreEqual("03", factory.RecordCode);

            FundType fundType = factory.GetNext();
            Assert.AreEqual("030", fundType.TypeCode);
            Assert.AreEqual("030", fundType.Detail.TypeCode);
            Assert.AreEqual("Current Ledger", fundType.Detail.Description);
            Assert.AreEqual(LevelType.Status, fundType.Detail.Level);
            Assert.AreEqual("917668634", fundType.Amount);

            fundType = factory.GetNext();
            Assert.AreEqual("060", fundType.TypeCode);
            Assert.AreEqual("060", fundType.Detail.TypeCode);
            Assert.AreEqual("Current Available", fundType.Detail.Description);
            Assert.AreEqual(LevelType.Status, fundType.Detail.Level);
            Assert.AreEqual("917495434", fundType.Amount);

            fundType = factory.GetNext();
            Assert.AreEqual("072", fundType.TypeCode);
            Assert.AreEqual("072", fundType.Detail.TypeCode);
            Assert.AreEqual("1-Day Float", fundType.Detail.Description);
            Assert.AreEqual(LevelType.Status, fundType.Detail.Level);
            Assert.AreEqual("173200", fundType.Amount);

            fundType = factory.GetNext();
            Assert.AreEqual("074", fundType.TypeCode);
            Assert.AreEqual("074", fundType.Detail.TypeCode);
            Assert.AreEqual("2 or More Days Float", fundType.Detail.Description);
            Assert.AreEqual(LevelType.Status, fundType.Detail.Level);
            Assert.AreEqual("0", fundType.Amount);

            fundType = factory.GetNext();
            Assert.AreEqual("100", fundType.TypeCode);
            Assert.AreEqual("100", fundType.Detail.TypeCode);
            Assert.AreEqual("Total Credits", fundType.Detail.Description);
            Assert.AreEqual(LevelType.Summary, fundType.Detail.Level);
            Assert.AreEqual("5", fundType.ItemCount);
            Assert.AreEqual("S", fundType.FundsType);
            Assert.AreEqual("349587360", fundType.Amount);
            Assert.AreEqual("324102900", fundType.Immediate);

            fundType = factory.GetNext();
            Assert.AreEqual("190", fundType.TypeCode);
            Assert.AreEqual("190", fundType.Detail.TypeCode);
            Assert.AreEqual("Total Incoming Money Transfers", fundType.Detail.Description);
            Assert.AreEqual(LevelType.Summary, fundType.Detail.Level);
            Assert.AreEqual("2", fundType.ItemCount);
            Assert.AreEqual("S", fundType.FundsType);
            Assert.AreEqual("324102900", fundType.Amount);
            Assert.AreEqual("324102900", fundType.Immediate);

            fundType = factory.GetNext();
            Assert.IsNull(fundType);
        }

        [Test]
        public void OriginalSampleFileTest()
        {
            AccountFundTypeFactory factory = new AccountFundTypeFactory(new[]
            {
                "03,0123456789,,010,+4350000,,,040,2830000,,/",
                "88,072,1020000,,,074,500000,,/"
            });

            Assert.AreEqual("", factory.CurrencyCode);
            Assert.AreEqual("0123456789", factory.CustomerAccountNumber);
            Assert.AreEqual("03", factory.RecordCode);

            FundType fundType = factory.GetNext();
            Assert.AreEqual("010", fundType.TypeCode);
            Assert.AreEqual("010", fundType.Detail.TypeCode);
            Assert.AreEqual("Opening Ledger", fundType.Detail.Description);
            Assert.AreEqual(LevelType.Status, fundType.Detail.Level);
            Assert.AreEqual("+4350000", fundType.Amount);

            fundType = factory.GetNext();
            Assert.AreEqual("040", fundType.TypeCode);
            Assert.AreEqual("040", fundType.Detail.TypeCode);
            Assert.AreEqual("Opening Available", fundType.Detail.Description);
            Assert.AreEqual(LevelType.Status, fundType.Detail.Level);
            Assert.AreEqual("2830000", fundType.Amount);

            fundType = factory.GetNext();
            Assert.AreEqual("072", fundType.TypeCode);
            Assert.AreEqual("072", fundType.Detail.TypeCode);
            Assert.AreEqual("1-Day Float", fundType.Detail.Description);
            Assert.AreEqual(LevelType.Status, fundType.Detail.Level);
            Assert.AreEqual("1020000", fundType.Amount);

            fundType = factory.GetNext();
            Assert.AreEqual("074", fundType.TypeCode);
            Assert.AreEqual("074", fundType.Detail.TypeCode);
            Assert.AreEqual("2 or More Days Float", fundType.Detail.Description);
            Assert.AreEqual(LevelType.Status, fundType.Detail.Level);
            Assert.AreEqual("500000", fundType.Amount);

            fundType = factory.GetNext();
            Assert.IsNull(fundType);
        }
    }
}
