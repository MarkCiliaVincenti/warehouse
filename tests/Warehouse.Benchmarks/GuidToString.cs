﻿using BenchmarkDotNet.Attributes;
using System.Text.RegularExpressions;
using Vayosoft.Core.Utilities;

namespace Warehouse.Benchmarks
{
    [MemoryDiagnoser]
    public class GuidToString
    {
        private readonly Guid _guid = Guid.NewGuid();

        [Benchmark]
        public string ToStringFromGuid()
        {
            return GuidUtils.GetStringFromGuid(_guid);
        }

        [Benchmark]
        public string ShortUID()
        {
            return ShortUID(_guid);
        }

        public static string ShortUID(Guid? id = null, bool fixedLength = true)
        {
            var guid = id ?? Guid.NewGuid();
            var byteArray = guid.ToByteArray();
            return fixedLength
                ? Base64Utils.EncodeBase64URL(byteArray).Substring(0, 22)
                : Regex.Replace(Base64Utils.EncodeBase64(byteArray), "[/+=]", "");
        }
    }
}
