# Getting Started

## Installation

Install the analyzer package in the projects you want to check:

```xml
<PackageReference Include="CSharpIsNullAnalyzer" Version="VERSION" PrivateAssets="all" />
```

Choose `VERSION` from the [CSharpIsNullAnalyzer package page](https://www.nuget.org/packages/CSharpIsNullAnalyzer).

## Usage

The analyzers report equality and inequality comparisons with `null`. Apply the supplied code fixes to replace them with pattern syntax, including a bulk fix across the project or solution.

See [Analyzer rules](analyzers/index.md) for examples and details.
