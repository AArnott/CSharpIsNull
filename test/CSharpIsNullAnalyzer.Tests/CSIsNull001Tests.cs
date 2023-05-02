// Copyright (c) Andrew Arnott. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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

    [Fact]
    public async Task EqualsNull_NearExpressionTreeAssignment_ProducesDiagnostic()
    {
        string source = @"
using System;
using System.Linq.Expressions;

class Test
{
    void Method()
    {
        Expression<Func<string, bool>> e = """" [|== null|] ? (s => s == null) : (Expression<Func<string, bool>>)null;
    }
}";

        string fixedSource = @"
using System;
using System.Linq.Expressions;

class Test
{
    void Method()
    {
        Expression<Func<string, bool>> e = """" is null ? (s => s == null) : (Expression<Func<string, bool>>)null;
    }
}";

        await VerifyCS.VerifyAnalyzerAsync(source);
        await VerifyCS.VerifyCodeFixAsync(source, fixedSource);
    }

    [Fact]
    public async Task EqualsNullInExpressionTree_ProducesNoDiagnostic()
    {
        string source = @"
using System;
using System.Linq.Expressions;

class Test
{
    void Method()
    {
        _ = (Expression<Func<string, bool>>)(s => s == null);
    }
}";

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public async Task NullEqualsInExpressionTree_ProducesNoDiagnostic()
    {
        string source = @"
using System;
using System.Linq.Expressions;

class Test
{
    void Method()
    {
        _ = (Expression<Func<string, bool>>)(s => null == s);
    }
}";

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public async Task EqualsNullInExpressionTree_TargetTyped_ProducesNoDiagnostic()
    {
        string source = @"
using System;
using System.Linq.Expressions;

class Test
{
    void Method()
    {
        Expression<Func<string, bool>> e = s => s == null;
    }
}";

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public async Task EqualsDefaultInIfExpression_ProducesDiagnostic()
    {
        string source = @"
class Test
{
    void Method(object o)
    {
        if (o [|== default|])
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
    public async Task EqualsDefaultValueType_ProducesNoDiagnostic()
    {
        string source = @"
class Test
{
    void Method(int o)
    {
        if (o == default)
        {
        }
    }
}";

        await VerifyCS.VerifyAnalyzerAsync(source);
    }
}
