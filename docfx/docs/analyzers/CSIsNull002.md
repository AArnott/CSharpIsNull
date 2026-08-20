# CSIsNull002

This analyzer flags use of `!= null` to test whether a value is not `null`.

For example, this code produces a diagnostic:

```csharp
if (value != null)
{
}
```

The code fix replaces the inequality check with pattern syntax:

```csharp
if (value is object)
{
}
```

Pattern syntax cannot invoke an overloaded inequality operator. It also lets the compiler reject null checks against non-nullable value types.
