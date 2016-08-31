// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Pihrtsoft.CodeAnalysis.CSharp.Refactorings
{
    internal static class UsePostfixUnaryOperatorInsteadOfAssignmentRefactoring
    {
        public static async Task<Document> RefactorAsync(
            Document document,
            AssignmentExpressionSyntax assignment,
            SyntaxKind kind,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            SyntaxNode oldRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

            PostfixUnaryExpressionSyntax postfixUnary = PostfixUnaryExpression(kind, assignment.Left)
                .WithTriviaFrom(assignment)
                .WithFormatterAnnotation();

            SyntaxNode newRoot = oldRoot.ReplaceNode(assignment, postfixUnary);

            return document.WithSyntaxRoot(newRoot);
        }

        public static SyntaxKind GetPostfixUnaryOperatorKind(AssignmentExpressionSyntax assignment)
        {
            if (assignment == null)
                throw new ArgumentNullException(nameof(assignment));

            switch (assignment.Kind())
            {
                case SyntaxKind.AddAssignmentExpression:
                    return SyntaxKind.PostIncrementExpression;
                case SyntaxKind.SubtractAssignmentExpression:
                    return SyntaxKind.PostDecrementExpression;
            }

            switch (assignment.Right?.Kind())
            {
                case SyntaxKind.AddExpression:
                    return SyntaxKind.PostIncrementExpression;
                case SyntaxKind.SubtractExpression:
                    return SyntaxKind.PostDecrementExpression;
            }

            Debug.Assert(false, assignment.Kind().ToString());

            return SyntaxKind.None;
        }

        public static string GetOperatorText(AssignmentExpressionSyntax assignment)
        {
            return GetOperatorText(GetPostfixUnaryOperatorKind(assignment));
        }

        public static string GetOperatorText(SyntaxKind kind)
        {
            if (kind == SyntaxKind.PostIncrementExpression)
            {
                return "++";
            }
            else if (kind == SyntaxKind.PostDecrementExpression)
            {
                return "--";
            }

            return "";
        }
    }
}
