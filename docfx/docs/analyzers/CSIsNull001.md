# CSIsNull001

| Property | Value |
| --- | --- |
| Title | Use `is null` for null checks |
| Category | Usage |
| Default severity | Info |

This analyzer reports a diagnostic when C# code uses the built-in `==` operator to compare a reference or nullable value with `null`. Comparisons inside expression trees are excluded because expression trees may not support the equivalent pattern syntax.

Pattern syntax cannot invoke an overloaded equality operator. It also lets the compiler reject null checks against non-nullable value types.

## Example

For example, this code produces a diagnostic:

```csharp
if (value == null)
{
}
```

The code fix replaces the equality check with pattern syntax:

```csharp
if (value is null)
{
}
```

The code fix replaces the comparison with `is null` and supports Fix All.

## Configuration and suppression

The rule has no custom options. Configure its severity with the standard `.editorconfig` key `dotnet_diagnostic.CSIsNull001.severity`, or suppress a specific occurrence with standard pragma or `SuppressMessage` mechanisms.
