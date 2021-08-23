// Copyright (c) Andrew Arnott. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using CSharpIsNullAnalyzer;
using Xunit;
using VerifyCS = CSharpCodeFixVerifier<CSharpIsNullAnalyzer.CSIsNull002, CSharpIsNullAnalyzer.CSIsNull002Fixer>;

public class CSIsNull002Tests
{
    [Fact]
    public async Task NotEqualsNullInExpressionBody_ProducesDiagnostic()
    {
        string source = @"
class Test
{
    bool Method(object o) => o [|!= null|];
}";

        string fixedSource1 = @"
class Test
{
    bool Method(object o) => o is object;
}";

        string fixedSource2 = @"
class Test
{
    bool Method(object o) => o is not null;
}";

        await VerifyCS.VerifyCodeFixAsync(source, fixedSource1, CSIsNull002Fixer.IsObjectEquivalenceKey);
        await VerifyCS.VerifyCodeFixAsync(source, fixedSource2, CSIsNull002Fixer.IsNotNullEquivalenceKey);
    }

    [Fact]
    public async Task NullNotEqualsInExpressionBody_ProducesDiagnostic()
    {
        string source = @"
class Test
{
    bool Method(object o) => [|null !=|] o;
}";

        string fixedSource1 = @"
class Test
{
    bool Method(object o) => o is object;
}";

        string fixedSource2 = @"
class Test
{
    bool Method(object o) => o is not null;
}";

        await VerifyCS.VerifyCodeFixAsync(source, fixedSource1, CSIsNull002Fixer.IsObjectEquivalenceKey);
        await VerifyCS.VerifyCodeFixAsync(source, fixedSource2, CSIsNull002Fixer.IsNotNullEquivalenceKey);
    }

    [Fact]
    public async Task NotEqualsNullInIfExpression_ProducesDiagnostic()
    {
        string source = @"
class Test
{
    void Method(object o)
    {
        if (o [|!= null|])
        {
        }
    }
}";

        string fixedSource1 = @"
class Test
{
    void Method(object o)
    {
        if (o is object)
        {
        }
    }
}";

        string fixedSource2 = @"
class Test
{
    void Method(object o)
    {
        if (o is not null)
        {
        }
    }
}";

        await VerifyCS.VerifyCodeFixAsync(source, fixedSource1, CSIsNull002Fixer.IsObjectEquivalenceKey);
        await VerifyCS.VerifyCodeFixAsync(source, fixedSource2, CSIsNull002Fixer.IsNotNullEquivalenceKey);
    }

    [Fact]
    public async Task NullNotEqualsInIfExpression_ProducesDiagnostic()
    {
        string source = @"
class Test
{
    void Method(object o)
    {
        if ([|null !=|] o)
        {
        }
    }
}";

        string fixedSource1 = @"
class Test
{
    void Method(object o)
    {
        if (o is object)
        {
        }
    }
}";

        string fixedSource2 = @"
class Test
{
    void Method(object o)
    {
        if (o is not null)
        {
        }
    }
}";

        await VerifyCS.VerifyCodeFixAsync(source, fixedSource1, CSIsNull002Fixer.IsObjectEquivalenceKey);
        await VerifyCS.VerifyCodeFixAsync(source, fixedSource2, CSIsNull002Fixer.IsNotNullEquivalenceKey);
    }
}
