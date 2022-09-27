﻿using System.Text.RegularExpressions;

namespace Warehouse.UnitTests
{
    public class ValidationRules
    {
        [Fact]
        public void Check_MacAddress()
        {
            var pattern = @"^([0-9A-Fa-f]{2}[:-]?){5}([0-9A-Fa-f]{2})$";
            var matches = Regex.Match("DD340206CB76", pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            Assert.True(matches.Success);
        }
    }
}
