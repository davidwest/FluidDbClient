# FluidDbClient
When you absolutely need to work with ADO.NET but want to ditch 95% of the boilerplate code. Perfect fit if you need to run explicit SQL and/or map raw data to complex objects that you won't trust to your ORM. Intuitively parameterize queries and commands, stream records, process multiple result sets with ease, batch operations into a single transaction, build scripts with dynamically-generated parameters, and if you wish, do things asynchronously.  It's fast and super lightweight.

The libraries **FluidDbClient** and **FluidDbClient.Sql** target the following frameworks:

- .NET Framework 4.5, 4.5.1
- .NET Standard 1.3

## The Nuget Package

Currently, only a Sql Server provider exists for FluidDbClient.

[Get the Sql Server provider Nuget package.](https://www.nuget.org/packages/FluidDbClient.Sql/) 


## The Sandbox Demo Project
If you want to see FluidDbClient in action, it's recommended that you open **FluidDbClient.sln** in Visual Studio.
Set **Sandbox** as the startup project.  Attach the included **Acme.mdf** Sql Server database file to a locally running Sql Server instance.

Go to the **DemoRunner** class. All demo invocations are listed, one per line.  Run any demo you'd like and view the associated code to see what's going on.

## Getting Started
All databases in you application need to be registered at startup.  This is accomplished in 2 quick steps:

1. Define a database class by inheriting from `Database`. (Do this in your data access layer)
2. Register it with the static `DbRegistry` class. (Do this in your application root)

###Example
Suppose you have 2 databases.  In your data access layer, you could define them like this:

```
public class AcmeDb : Database
{
    public AcmeDb(string connectionString, Action<string> log = null) 
        : base("Acme Database", connectionString, new SqlDbProvider(), log)
    { }
}

public class NorthwindDb : Database
{
    public NorthwindDb(string connectionString, Action<string> log = null) 
        : base("Northwind Database", connectionString, new SqlDbProvider(), log)
    { }
}
```

> The optional `log` parameter allows you to supply a delegate for implementing run-time instrumentation. Runtime information includes when resources (DbConnections, DbTransactions, DbCommands, DbReaders) are being created and disposed. It also includes when connections are being opened and when transactions are committed or rolled back.

Then, in your application root during startup, you could do something like this:

```
var acmeDb = new AcmeDb(Config["AcmeConnectionString"], msg => Debug.WriteLine(msg));
var northwindDb = new NorthwindDb(Config["NorthwindConnectionString]);

DbRegistry.Register(acmeDb, northwindDb);
```

**Important** : by convention, the first database registered is considered the *default database*. When using queries and commands that target the *default database* (see below), a type parameter is not necessary.

And that's it.  Once `DbRegistry.Register` is invoked, it cannot be called again while the application is running.

##Queries and Commands
FluidDbClient divides operations into *queries* and *commands* (see interfaces `IManagedDbQuery` and `IManagedDbCommand`).
There are 4 concrete classes that expose all functionality:

1. `ScriptDbQuery<TDatabase>` : specify a SQL script to return data
2. `ScriptDbCommand<TDatabase>` : specify a SQL script to execute a command
3. `StoredProcedureDbQuery<TDatabase>` : specify a stored procedure to return data
4. `StoredProcedureDbCommand<TDatabase>` : specify as stored procedure to execute a command

**Important** : The type parameter is only required if you are *not* targeting the *default database* (see above).  If you *are* targeting the *default database*, the type parameter can be dropped.

Command operations run atomically (inside their own transaction) unless a `DbSession` instance is specified in the command object's constructor.  In this case, the command operation takes on the transaction defined by the `DbSession` object. Queries can also be constructed with a `DbSession` instance, allowing them to participate in the atomic session.

Please see the included **Sandbox** console project for examples of all these classes in action.

##Shell Wrapper Methods
To keep code terse, you can use one of the many built-in "shell" wrapper methods.  These create query or command objects on your behalf and use the supplied parameters to perform the operation.

###Examples

Creates and invokes method on `ScriptDbQuery` targeting the default database:
```
var widgets = Db.GetResultSet("SELECT * FROM Widget")
              .Select(record => record.MapToWidget());
```


Creates and invokes method on `StoredProcedureDbQuery<NorthwindDb>`:
```
var applications = DbProc<NorthwindDb>.GetResultSet("GetApplications")
                   .Select(record => record.MapToApplication());
```


Creates and invokes async method on `ScriptDbCommand` targeting the default database:
```
await Db.ExecuteAsync("UPDATE Robot SET Name = @name WHERE id = @id", 
                      new {name = "Terminator", id = 6});
```

Creates and invokes method on `StoredProcedureDbCommand<NorthwindDb>`
```
DbProc<NorthwindDb>.Execute(IsolationLevel.Snapshot, "UpdateApplicationDate", 
                            new {id = 100, date = DateTime.Now);
```

###When *not* to use Shell Wrapper Methods
Most functionality is exposed through the shell wrapper methods, but if you prefer to be explicit, by all means "new up" a query or command.

A handy diagnostic method exists for all concrete query and command objects: `.ToDiagnosticString()`.  This allows you to see the current "specification" state of the query or command, including parameters.  Obviously, this functionality is hidden when using the shell wrapper methods.

**Important**: `DbScriptQuery<TDatabase>` and `DbScriptCommand<TDatabase>` objects must be explicitly constructed (i.e. cannot use shell wrapper methods) if you plan on including complex scripts with dynamically-generated parameters built with a `DbScriptCompiler<TDatabase>`.

Please see the included **Sandbox** console project for examples of shell methods and usage of the `DbScriptCompiler<TDatabase>` class.


##More Help
More information will be added to this document as the need arises. Feel free to contact me at dwest.netdev@gmail.com if you have a burning question or concern.