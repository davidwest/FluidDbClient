# FluidDbClient
When you absolutely need to work with ADO.NET but want to ditch 95% of the boilerplate code. Perfect fit if you need to run explicit SQL and/or map raw data to complex objects that you won't trust to your ORM. Intuitively parameterize queries and commands, stream records, process multiple result sets with ease, batch operations into a single transaction, build scripts with dynamically-generated parameters, and if you wish, do things asynchronously.  It's fast and super lightweight.

## The Nuget Package

Currently, only a Sql Server provider exists for FluidDbClient.

[Get the Sql Server provider Nuget package.](https://www.nuget.org/packages/FluidDbClient.Sql/) 


## The Sandbox Demo Project
If you want to see FluidDbClient in action, it's recommended that you open **FluidDbClient.sln** in Visual Studio.
Set **Sandbox** as the startup project.  Attach the included **Acme.mdf** Sql Server database file to a locally running Sql Server instance.

Go to the **DemoRunner** class. All demo invocations are listed, one per line.  Run any demo you'd like and go to the associated code to see what's going on.

## Getting Started



