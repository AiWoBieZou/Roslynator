﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;

namespace Pihrtsoft.CodeAnalysis
{
    public static class FileLinePositionSpanExtensions
    {
        public static int StartLine(this FileLinePositionSpan fileLinePositionSpan)
        {
            return fileLinePositionSpan.StartLinePosition.Line;
        }

        public static int EndLine(this FileLinePositionSpan fileLinePositionSpan)
        {
            return fileLinePositionSpan.EndLinePosition.Line;
        }

        public static bool IsMultiLine(this FileLinePositionSpan fileLinePositionSpan)
        {
            return fileLinePositionSpan.StartLine() != fileLinePositionSpan.EndLine();
        }

        public static bool IsSingleLine(this FileLinePositionSpan fileLinePositionSpan)
        {
            return !fileLinePositionSpan.IsMultiLine();
        }
    }
}
