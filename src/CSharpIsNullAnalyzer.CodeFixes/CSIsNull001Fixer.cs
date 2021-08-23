// Copyright (c) Andrew Arnott. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CSharpIsNullAnalyzer
{
    using System;
    using System.Collections.Immutable;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Provides code fixes for <see cref="CSIsNull001"/>.
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp)]
    public class CSIsNull001Fixer : CodeFixProvider
    {
        private static readonly ImmutableArray<string> ReusableFixableDiagnosticIds = ImmutableArray.Create(
            CSIsNull001.Id);

        private static readonly ConstantPatternSyntax NullPattern = SyntaxFactory.ConstantPattern(SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression));

        /// <inheritdoc/>
        public override ImmutableArray<string> FixableDiagnosticIds => ReusableFixableDiagnosticIds;

        /// <inheritdoc/>
        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            foreach (Diagnostic diagnostic in context.Diagnostics)
            {
                if (diagnostic.Id == CSIsNull001.Id)
                {
                    SyntaxNode? syntaxRoot = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
                    BinaryExpressionSyntax? expr = syntaxRoot?.FindNode(diagnostic.Location.SourceSpan).FirstAncestorOrSelf<BinaryExpressionSyntax>();
                    if (expr is object)
                    {
                        context.RegisterCodeFix(
                            CodeAction.Create(
                                Strings.CSIsNull001_FixTitle,
                                ct =>
                                {
                                    Document document = context.Document;
                                    IsPatternExpressionSyntax changedExpression = expr.Right is LiteralExpressionSyntax { RawKind: (int)SyntaxKind.NullLiteralExpression }
                                        ? SyntaxFactory.IsPatternExpression(expr.Left, NullPattern)
                                        : SyntaxFactory.IsPatternExpression(expr.Right, NullPattern);
                                    syntaxRoot = (syntaxRoot!).ReplaceNode(expr, changedExpression);
                                    document = document.WithSyntaxRoot(syntaxRoot);
                                    return Task.FromResult(document);
                                },
                                equivalenceKey: "isNull"),
                            diagnostic);
                    }
                }
            }
        }

        /// <inheritdoc />
        public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;
    }
}
