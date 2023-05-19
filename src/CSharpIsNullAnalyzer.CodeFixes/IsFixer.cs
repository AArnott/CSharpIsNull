// Copyright (c) Andrew Arnott. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq.Expressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace CSharpIsNullAnalyzer
{
    /// <summary>
    /// Provides shared methods to replace binary expression with 'is' expression/Pattern syntax.
    /// </summary>
    internal static class IsFixer
    {
        /// <summary>
        /// Replaces the <paramref name="expr"/> with an 'is' expressionOrPattern.
        /// </summary>
        /// <param name="expr">The binary expressionOrPattern to replace.</param>
        /// <param name="document">The document in which to do the fix.</param>
        /// <param name="syntaxRoot">The root SyntaxNode to update.</param>
        /// <param name="pattern">The expressionOrPattern to replace the binary expressio with.</param>
        /// <returns>Document with binary expressionOrPattern replace with expressionOrPattern syntax.</returns>
        public static Task<Document> ReplaceBinaryExpressionWithIsPattern(this BinaryExpressionSyntax expr, Document document, SyntaxNode syntaxRoot, PatternSyntax pattern)
            => expr.ReplaceBinaryExpressionWithIsExpressionOrPattern(document, syntaxRoot, pattern, IsPatternExpression);

        /// <summary>
        /// Replaces the <paramref name="expr"/> with an 'is' expressionOrPattern.
        /// </summary>
        /// <param name="expr">The binary expressionOrPattern to replace.</param>
        /// <param name="document">The document in which to do the fix.</param>
        /// <param name="syntaxRoot">The root SyntaxNode to update.</param>
        /// <param name="expression">The expressionOrPattern to replace the binary expressio with.</param>
        /// <returns>Document with binary expressionOrPattern replace with expressionOrPattern syntax.</returns>
        public static Task<Document> ReplaceBinaryExpressionWithIsExpression(this BinaryExpressionSyntax expr, Document document, SyntaxNode syntaxRoot, ExpressionSyntax expression)
            => expr.ReplaceBinaryExpressionWithIsExpressionOrPattern(document, syntaxRoot, expression, IsExpression);

        private static Task<Document> ReplaceBinaryExpressionWithIsExpressionOrPattern<T>(
            this BinaryExpressionSyntax expr,
            Document document,
            SyntaxNode syntaxRoot,
            T expressionOrPattern,
            Func<ExpressionSyntax, T, ExpressionSyntax> create)
            where T : ExpressionOrPatternSyntax
        {
            T expressionWithTrivia = expressionOrPattern.WithTriviaFrom(expr.Right);
            ExpressionSyntax changedExpression = expr.Right is LiteralExpressionSyntax { RawKind: (int)SyntaxKind.NullLiteralExpression or (int)SyntaxKind.DefaultLiteralExpression }
                ? create(expr.Left, expressionWithTrivia)
                : create(expr.Right.WithoutTrailingTrivia().WithTrailingTrivia(Space), expressionWithTrivia);
            SyntaxNode updatedSyntaxRoot = syntaxRoot.ReplaceNode(expr, changedExpression);
            return Task.FromResult(document.WithSyntaxRoot(updatedSyntaxRoot));
        }

        /// <summary>
        /// Creates a new IsExpressionSyntax instance.
        /// </summary>
        private static BinaryExpressionSyntax IsExpression(ExpressionSyntax left, ExpressionSyntax right)
            => BinaryExpression(SyntaxKind.IsExpression, left, right);
    }
}
