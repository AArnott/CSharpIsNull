// Copyright (c) Andrew Arnott. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace CSharpIsNullAnalyzer;

/// <summary>
/// Provides code fixes for <see cref="CSIsNull002"/>.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp)]
public class CSIsNull002Fixer : CodeFixProvider
{
    /// <summary>
    /// The equivalence key used for the code fix that uses <c>is object</c> syntax.
    /// </summary>
    public const string IsObjectEquivalenceKey = "IsObject";

    /// <summary>
    /// The equivalence key used for the code fix that uses <c>is not null</c> syntax.
    /// </summary>
    public const string IsNotNullEquivalenceKey = "IsNotNull";

    private static readonly ImmutableArray<string> ReusableFixableDiagnosticIds = ImmutableArray.Create(
        CSIsNull002.Id);

    private static readonly ExpressionSyntax ObjectLiteral = PredefinedType(Token(SyntaxKind.ObjectKeyword));
    private static readonly PatternSyntax NotNullPattern = UnaryPattern(Token(SyntaxKind.NotKeyword), ConstantPattern(LiteralExpression(SyntaxKind.NullLiteralExpression)));

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
                BinaryExpressionSyntax? expr = syntaxRoot?.FindNode(diagnostic.Location.SourceSpan, getInnermostNodeForTie: true).FirstAncestorOrSelf<BinaryExpressionSyntax>();
                if (expr is not null)
                {
                    if (context.Document.Project.ParseOptions is CSharpParseOptions { LanguageVersion: >= LanguageVersion.CSharp9 } &&
                        diagnostic.Properties.ContainsKey(CSIsNull002.OfferIsNotNullFixKey))
                    {
                        context.RegisterCodeFix(
                            CodeAction.Create(
                                Strings.CSIsNull002_Fix2Title,
                                ct => expr.ReplaceBinaryExpressionWithIsPattern(context.Document, syntaxRoot!, NotNullPattern),
                                equivalenceKey: IsNotNullEquivalenceKey),
                            diagnostic);
                    }

                    context.RegisterCodeFix(
                        CodeAction.Create(
                            Strings.CSIsNull002_Fix1Title,
                            ct => expr.ReplaceBinaryExpressionWithIsExpression(context.Document, syntaxRoot!, ObjectLiteral),
                            equivalenceKey: IsObjectEquivalenceKey),
                        diagnostic);
                }
            }
        }
    }

    /// <inheritdoc />
    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;
}
