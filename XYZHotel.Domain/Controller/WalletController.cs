using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using XYZHotel.Domain.Entities;
using XYZHotel.Domain.ValueObjects;

namespace XYZHotel.Domain.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly string _csvFilePath;

        public WalletController()
        {
            _csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "WalletDB.csv");
        }

        [HttpPost("/CreateWallet/{id}")]
        public IActionResult CreateWallet(Guid id)
        {
            var wallets = ReadWalletsFromCsv();
            var wallet = wallets.FirstOrDefault(w => w.Id == id);
            if (wallet != null)
                return BadRequest("Wallet already exists.");

            wallet = new Wallet(id, new Balance
            {
                Amount = 0,
                Currency = Enums.Currency.EUR
            });
            wallets.Add(wallet);
            WriteWalletsToCsv(wallets);
            return Ok(wallet);
        }

        [HttpGet("/GetWallet/{id}")]
        public IActionResult GetWallet(Guid id)
        {
            var wallets = ReadWalletsFromCsv();
            var wallet = wallets.FirstOrDefault(w => w.Id == id);
            if (wallet == null)
            {
                return CreateWallet(id);
            }

            return Ok(wallet);
        }

        [HttpPost("/CreditWallet/{id}")]
        public IActionResult CreditWallet(Guid id, [FromBody] Balance credit)
        {
            var wallets = ReadWalletsFromCsv();
            var wallet = wallets.FirstOrDefault(w => w.Id == id);
            if (wallet == null)
                return NotFound("Wallet not found.");

            if (credit.Currency == null || wallet.Balance.Currency == null)
                return BadRequest("Currency cannot be null.");

            var exchangeRate = GetExchangeRate(credit.Currency.Value, wallet.Balance.Currency.Value);
            var amountInWalletCurrency = credit.Amount * exchangeRate;
            wallet.Balance.Amount += amountInWalletCurrency;
            WriteWalletsToCsv(wallets);
            return Ok(wallet);
        }

        [HttpPost("/DebitWallet/{id}")]
        public IActionResult DebitWallet(Guid id, [FromBody] Balance debit)
        {
            var wallets = ReadWalletsFromCsv();
            var wallet = wallets.FirstOrDefault(w => w.Id == id);
            if (wallet == null)
                return NotFound("Wallet not found.");

            if (wallet.Balance.Amount < debit.Amount)
                return BadRequest("Insufficient balance.");

            wallet.Balance.Amount -= debit.Amount;
            WriteWalletsToCsv(wallets);
            return Ok(wallet);
        }

        private void WriteWalletsToCsv(List<Wallet> wallets)
        {
            var lines = new List<string> { "Id,Balance, Currency" };
            lines.AddRange(wallets.Select(wallet =>
                $"{wallet.Id},{wallet.Balance.Amount},{wallet.Balance.Currency}"));

            System.IO.File.WriteAllLines(_csvFilePath, lines);
        }

        private List<Wallet> ReadWalletsFromCsv()
        {
            var wallets = new List<Wallet>();

            var lines = System.IO.File.ReadAllLines(_csvFilePath);

            foreach (var line in lines.Skip(1))
            {
                var values = line.Split(',');
                var wallet = new Wallet(new Guid(values[0]), new Balance
                {
                    Amount = decimal.Parse(values[1]),
                    Currency = (Enums.Currency)Enum.Parse(typeof(Enums.Currency), values[2])
                });
                wallets.Add(wallet);
            }

            return wallets;
        }

        private decimal GetExchangeRate(Enums.Currency sourceCurrency, Enums.Currency targetCurrency)
        {
            if (sourceCurrency == targetCurrency)
                return 1m;

            switch (sourceCurrency)
            {
                case Enums.Currency.USD:
                    return 0.85m;
                case Enums.Currency.GBP:
                    return 1.17m;
                case Enums.Currency.JPY:
                    return 0.0075m;
                default:
                    throw new ArgumentException("Invalid source currency");
            }
        }
    }
}