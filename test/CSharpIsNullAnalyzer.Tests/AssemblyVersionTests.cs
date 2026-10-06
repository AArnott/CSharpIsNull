// Copyright (c) Andrew Arnott. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/// <summary>
/// Verifies that projects with their own version.json get the assembly version it specifies
/// rather than the repo root version.json's version (e.g. when a root GitVersionBaseDirectory overrides it).
/// </summary>
/// <remarks>
/// The expected versions are computed at build time by the AddNestedVersionJsonAssemblyVersionExpectations target in test/Directory.Build.targets.
/// </remarks>
public class AssemblyVersionTests
{
    [Test]
    public void CSharpIsNullAnalyzer() => AssertNestedVersionJsonAssemblyVersion(typeof(global::CSharpIsNullAnalyzer.CSIsNull001).Assembly);

    [Test]
    public void CSharpIsNullAnalyzerCodeFixes() => AssertNestedVersionJsonAssemblyVersion(typeof(global::CSharpIsNullAnalyzer.CSIsNull001Fixer).Assembly);

    private static void AssertNestedVersionJsonAssemblyVersion(System.Reflection.Assembly assembly)
    {
        System.Reflection.AssemblyName assemblyName = assembly.GetName();
        string? expected = GetExpectedAssemblyVersion(assemblyName.Name!);
        Assert.True(expected is not null, $"The test project should list {assemblyName.Name} as a NestedVersionJsonProject.");
        Assert.Equal(Version.Parse(expected), assemblyName.Version);
    }

    private static string? GetExpectedAssemblyVersion(string assemblyName)
    {
        return typeof(AssemblyVersionTests).Assembly.GetCustomAttributes(typeof(System.Reflection.AssemblyMetadataAttribute), inherit: false)
            .Cast<System.Reflection.AssemblyMetadataAttribute>()
            .SingleOrDefault(a => a.Key == $"ExpectedAssemblyVersion:{assemblyName}")?.Value;
    }
}
