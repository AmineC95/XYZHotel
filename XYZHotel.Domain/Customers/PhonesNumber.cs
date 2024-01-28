﻿using System.ComponentModel.DataAnnotations;

namespace XYZHotel.Domain.Customers
{
    public class PhonesNumber
    {
        [Phone]
        public string Number { get; }

        public PhonesNumber(string number)
        {
            Number = number;
        }

        public override string ToString()
        {
            return Number;
        }
    }
}
