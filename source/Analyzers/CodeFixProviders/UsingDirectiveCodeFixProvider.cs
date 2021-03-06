﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Composition;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Pihrtsoft.CodeAnalysis.CSharp.SyntaxRewriters;

namespace Pihrtsoft.CodeAnalysis.CSharp.CodeFixProviders
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(UsingDirectiveCodeFixProvider))]
    [Shared]
    public class UsingDirectiveCodeFixProvider : BaseCodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(DiagnosticIdentifiers.AvoidUsageOfUsingAliasDirective); }
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            SyntaxNode root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            UsingDirectiveSyntax usingDirective = root
                .FindNode(context.Span, getInnermostNodeForTie: true)?
                .FirstAncestorOrSelf<UsingDirectiveSyntax>();

            if (usingDirective == null)
                return;

            string name = usingDirective.Alias.Name.Identifier.ValueText;

            CodeAction codeAction = CodeAction.Create(
                $"Remove alias '{name}'",
                cancellationToken => UsingAliasDirectiveSyntaxRewriter.RewriteAsync(context.Document, usingDirective, cancellationToken),
                DiagnosticIdentifiers.AvoidUsageOfUsingAliasDirective + EquivalenceKeySuffix);

            context.RegisterCodeFix(codeAction, context.Diagnostics);
        }
    }
}
