﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Pihrtsoft.CodeAnalysis
{
    public static class SyntaxTriviaExtensions
    {
        public static int GetSpanStartLine(this SyntaxTrivia trivia, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (trivia.SyntaxTree != null)
                return trivia.SyntaxTree.GetLineSpan(trivia.Span, cancellationToken).StartLine();

            return -1;
        }

        public static int GetFullSpanStartLine(this SyntaxTrivia trivia, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (trivia.SyntaxTree != null)
                return trivia.SyntaxTree.GetLineSpan(trivia.FullSpan, cancellationToken).StartLine();

            return -1;
        }

        public static int GetSpanEndLine(this SyntaxTrivia trivia, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (trivia.SyntaxTree != null)
                return trivia.SyntaxTree.GetLineSpan(trivia.Span, cancellationToken).EndLine();

            return -1;
        }

        public static int GetFullSpanEndLine(this SyntaxTrivia trivia, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (trivia.SyntaxTree != null)
                return trivia.SyntaxTree.GetLineSpan(trivia.FullSpan, cancellationToken).EndLine();

            return -1;
        }

        public static bool IsKind(this SyntaxTrivia trivia, SyntaxKind kind1, SyntaxKind kind2)
        {
            SyntaxKind kind = trivia.Kind();

            return kind == kind1
                || kind == kind2;
        }

        public static bool IsKind(this SyntaxTrivia trivia, SyntaxKind kind1, SyntaxKind kind2, SyntaxKind kind3)
        {
            SyntaxKind kind = trivia.Kind();

            return kind == kind1
                || kind == kind2
                || kind == kind3;
        }

        public static bool IsKind(this SyntaxTrivia trivia, SyntaxKind kind1, SyntaxKind kind2, SyntaxKind kind3, SyntaxKind kind4)
        {
            SyntaxKind kind = trivia.Kind();

            return kind == kind1
                || kind == kind2
                || kind == kind3
                || kind == kind4;
        }

        public static bool IsKind(this SyntaxTrivia trivia, SyntaxKind kind1, SyntaxKind kind2, SyntaxKind kind3, SyntaxKind kind4, SyntaxKind kind5)
        {
            SyntaxKind kind = trivia.Kind();

            return kind == kind1
                || kind == kind2
                || kind == kind3
                || kind == kind4
                || kind == kind5;
        }

        public static bool IsKind(this SyntaxTrivia trivia, SyntaxKind kind1, SyntaxKind kind2, SyntaxKind kind3, SyntaxKind kind4, SyntaxKind kind5, SyntaxKind kind6)
        {
            SyntaxKind kind = trivia.Kind();

            return kind == kind1
                || kind == kind2
                || kind == kind3
                || kind == kind4
                || kind == kind5
                || kind == kind6;
        }

        public static bool IsWhitespaceTrivia(this SyntaxTrivia trivia)
        {
            return Microsoft.CodeAnalysis.CSharpExtensions.IsKind(trivia, SyntaxKind.WhitespaceTrivia);
        }

        public static bool IsEndOfLineTrivia(this SyntaxTrivia trivia)
        {
            return Microsoft.CodeAnalysis.CSharpExtensions.IsKind(trivia, SyntaxKind.EndOfLineTrivia);
        }

        public static bool IsWhitespaceOrEndOfLineTrivia(this SyntaxTrivia trivia)
        {
            return trivia.IsWhitespaceTrivia() || trivia.IsEndOfLineTrivia();
        }

        public static bool IsSingleLineCommentTrivia(this SyntaxTrivia trivia)
        {
            return Microsoft.CodeAnalysis.CSharpExtensions.IsKind(trivia, SyntaxKind.SingleLineCommentTrivia);
        }

        public static bool IsMultiLineCommentTrivia(this SyntaxTrivia trivia)
        {
            return Microsoft.CodeAnalysis.CSharpExtensions.IsKind(trivia, SyntaxKind.MultiLineCommentTrivia);
        }

        public static bool IsSingleLineDocumentationCommentTrivia(this SyntaxTrivia trivia)
        {
            return Microsoft.CodeAnalysis.CSharpExtensions.IsKind(trivia, SyntaxKind.SingleLineDocumentationCommentTrivia);
        }

        public static bool IsMultiLineDocumentationCommentTrivia(this SyntaxTrivia trivia)
        {
            return Microsoft.CodeAnalysis.CSharpExtensions.IsKind(trivia, SyntaxKind.MultiLineDocumentationCommentTrivia);
        }

        public static bool IsDocumentationCommentTrivia(this SyntaxTrivia trivia)
        {
            return trivia.IsKind(
                SyntaxKind.SingleLineDocumentationCommentTrivia,
                SyntaxKind.MultiLineDocumentationCommentTrivia);
        }

        public static bool IsCommentTrivia(this SyntaxTrivia trivia)
        {
            return trivia.IsKind(
                SyntaxKind.SingleLineCommentTrivia,
                SyntaxKind.SingleLineDocumentationCommentTrivia,
                SyntaxKind.MultiLineCommentTrivia,
                SyntaxKind.MultiLineDocumentationCommentTrivia);
        }
    }
}
