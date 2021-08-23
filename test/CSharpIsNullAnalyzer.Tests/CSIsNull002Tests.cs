// Copyright (c) Andrew Arnott. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
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

        string fixedSource = @"
class Test
{
    bool Method(object o) => o is object;
}";

        await VerifyCS.VerifyCodeFixAsync(source, fixedSource);
    }

    [Fact]
    public async Task NullNotEqualsInExpressionBody_ProducesDiagnostic()
    {
        string source = @"
class Test
{
    bool Method(object o) => [|null !=|] o;
}";

        string fixedSource = @"
class Test
{
    bool Method(object o) => o is object;
}";

        await VerifyCS.VerifyCodeFixAsync(source, fixedSource);
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

        string fixedSource = @"
class Test
{
    void Method(object o)
    {
        if (o is object)
        {
        }
    }
}";

        await VerifyCS.VerifyCodeFixAsync(source, fixedSource);
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

        string fixedSource = @"
class Test
{
    void Method(object o)
    {
        if (o is object)
        {
        }
    }
}";

        await VerifyCS.VerifyCodeFixAsync(source, fixedSource);
    }
}
