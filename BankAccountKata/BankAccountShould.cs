using System;
using FluentAssertions;
using Xunit;

namespace BankAccountKata
{
    static class  Builder
    {
        public static AccountBuilder AccountBuilder()
        {
            return new AccountBuilder();
        }
    }
    class AccountBuilder
    {
        private readonly Account _account;

        public AccountBuilder()
        {
            _account=new Account();
        }

        public AccountBuilder WithBalance(Amount balance)
        {
            _account.Balance = balance;
            return this;
        }

        public Account Build()
        {
            return _account;
        }
    }
    public class BankAccountShould
    {
        [Fact]
        public void receive_a_deposit()
        {
            var account = new Account();
            var amount = new Amount(10m);
            account.Deposit(amount);
            account.Balance.Value.Should().Be(amount.Value);
        }

        [Fact]
        public void receive_a_withdrawal()
        {
            var account = Builder.AccountBuilder().WithBalance(new Amount(10m)).Build();
            account.Withdrawal(new Amount(6));
            account.Balance.Value.Should().Be(4);
        }
    }


    public class Account
    {
        public Account()
        {
            Balance = new Amount();
        }


        public Amount Balance { get; set; }

        public void Deposit(Amount amount)
        {
            Balance.Value += amount.Value;
        }

        public void Withdrawal(Amount amount)
        {
            Balance.Value -= amount.Value;
        }
    }

    public class Amount
    {
        public Amount()
        {
            
        }

        public Amount(decimal value)
        {
            Value = value;
        }

        public decimal Value { get; set; }
    }
}
