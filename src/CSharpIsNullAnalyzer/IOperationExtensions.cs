// Copyright (c) Andrew Arnott. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CSharpIsNullAnalyzer
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using Microsoft.CodeAnalysis;

    /// <summary>
    /// Extensions methods for the <see cref="IOperation"/> interface.
    /// </summary>
    internal static class IOperationExtensions
    {
        private static readonly ImmutableArray<OperationKind> LambdaAndLocalFunctionKinds =
            ImmutableArray.Create(OperationKind.AnonymousFunction, OperationKind.LocalFunction);

        /// <summary>
        /// Tests whether an operation falls within an expression tree.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="linqExpressionTreeType">The result of a call to <see cref="Compilation.GetTypeByMetadataName(string)"/>, passing in
        /// the full name of the <see cref="System.Linq.Expressions.Expression{TDelegate}"/> type.</param>
        /// <returns>A value indicating whether this operation falls within an expression tree.</returns>
        internal static bool IsWithinExpressionTree(this IOperation operation, [NotNullWhen(true)] INamedTypeSymbol? linqExpressionTreeType)
           => linqExpressionTreeType != null
               && operation.GetAncestor(LambdaAndLocalFunctionKinds)?.Parent?.Type?.OriginalDefinition is { } lambdaType
               && linqExpressionTreeType.Equals(lambdaType, SymbolEqualityComparer.Default);

        /// <summary>
        /// Gets the first ancestor of this operation with:
        ///  1. Any OperationKind from the specified <paramref name="ancestorKinds"/>.
        ///  2. If <paramref name="predicate"/> is non-null, it succeeds for the ancestor.
        /// Returns null if there is no such ancestor.
        /// </summary>
        /// <param name="root">The operation to start the search.</param>
        /// <param name="ancestorKinds">The kinds of ancestors to look for.</param>
        /// <param name="predicate">An optional test to apply before matching.</param>
        /// <returns>The matching ancestor operation, if found.</returns>
        internal static IOperation? GetAncestor(this IOperation root, ImmutableArray<OperationKind> ancestorKinds, Func<IOperation, bool>? predicate = null)
        {
            IOperation? ancestor = root;
            do
            {
                ancestor = ancestor.Parent;
            }
            while (ancestor is object && !ancestorKinds.Contains(ancestor.Kind));

            if (ancestor is object)
            {
                if (predicate is object && !predicate(ancestor))
                {
                    return GetAncestor(ancestor, ancestorKinds, predicate);
                }

                return ancestor;
            }
            else
            {
                return default;
            }
        }
    }
}
