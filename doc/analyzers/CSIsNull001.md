# CSIsNull001

This analyzer flags use of `== null` to test whether a value is `null`.

For example this code would produce a diagnostic:

```cs
if (o == null)
{
}
```

A code fix is offered to automate the fix, which is to use pattern syntax instead:

```cs
if (o is null)
{
}
```

Pattern syntax is preferred because if `o` is typed as a struct, the compiler will report an error when testing it for `null`, which a struct can never be.
