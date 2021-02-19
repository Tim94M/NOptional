# NOptional ![Build](https://github.com/Tim94M/NOptional/workflows/Build/badge.svg)

NOptional is a very close clone of Java's Optional type ported to .NET. For further references, see [Java documentation](https://devdocs.io/openjdk~15/java.base/java/util/optional). I aimed for a very close implementation, so the original documentation can be used for details.

## Usage

### Creation

Use either `Optional.Empty<T>` or `Optional.Of<T>(T value)` to create a new Instance of your Optional. 

```c#
IOptional<string> emptyOptional = Optional.Empty<string>();
IOptional<string> filledOptional = Optional.Of("MyString");
```

When you are using this in a scenario where you don't know, whether you actually have a value or not, use `Optional.OfNullable<T>(T value)`
```c#

private IOptional<T> Create<T>(T value)
{
  return Optional.OfNullable(value);
}

```
### Accessing Value of Optional

Never access `IOptional.Value` without prior checking whether a value is preset using `IOptional.HasValue()`. Ideally, you would never use the property-accessor directly. Instead, use any of the `IOptional.Or...` methods.

### General usage

To quote the original documentation:

> `Optional` is primarily intended for use as a method return type where there is a clear need to represent "no result," and where using null is likely to cause errors. A variable whose type is Optional should never itself be null; it should always point to an Optional instance.

## Installation

NOptional is available via nuget: [NOptional via nuget](https://www.nuget.org/packages/NOptional/) 

You can use either Package Manager or .NET CLI to install it

Package Manager:
```
Install-Package NOptional -Version LATEST_VERSION
```

.NET CLI:
```
dotnet add package NOptional --version LATEST_VERSION
```
