// Copyright (c) Andrew Arnott. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Xunit;
using VerifyCS = CSharpCodeFixVerifier<CSharpIsNullAnalyzer.CSIsNull001, CSharpIsNullAnalyzer.CSIsNull001Fixer>;

public class CsIsNull001Tests
{
    [Fact]
    public async Task EqualsNullInExpressionBody_ProducesDiagnostic()
    {
        string source = @"
class Test
{
    bool Method(object o) => o [|== null|];
}";

        string fixedSource = @"
class Test
{
    bool Method(object o) => o is null;
}";

        await VerifyCS.VerifyCodeFixAsync(source, fixedSource);
    }

    [Fact]
    public async Task NullEqualsInExpressionBody_ProducesDiagnostic()
    {
        string source = @"
class Test
{
    bool Method(object o) => [|null ==|] o;
}";

        string fixedSource = @"
class Test
{
    bool Method(object o) => o is null;
}";

        await VerifyCS.VerifyCodeFixAsync(source, fixedSource);
    }

    [Fact]
    public async Task EqualsNullInIfExpression_ProducesDiagnostic()
    {
        string source = @"
class Test
{
    void Method(object o)
    {
        if (o [|== null|])
        {
        }
    }
}";

        string fixedSource = @"
class Test
{
    void Method(object o)
    {
        if (o is null)
        {
        }
    }
}";

        await VerifyCS.VerifyCodeFixAsync(source, fixedSource);
    }

    [Fact]
    public async Task NullEqualsInIfExpression_ProducesDiagnostic()
    {
        string source = @"
class Test
{
    void Method(object o)
    {
        if ([|null ==|] o)
        {
        }
    }
}";

        string fixedSource = @"
class Test
{
    void Method(object o)
    {
        if (o is null)
        {
        }
    }
}";

        await VerifyCS.VerifyCodeFixAsync(source, fixedSource);
    }

    [Fact]
    public async Task EqualsInArgument_ProducesDiagnostic()
    {
        string source = @"
class Test
{
    void Method(object o)
    {
        Other(o [|== null|]);
    }

    void Other(bool condition) { }
}";

        string fixedSource = @"
class Test
{
    void Method(object o)
    {
        Other(o is null);
    }

    void Other(bool condition) { }
}";

        await VerifyCS.VerifyCodeFixAsync(source, fixedSource);
    }
}
