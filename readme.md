# Nimator [![(AppVeyor status...)](https://ci.appveyor.com/api/projects/status/ghh8tjguwyb9wpru?svg=true)](https://ci.appveyor.com/project/JeroenHeijmans/nimator)

A light-weight framework for creating an application to monitor your systems. You to write checks for your systems in C#, and the framework takes care of (nearly all) the rest.

You are looking at a very early version of this codebase. Even though we are dogfooding this code, please consider it as pre-alpha code at this moment.

## Why Another Monitoring Tool?

There's a plethora of monitoring and alerting tools (both commercial and open source) out there already: [Nagios](https://www.nagios.org/), [Bosun](https://bosun.org/), [ElasticSearch Marvel](https://www.elastic.co/products/marvel), and many, many more. So why create a new one?

Nimator is meant to be a light-weight framework for creating small monitoring tools, where you can write the code for checking parts of your system using C# (or VB.NET, if that's your thing). It is not *meant* to be competitive with the bigger, fully-fledged tools: if you have a large organization you're probably better off with one of the alternatives. However, if you are a small .NET shop, fluent in C#, and want to write a few system-monitoring checks in your favorite language: Nimator might be useful to you.

We mostly wrote this because we needed it ourselves (so you can count on this code being run in production in at least one place). We open sourced it because it's not so much our core business, and it might be of use to someone else too.

So, want to write a few lines of C# to periodically monitor your systems, and not have to worry about aggregating, exception handling, distribution, etc? Read on!

## Full Docs

Additional documentation is a work in progress, to be found in [the wiki](../../wiki). But you can also read on here, with the [Getting Started](#getting-started) section.

## Getting Started

### Semi-Quick Start

1. Create a new Console Application project
2. Run `Install-Package Nimator` in the Package Manager Console
3. Copy the contents of [the example `class Program {...}`](/Nimator.ExampleConsoleApp/Program.cs) over your blank `Program`
4. Create an file `config.json` next to `Program.cs`
5. Set `config.json` to be an *embedded resource*
6. Copy the contents of [the example `config.json`](/Nimator.ExampleConsoleApp/config.json) into that file
7. Either configure `log4net` analogous to [the example `App.config`](/Nimator.ExampleConsoleApp/App.config), or comment out the `Configure()` call.
8. Compile and run!

You should now see something along these lines:

> ```
> Creating Nimator.
> Nimator created. Starting timer for cycle every 15 seconds.
> Press any key to exit.
> ERROR between 21:25:56.114 and 21:25:56.140 (on 2016-09-26)
> Failure in [Demo Layer 2], checks [AlwaysError].
> - Warning: after running 2 check(s) in Demo Layer 1
> - - Okay in AlwaysOkay: no details provided
> - - Warning in AlwaysWarning: no details provided
> - Error: after running 1 check(s) in Demo Layer 2
> - - Error in AlwaysError: no details provided
> ```

Congratulations! You've successfully run a meaningless moninitoring cycle!

### Your First check

Writing checks is quite easy. You have to do three things to get up and running:

1. Create an instance of `ICheck`. Here's a trivial example:

    ```csharp
    class ImportantFileCheck : ICheck
    {
        private readonly string folder;

        public string ShortName { get; } = nameof(ImportantFileCheck);

        public ImportantFileCheck(string folder)
        {
            this.folder = folder;
        }

        public Task<ICheckResult> RunAsync()
        {
            var level = File.Exists($"{folder}/important-file-{DateTime.Now.ToString("yyyy-MM-dd")}.txt")
                ? NotificationLevel.Okay
                : NotificationLevel.Error;

            return Task.FromResult<ICheckResult>(new CheckResult(ShortName, level));
        }
    }
    ```

   Note that even though the `ICheck` interface asks you to try and be `async`, you can just as well return synchronously with `Task.FromResult<T>(...)`.

2. Create a corresponding instance of `ICheckSettings`:

    ```csharp
    class ImportantFileCheckSettings : ICheckSettings
    {
        public string Folder { get; set; }

        public ICheck ToCheck()
        {
            return new ImportantFileCheck(Folder);
        }
    }
    ```

3. Add configuration for your check to the `config.json` file (to any of the `Checks` arrays in a layer):

    ```json
    {
      "$type": "ConsoleApplication1.ImportantFileCheckSettings, ConsoleApplication1",
      "Folder": "c:/some/important/folder/"
    }
    ```

   Note that you should replace `ConsoleApplication1` with your application's Namespace.

That's it! If you now run the application again, the check will periodically look if the important log file is available on disk.

### Some more ideas

Okay, the above example is *moderately* useful at best. But you can do better. Here's a few ideas to get you started with other checks:

- Ping a certain host to see if it's available
- Try to do a `WebRequest` or two to check up on your application
- Run a query in your database to check for simple anomalies
- Query your logs for recent spikes in errors, etc.

Careful: the most interesting checks are often the ones that would fail on a timeout. Be sure to set a proper timeout when using third party libraries (e.g. a Connection Timeout when querying your database) and fail your `ICheck` on a timeout.

## NuGet

Nimator is [listed on NuGet as a package](https://www.nuget.org/packages/Nimator/). Again, note that this is a very early version, use at your own risk.

```
Install-Package Nimator
```

The above command will get you the latest version. Please note that we will not move to SemVer until a "version 1" has crystallized, and until then any new package versions might include breaking changes.

## License

Code in this repository is available under [the MIT license](license.md), except when explicitly noted otherwise (e.g. when the source code includes a method that has a comment linking to Stack Overflow source post, which entails CC-BY-SA 3.0 with attribution required).

## Questions

If you have an *issue* (bug, feature request, etc), please report it on GitHub. If you have a question about this project or its use, please contact the main committer on nimator-at-jeroenheijmans-dot-nl or via Twitter @jeroenheijmans.
