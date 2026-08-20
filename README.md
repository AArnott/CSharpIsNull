# C# null test syntax analyzers

[![NuGet package](https://img.shields.io/nuget/v/CSharpIsNullAnalyzer.svg)](https://nuget.org/packages/CSharpIsNullAnalyzer)
[![Build Status](https://github.com/AArnott/CSharpIsNull/actions/workflows/build.yml/badge.svg)](https://github.com/AArnott/CSharpIsNull/actions/workflows/build.yml)

## Features

* Guard against bugs from testing structs against `null`.
* Bulk code fix will update all your code at once.

### Analyzers

* [CSIsNull001](https://aarnott.github.io/CSharpIsNull/docs/analyzers/CSIsNull001.html) to catch uses of `== null`
* [CSIsNull002](https://aarnott.github.io/CSharpIsNull/docs/analyzers/CSIsNull002.html) to catch uses of `!= null`

## Consumption

Install it via NuGet through the nuget badge at the top of this file.

Then install the package with this command in your Package Manager Console:

```ps1
Install-Package CSharpIsNullAnalyzer -pre
```
