// Copyright (c) Andrew Arnott. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CSharpIsNullAnalyzer
{
    using System;
    using System.Collections.Immutable;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.Text;

    /// <summary>
    /// An analyzer that finds <c>== null</c> expressions.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CSIsNull001 : DiagnosticAnalyzer
    {
        /// <summary>
        /// The ID for diagnostics reported by this analyzer.
        /// </summary>
        public const string Id = "CSIsNull001";

        /// <summary>
        /// The descriptor used for diagnostics created by this rule.
        /// </summary>
        internal static readonly DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(
            id: Id,
            title: Strings.CSIsNull001_Title,
            messageFormat: Strings.CSIsNull001_MessageFormat,
            helpLinkUri: Utils.GetHelpLink(Id),
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Info,
            isEnabledByDefault: true);

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Descriptor);

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.ReportDiagnostics);

            context.RegisterSyntaxNodeAction(
                ctxt =>
                {
                    if (ctxt.Node is BinaryExpressionSyntax
                        {
                            OperatorToken: { RawKind: (int)SyntaxKind.EqualsEqualsToken } opRight,
                            Right: LiteralExpressionSyntax { RawKind: (int)SyntaxKind.NullLiteralExpression } right,
                        })
                    {
                        Location location = ctxt.Node.SyntaxTree.GetLocation(new TextSpan(opRight.SpanStart, right.Span.End - opRight.SpanStart));
                        ctxt.ReportDiagnostic(Diagnostic.Create(Descriptor, location));
                    }
                    else if (ctxt.Node is BinaryExpressionSyntax
                        {
                            OperatorToken: { RawKind: (int)SyntaxKind.EqualsEqualsToken } opLeft,
                            Left: LiteralExpressionSyntax { RawKind: (int)SyntaxKind.NullLiteralExpression } left,
                        })
                    {
                        Location location = ctxt.Node.SyntaxTree.GetLocation(new TextSpan(left.SpanStart, opLeft.Span.End - left.SpanStart));
                        ctxt.ReportDiagnostic(Diagnostic.Create(Descriptor, location));
                    }
                },
                SyntaxKind.EqualsExpression);
        }
    }
}
