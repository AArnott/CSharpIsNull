// Copyright (c) Andrew Arnott. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/// <summary>
/// Verifies that projects with their own version.json get its revision-level assembly version,
/// rather than the root version.json's x.y.0.0 (e.g. when a root GitVersionBaseDirectory overrides it).
/// </summary>
public class AssemblyVersionTests
{
    [Test]
    public void CSharpIsNullAnalyzer() => AssertRevisionIsNonZero(typeof(global::CSharpIsNullAnalyzer.CSIsNull001).Assembly);

    [Test]
    public void CSharpIsNullAnalyzerCodeFixes() => AssertRevisionIsNonZero(typeof(global::CSharpIsNullAnalyzer.CSIsNull001Fixer).Assembly);

    private static void AssertRevisionIsNonZero(System.Reflection.Assembly assembly)
    {
        System.Reflection.AssemblyName name = assembly.GetName();
        Assert.True(name.Version!.Revision != 0, $"{name.Name} has assembly version {name.Version}, but its own version.json should give it a non-zero revision.");
    }
}
