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
    /// Provides code fixes for <see cref="CSIsNull002"/>.
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp)]
    public class CSIsNull002Fixer : CodeFixProvider
    {
        private static readonly ImmutableArray<string> ReusableFixableDiagnosticIds = ImmutableArray.Create(
            CSIsNull002.Id);

        private static readonly ExpressionSyntax ObjectLiteral = SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ObjectKeyword));

        /// <inheritdoc/>
        public override ImmutableArray<string> FixableDiagnosticIds => ReusableFixableDiagnosticIds;

        /// <inheritdoc/>
        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            foreach (Diagnostic diagnostic in context.Diagnostics)
            {
                if (diagnostic.Id == CSIsNull002.Id)
                {
                    SyntaxNode? syntaxRoot = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
                    BinaryExpressionSyntax? expr = syntaxRoot?.FindNode(diagnostic.Location.SourceSpan).FirstAncestorOrSelf<BinaryExpressionSyntax>();
                    if (expr is object)
                    {
                        context.RegisterCodeFix(
                            CodeAction.Create(
                                Strings.CSIsNull002_Fix1Title,
                                ct =>
                                {
                                    Document document = context.Document;
                                    ExpressionSyntax changedExpression = expr.Right is LiteralExpressionSyntax { RawKind: (int)SyntaxKind.NullLiteralExpression }
                                        ? SyntaxFactory.BinaryExpression(SyntaxKind.IsExpression, expr.Left, ObjectLiteral)
                                        : SyntaxFactory.BinaryExpression(SyntaxKind.IsExpression, expr.Right, ObjectLiteral);
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
