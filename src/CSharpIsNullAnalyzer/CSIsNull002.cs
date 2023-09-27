// Copyright (c) Andrew Arnott. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis.Text;

namespace CSharpIsNullAnalyzer;

/// <summary>
/// An analyzer that finds <c>!= null</c> expressions.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class CSIsNull002 : DiagnosticAnalyzer
{
    /// <summary>
    /// The ID for diagnostics reported by this analyzer.
    /// </summary>
    public const string Id = "CSIsNull002";

    /// <summary>
    /// A key to a property that will be set in a diagnostic if the <c>is not null</c> code fix should be offered.
    /// </summary>
    public const string OfferIsNotNullFixKey = "OfferIsNotNullFix";

    /// <summary>
    /// The descriptor used for diagnostics created by this rule.
    /// </summary>
    internal static readonly DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(
        id: Id,
        title: Strings.CSIsNull002_Title,
        messageFormat: Strings.CSIsNull002_MessageFormat,
        helpLinkUri: Utils.GetHelpLink(Id),
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true);

    /// <inheritdoc/>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Descriptor);

    /// <inheritdoc/>
    public override void Initialize(AnalysisContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.ReportDiagnostics);

        context.RegisterCompilationStartAction(
            startContext =>
            {
                INamedTypeSymbol? linqExpressionType = startContext.Compilation.GetTypeByMetadataName(WellKnownTypeNames.SystemLinqExpressionsExpression1);
                startContext.RegisterOperationAction(
                    ctxt =>
                    {
                        if (ctxt.Operation.Type.SpecialType == SpecialType.System_Boolean)
                        {
                            if (ctxt.Operation is IBinaryOperation { OperatorKind: BinaryOperatorKind.NotEquals } binaryOp)
                            {
                                Location? location = null;
                                if (binaryOp.RightOperand is IConversionOperation { ConstantValue: { HasValue: true, Value: null } } or ILiteralOperation { ConstantValue: { HasValue: true, Value: null } })
                                {
                                    location = binaryOp.RightOperand.Syntax.GetLocation();
                                    if (binaryOp.Syntax is BinaryExpressionSyntax { OperatorToken: { } operatorLocation, Right: { } right })
                                    {
                                        location = ctxt.Operation.Syntax.SyntaxTree.GetLocation(new TextSpan(operatorLocation.SpanStart, right.Span.End - operatorLocation.SpanStart));
                                    }
                                }
                                else if (binaryOp.LeftOperand is IConversionOperation { ConstantValue: { HasValue: true, Value: null } } or ILiteralOperation { ConstantValue: { HasValue: true, Value: null } })
                                {
                                    location = binaryOp.LeftOperand.Syntax.GetLocation();
                                    if (binaryOp.Syntax is BinaryExpressionSyntax { OperatorToken: { } operatorLocation, Left: { } left })
                                    {
                                        location = ctxt.Operation.Syntax.SyntaxTree.GetLocation(new TextSpan(left.SpanStart, operatorLocation.Span.End - left.SpanStart));
                                    }
                                }

                                if (location is object)
                                {
                                    ImmutableDictionary<string, string?> properties = ImmutableDictionary<string, string?>.Empty;
                                    if (!binaryOp.IsWithinExpressionTree(linqExpressionType))
                                    {
                                        properties = properties.Add(OfferIsNotNullFixKey, "true");
                                    }

                                    ctxt.ReportDiagnostic(Diagnostic.Create(Descriptor, location, properties));
                                }
                            }
                        }
                    },
                    OperationKind.Binary);
            });
    }
}
