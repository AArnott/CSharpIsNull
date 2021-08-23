# C# null test syntax analyzers

[![NuGet package](https://img.shields.io/nuget/v/CSharpIsNullAnalyzer.svg)](https://nuget.org/packages/CSharpIsNullAnalyzer)
[![Build Status](https://dev.azure.com/andrewarnott/OSS/_apis/build/status/CSharpIsNull?branchName=main)](https://dev.azure.com/andrewarnott/OSS/_build/latest?definitionId=54&branchName=main)

## Features

* Guard against bugs from testing structs against `null`.
* Bulk code fix will update all your code at once.

## Consumption

Until the analyzer is pushed to nuget.org in some form, you can [get it from my CI feed](https://dev.azure.com/andrewarnott/OSS/_packaging?_a=package&feed=PublicCI&package=CSharpIsNullAnalyzer&protocolType=NuGet):

[Connect to the feed](https://dev.azure.com/andrewarnott/OSS/_packaging?_a=connect&feed=PublicCI):

```xml
<add key="PublicCI" value="https://pkgs.dev.azure.com/andrewarnott/OSS/_packaging/PublicCI/nuget/v3/index.json" />
```

Then install the package with this command in your Package Manager Console:

```ps1
Install-Package CSharpIsNullAnalyzer -pre
```
