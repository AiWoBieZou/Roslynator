﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pihrtsoft.CodeAnalysis.CSharp.Refactorings.Tests
{
    internal class ChangeMethodReturnTypeToVoidRefactoring
    {
        public string ProcessValue(string value)
        {
            ProcessValue(new string[] { value });
        }

        public void ProcessValue(string[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[0] = null;
            }
        }

        public IEnumerable<string> GetValues()
        {
            ProcessValue("");
        }

        public IEnumerable<string> GetValues2()
        {
            yield return "";
        }

        public string GetValues3()
        {
            yield return "";
        }

        public async Task GetValueAsync()
        {
            bool x = await Task.FromResult(false);
        }

        public int GetValues4()
        {
            yield break;
        }
    }
}
