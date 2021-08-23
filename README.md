# C# null test syntax analyzers

[![NuGet package](https://img.shields.io/nuget/v/CSharpIsNullAnalyzer.svg)](https://nuget.org/packages/CSharpIsNullAnalyzer)
[![NuGet package](https://img.shields.io/badge/nuget-From%20CI-yellow)](https://dev.azure.com/andrewarnott/OSS/_packaging?_a=package&feed=PublicCI&package=CSharpIsNullAnalyzer&version=0.1.278-beta&protocolType=NuGet)
[![Build Status](https://dev.azure.com/andrewarnott/OSS/_apis/build/status/CSharpIsNull?branchName=main)](https://dev.azure.com/andrewarnott/OSS/_build/latest?definitionId=54&branchName=main)

## Features

* Guard against bugs from testing structs against `null`.
* Bulk code fix will update all your code at once.

### Analyzers

* [CSIsNull001](doc/analyzers/CSIsNull001.md) to catch uses of `== null`
* [CSIsNull002](doc/analyzers/CSIsNull002.md) to catch uses of `!= null`

## Consumption

Install it via NuGet through the nuget badge at the top of this file.

### Consume from CI

To get the very latest analyzer [from my CI feed](https://dev.azure.com/andrewarnott/OSS/_packaging?_a=package&feed=PublicCI&package=CSharpIsNullAnalyzer&protocolType=NuGet):

[Connect to the feed](https://dev.azure.com/andrewarnott/OSS/_packaging?_a=connect&feed=PublicCI):

```xml
<add key="PublicCI" value="https://pkgs.dev.azure.com/andrewarnott/OSS/_packaging/PublicCI/nuget/v3/index.json" />
```

Then install the package with this command in your Package Manager Console:

```ps1
Install-Package CSharpIsNullAnalyzer -pre
```
