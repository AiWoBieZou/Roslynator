﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Pihrtsoft.CodeAnalysis.CSharp.Refactorings;

namespace Pihrtsoft.CodeAnalysis.CSharp.CodeFixProviders
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ReplaceVarWithExplicitTypeInForEachCodeFixProvider))]
    [Shared]
    public class ReplaceVarWithExplicitTypeInForEachCodeFixProvider : BaseCodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
            => ImmutableArray.Create(DiagnosticIdentifiers.UseExplicitTypeInsteadOfVarInForEach);

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            SyntaxNode root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            ForEachStatementSyntax forEachStatement = root
                .FindNode(context.Span, getInnermostNodeForTie: true)?
                .FirstAncestorOrSelf<ForEachStatementSyntax>();

            if (forEachStatement == null)
                return;

            if (context.Document.SupportsSemanticModel)
            {
                SemanticModel semanticModel = await context.Document.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);

                ITypeSymbol typeSymbol = semanticModel.GetTypeInfo(forEachStatement.Type, context.CancellationToken).Type;

                if (typeSymbol != null)
                {
                    CodeAction codeAction = CodeAction.Create(
                        $"Change type to '{typeSymbol.ToDisplayString(TypeSyntaxRefactoring.SymbolDisplayFormat)}'",
                        cancellationToken => ChangeTypeAsync(context.Document, forEachStatement.Type, typeSymbol, cancellationToken),
                        DiagnosticIdentifiers.UseExplicitTypeInsteadOfVarInForEach + EquivalenceKeySuffix);

                    context.RegisterCodeFix(codeAction, context.Diagnostics);
                }
            }
        }

        private static async Task<Document> ChangeTypeAsync(
            Document document,
            TypeSyntax type,
            ITypeSymbol typeSymbol,
            CancellationToken cancellationToken)
        {
            SyntaxNode oldRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

            TypeSyntax newType = TypeSyntaxRefactoring.CreateTypeSyntax(typeSymbol)
                .WithTriviaFrom(type)
                .WithSimplifierAnnotation();

            SyntaxNode newRoot = oldRoot.ReplaceNode(type, newType);

            return document.WithSyntaxRoot(newRoot);
        }
    }
}