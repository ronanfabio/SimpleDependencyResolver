# SimpleDependencyResolver

A lightweight library for dependency registration using annotation attributes in .NET.

## 📝 Description

SimpleDependencyResolver enables dependency registration through attributes, simplifying your DI container setup. The project is split into two packages:

- **SimpleDependencyResolver**: Core library with resolver implementation
- **SimpleDependencyResolver.Abstractions**: Contains only attributes, suitable for class libraries without unnecessary dependencies

## ✨ Features

- Automatic dependency registration
- Support for different lifetimes:
    - Transient
    - Scoped
    - Singleton
- Keyed services support for .NET 8+
- Compatible with .NET Standard 2.1 and higher
- Separated abstractions for class libraries

## 🚀 Installation

For projects implementing services:
```sh
dotnet add package SimpleDependencyResolver.Abstractions
```

For projects that need to register dependencies:
```sh
dotnet add package SimpleDependencyResolver
```

## 📋 Usage

1. In your class library, add the abstractions reference and use attributes:

```csharp
using SimpleDependencyResolver.Attributes;

[Scoped]
public class MyService : IMyService
{
    public string GetData() => "Hello from MyService";
}
```

2. In your startup project, add the main reference and register the resolver:

```csharp
using SimpleDependencyResolver;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSimpleDependencyResolver();
```

3. Alternatively, you can register the resolver with a specific assembly:

```csharp
using SimpleDependencyResolver;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSimpleDependencyResolver(Assembly.GetExecutingAssembly());
```

### Using Keyed Services (.NET 8+)

```csharp
[Scoped(ServiceKey = "primary")]
public class PrimaryService : IMyService
{
    public string GetData() => "Primary Service";
}

[Scoped(ServiceKey = "secondary")]
public class SecondaryService : IMyService
{
    public string GetData() => "Secondary Service";
}

// Inject using IKeyedServiceProvider
public class Consumer
{
    private readonly IMyService _primaryService;
        
    public Consumer([FromKeyedServices("primary")] IMyService service)
    {
        _primaryService = service;
    }
}
```

## 🧩 Examples

Check the `samples/SampleApi` project for a complete implementation example.

## 📄 License

This project is licensed under the Apache License 2.0.