// Copyright (c) Andrew Arnott. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CSharpIsNullAnalyzer
{
    /// <summary>
    /// Utility methods for analyzers.
    /// </summary>
    internal static class Utils
    {
        /// <summary>
        /// Gets the URL to the help topic for a particular analyzer.
        /// </summary>
        /// <param name="analyzerId">The ID of the analyzer.</param>
        /// <returns>The URL for the analyzer's documentation.</returns>
        internal static string GetHelpLink(string analyzerId)
        {
            return $"https://github.com/aarnott/CSharpIsNullAnalyzer/blob/main/doc/analyzers/{analyzerId}.md";
        }
    }
}
