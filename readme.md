## Scope functions

Functions `Map`, `Apply`, `ApplyForEach`, `ApplyForEachEager`, `WrapException` allow you to have side-effects or manipulate objects in fluent invocation chains or object initializers.
 
For example: 

```csharp
User manager = null;
Department department = new Department
{
    // before, you would have to do something like
    // ManagerTitle = user == null ? null : GetUserTitle(user)
    ManagerTitle = manager?.Map(GetUserTitle)
    Budget = await 
        repo.WrapExceptionAsync(
            r => r.GetBudgets(budgetCode).Where(x => x.Year == DateTime.Now.Year).SingleAsync(),
            (_, ex) => throw new DepartmentCreationException($"Could not determine budget for code {budgetCode}", ex))
        .ContinueWith(budgetTask => budgetTask.Result.Apply(ValidateBudget))
    // ....
}
```

These functions are especially useful when setting up various context objects: 

```csharp
var permissionsMissingTestContext = new TestContext
{
    Users = GetStandardUsers().Where(x => x.IsActive).ApplyForEach(x => x.Permissions = new Permission[0]).ToArray(),
    Departments = GetStandardDepartments().ApplyForEachEager(x => Console.WriteLine(x.name)),
    SecurityPolicy = GetStandardSecurityPolicy().Apply(x => x.ShowWarningWhenPermissionsMissing = true),
    AnonymousPageUrl = GetStandardConfig().Map(x => new Url(x.PermissionsMissingScreenUrl)),
    Timeout = GetTestTimeout()?.Map(x => TimeSpan.FromSeconds(x)) ?? TimeSpan.FromSeconds(30)
}
```