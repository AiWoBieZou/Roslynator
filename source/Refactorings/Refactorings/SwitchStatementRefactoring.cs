﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Pihrtsoft.CodeAnalysis.CSharp.Analysis;
using Pihrtsoft.CodeAnalysis.CSharp.Refactorings.SortSwitchSections;

namespace Pihrtsoft.CodeAnalysis.CSharp.Refactorings
{
    internal static class SwitchStatementRefactoring
    {
        public static async Task ComputeRefactoringsAsync(RefactoringContext context, SwitchStatementSyntax switchStatement)
        {
            if (context.IsRefactoringEnabled(RefactoringIdentifiers.GenerateSwitchSections)
                && await GenerateSwitchSectionsRefactoring.CanRefactorAsync(context, switchStatement).ConfigureAwait(false))
            {
                context.RegisterRefactoring(
                    "Generate switch sections",
                    cancellationToken =>
                    {
                        return GenerateSwitchSectionsRefactoring.RefactorAsync(
                            context.Document,
                            switchStatement,
                            cancellationToken);
                    });
            }

            if (context.IsRefactoringEnabled(RefactoringIdentifiers.RemoveStatementsFromSwitchSections))
                RemoveStatementsFromSwitchSectionsRefactoring.ComputeRefactoring(context, switchStatement);

            if (switchStatement.Sections.Count > 0
                && switchStatement.SwitchKeyword.Span.Contains(context.Span))
            {
                if (context.IsAnyRefactoringEnabled(
                    RefactoringIdentifiers.AddBracesToSwitchSections,
                    RefactoringIdentifiers.RemoveBracesFromSwitchSections))
                {
                    SwitchStatementAnalysisResult result = SwitchStatementAnalysis.Analyze(switchStatement);

                    if (result.CanAddBraces
                        && context.IsRefactoringEnabled(RefactoringIdentifiers.AddBracesToSwitchSections))
                    {
                        context.RegisterRefactoring(
                            "Add braces to switch sections",
                            cancellationToken => AddBracesToSwitchSectionsRefactoring.RefactorAsync(context.Document, switchStatement, cancellationToken));
                    }

                    if (result.CanRemoveBraces
                        && context.IsRefactoringEnabled(RefactoringIdentifiers.RemoveBracesFromSwitchSections))
                    {
                        context.RegisterRefactoring(
                            "Remove braces from switch sections",
                            cancellationToken => RemoveBracesFromSwitchSectionsRefactoring.RefactorAsync(context.Document, switchStatement, cancellationToken));
                    }
                }

                if (context.IsRefactoringEnabled(RefactoringIdentifiers.ReplaceSwitchWithIfElse)
                    && switchStatement.Sections
                        .Any(section => !section.Labels.Contains(SyntaxKind.DefaultSwitchLabel)))
                {
                    context.RegisterRefactoring(
                        "Replace 'switch' with 'if-else'",
                        cancellationToken => ReplaceSwitchWithIfElseRefactoring.RefactorAsync(context.Document, switchStatement, cancellationToken));
                }

                if (context.IsRefactoringEnabled(RefactoringIdentifiers.SortSwitchSections))
                {
                    context.RegisterRefactoring(
                        "Sort switch sections",
                        cancellationToken => SortSwitchSectionsRefactoring.RefactorAsync(context.Document, switchStatement, cancellationToken));
                }
            }
        }
    }
}