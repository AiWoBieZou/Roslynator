﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Pihrtsoft.CodeAnalysis.CSharp.Refactorings;

namespace Pihrtsoft.CodeAnalysis.CSharp.Internal.DiagnosticAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AwaitExpressionDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            => ImmutableArray.Create(DiagnosticDescriptors.AddConfigureAwait);

        public override void Initialize(AnalysisContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            context.RegisterSyntaxNodeAction(f => AnalyzeAwaitExpression(f), SyntaxKind.AwaitExpression);
        }

        private void AnalyzeAwaitExpression(SyntaxNodeAnalysisContext context)
        {
            var awaitExpression = (AwaitExpressionSyntax)context.Node;

            if (AddConfigureAwaitRefactoring.CanRefactor(awaitExpression, context.SemanticModel, context.CancellationToken))
            {
                context.ReportDiagnostic(
                    DiagnosticDescriptors.AddConfigureAwait,
                    awaitExpression.Expression.GetLocation());
            }
        }
    }
}
