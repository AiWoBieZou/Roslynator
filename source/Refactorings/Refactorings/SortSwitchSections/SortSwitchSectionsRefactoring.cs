﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Pihrtsoft.CodeAnalysis.CSharp.Refactorings.SortSwitchSections
{
    internal static class SortSwitchSectionsRefactoring
    {
        private static readonly SwitchLabelSorter _switchLabelSorter = new SwitchLabelSorter();
        private static readonly SwitchSectionSorter _switchSectionSorter = new SwitchSectionSorter();

        public static async Task<Document> RefactorAsync(
            Document document,
            SwitchStatementSyntax switchStatement,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            SyntaxNode oldRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

            SwitchStatementSyntax newSwitchStatement = SortSections(switchStatement)
                .WithFormatterAnnotation();

            SyntaxNode newRoot = oldRoot.ReplaceNode(switchStatement, newSwitchStatement);

            return document.WithSyntaxRoot(newRoot);
        }

        private static SwitchStatementSyntax SortSections(SwitchStatementSyntax switchStatement)
        {
            if (switchStatement == null)
                throw new ArgumentNullException(nameof(switchStatement));

            return switchStatement.WithSections(
                List(switchStatement.Sections
                    .Select(f => SortLabels(f))
                    .OrderBy(f => f, _switchSectionSorter)));
        }

        private static SwitchSectionSyntax SortLabels(SwitchSectionSyntax switchSection)
        {
            if (switchSection == null)
                throw new ArgumentNullException(nameof(switchSection));

            return SwitchSection(
                List(switchSection.Labels.OrderBy(f => f, _switchLabelSorter)),
                switchSection.Statements);
        }
    }
}
