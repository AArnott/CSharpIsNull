# CSIsNull001

This analyzer flags use of `== null` to test whether a value is `null`.

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

Pattern syntax cannot invoke an overloaded equality operator. It also lets the compiler reject null checks against non-nullable value types.