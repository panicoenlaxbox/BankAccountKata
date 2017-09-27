using System;
using FluentAssertions;
using Xunit;

namespace BankAccountKata
{
    public class BankAccountShould
    {
        [Fact]
        public void receive_a_deposit()
        {
            var account = new Account();
            var amount = 10m;
            account.Deposit(amount);
            account.Balance.Value.Should().Be(amount);
        }
    }


    public class Account
    {
        public Account()
        {
            Balance = new Amount();
        }

        public Amount Balance { get; set; }

        public void Deposit(decimal amount)
        {
            Balance.Value += amount;
        }
    }

    public class Amount
    {
        public decimal Value { get; set; }
    }
}
