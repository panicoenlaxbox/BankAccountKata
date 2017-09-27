using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace BankAccountKata
{
    static class Builder
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
            _account = new Account();
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

        [Fact]
        public void do_a_transfer_to_another_account()
        {
            var source = Builder.AccountBuilder().WithBalance(new Amount(10m)).Build();
            var destiny = Builder.AccountBuilder().WithBalance(new Amount(20m)).Build();
            source.Transfer(destiny, new Amount(6));
            source.Balance.Value.Should().Be(4);
            destiny.Balance.Value.Should().Be(26);
        }

        [Fact]
        public void withdrawal_drop_a_account_movement()
        {
            var account = Builder.AccountBuilder().WithBalance(new Amount(10m)).Build();
            account.Withdrawal(new Amount(6));
            account.Movements.Count().Should().Be(1);
            var movement = account.Movements.First();
            movement.Type.Should().Be(MovementType.Withdrawal);
            movement.Amount.Value.Should().Be(6);
        }

        [Fact]
        public void deposit_drop_a_account_movement()
        {
            var account = Builder.AccountBuilder().WithBalance(new Amount(10m)).Build();
            account.Deposit(new Amount(6));
            account.Movements.Count().Should().Be(1);
            var movement = account.Movements.First();
            movement.Type.Should().Be(MovementType.Deposit);
            movement.Amount.Value.Should().Be(6);
        }

        [Fact]
        public void movement_register_date_and_balance()
        {
            var account = Builder.AccountBuilder().WithBalance(new Amount(10m)).Build();
            account.Deposit(new Amount(6));

            var movement = account.Movements.First();
            movement.Date.Should().BeCloseTo(DateTime.Now);
            movement.Balance.Value.Should().Be(16);
        }
    }


    public class Account
    {
        private readonly IList<Movement> _movements;

        public Account()
        {
            Balance = new Amount();
            _movements = new List<Movement>();
        }

        public Amount Balance { get; set; }

        public IEnumerable<Movement> Movements => _movements;

        public void Deposit(Amount amount)
        {
            Balance.Value += amount.Value;
            AddMovement(MovementType.Deposit, amount);
        }

        public void Withdrawal(Amount amount)
        {
            Balance.Value -= amount.Value;
            AddMovement(MovementType.Withdrawal, amount);
        }

        private void AddMovement(MovementType type, Amount amount)
        {
            _movements.Add(new Movement(type, amount, DateTime.Now, Balance));
        }

        public void Transfer(Account destiny, Amount amount)
        {
            Withdrawal(amount);
            destiny.Deposit(amount);
        }
    }

    public class Movement
    {
        public Movement(MovementType type, Amount amount, DateTime date, Amount balance)
        {
            Type = type;
            Amount = amount;
            Date = date;
            Balance= balance;
        }

        public MovementType Type { get; }
        public Amount Amount { get; }
        public DateTime Date { get; }
        public Amount Balance { get; }
    }

    public enum MovementType
    {
        Withdrawal,
        Deposit
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
