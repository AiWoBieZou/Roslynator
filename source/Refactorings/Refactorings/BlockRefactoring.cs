﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Pihrtsoft.CodeAnalysis.CSharp.Refactorings.WrapStatements;

namespace Pihrtsoft.CodeAnalysis.CSharp.Refactorings
{
    internal static class BlockRefactoring
    {
        public static async Task ComputeRefactoringAsync(RefactoringContext context, BlockSyntax block)
        {
            RemoveBracesRefactoring.ComputeRefactoring(context, block);

            if (context.IsAnyRefactoringEnabled(
                RefactoringIdentifiers.WrapInUsingStatement,
                RefactoringIdentifiers.CollapseToInitializer,
                RefactoringIdentifiers.MergeIfStatements,
                RefactoringIdentifiers.MergeLocalDeclarations,
                RefactoringIdentifiers.WrapInIfStatement,
                RefactoringIdentifiers.WrapInTryCatch))
            {
                SelectedStatementsInfo info = SelectedStatementsInfo.Create(block, context.Span);

                if (info.IsAnySelected)
                {
                    if (context.IsRefactoringEnabled(RefactoringIdentifiers.WrapInUsingStatement)
                        && context.SupportsSemanticModel)
                    {
                        var refactoring = new WrapInUsingStatementRefactoring();
                        await refactoring.ComputeRefactoringAsync(context, info);
                    }

                    if (context.IsRefactoringEnabled(RefactoringIdentifiers.CollapseToInitializer))
                        await CollapseToInitializerRefactoring.ComputeRefactoringsAsync(context, info);

                    if (context.IsRefactoringEnabled(RefactoringIdentifiers.MergeIfStatements))
                        MergeIfStatementsRefactoring.ComputeRefactorings(context, info);

                    if (context.IsRefactoringEnabled(RefactoringIdentifiers.MergeLocalDeclarations)
                        && context.SupportsSemanticModel)
                    {
                        await MergeLocalDeclarationsRefactoring.ComputeRefactoringsAsync(context, info);
                    }

                    if (context.IsRefactoringEnabled(RefactoringIdentifiers.MergeAssignmentExpressionWithReturnStatement))
                        MergeAssignmentExpressionWithReturnStatementRefactoring.ComputeRefactorings(context, info);

                    if (context.IsRefactoringEnabled(RefactoringIdentifiers.WrapInIfStatement))
                    {
                        context.RegisterRefactoring(
                            "Wrap in condition",
                            cancellationToken =>
                            {
                                var refactoring = new WrapInIfStatementRefactoring();
                                return refactoring.RefactorAsync(context.Document, info, cancellationToken);
                            });
                    }

                    if (context.IsRefactoringEnabled(RefactoringIdentifiers.WrapInTryCatch))
                    {
                        context.RegisterRefactoring(
                            "Wrap in try-catch",
                            cancellationToken =>
                            {
                                var refactoring = new WrapInTryCatchRefactoring();
                                return refactoring.RefactorAsync(context.Document, info, cancellationToken);
                            });
                    }
                }
            }
        }
    }
}
