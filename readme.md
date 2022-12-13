## Scope functions

Functions `Apply`, `ApplyForEach` and `Map` allow you to have side-effects or manipulate objects in fluent invocation chains or object initializers.
 
```csharp
var permissionsMissingTestContext = new TestContext
{
    Users = GetStandardUsers().ApplyForEach(x => x.Permissions = new[0]),
    SecurityPolicy = GetStandardSecurityPolicy().Apply(x => x.ShowWarningWhenPermissionsMissing = true),
    AnonymousPageUrl = GetStandardConfig().Map(x => new Url(x.PermissionsMissingScreenUrl)),
    Timeout = GetTestTimeout()?.Map(x => TimeSpan.FromSeconds(x)) ?? TimeSpan.FromSeconds(30)
}
```