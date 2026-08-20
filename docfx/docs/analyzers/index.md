# Analyzer rules

CSharpIsNull reports null checks that use equality operators and offers code fixes that use pattern syntax instead.

| Rule | Description |
| --- | --- |
| [CSIsNull001](CSIsNull001.md) | Replace `== null` with `is null`. |
| [CSIsNull002](CSIsNull002.md) | Replace `!= null` with a non-null pattern. |
