# SweIdNum [![Build status](https://ci.appveyor.com/api/projects/status/feqy2yo8k2738kub/branch/master?svg=true)](https://ci.appveyor.com/project/wallymathieu/swe-id-num/branch/master) [![Build Status](https://travis-ci.org/wallymathieu/swe-id-num.svg?branch=master)](https://travis-ci.org/wallymathieu/swe-id-num) [![NuGet](http://img.shields.io/nuget/v/SweIdNum.svg)](https://www.nuget.org/packages/SweIdNum/)

Validate swedish identity numbers

## Installation

Add the `GlobalPhone` nuget package to your app. For example, using Package Manager Console:

```
PM> Install-Package SweIdNum
```

## Examples

### `c#`

First make sure to import the relevant namespaces

```
using SweIdNum;
using SweIdNum.Core;
```

Then you can parse swedish personal identity numbers

```
try{
  var tolvansson = PersonalIdentityNumbers.Parse("121212-1212");
  var birthdate = tolvansson.GetDate();
  var controlNumber = tolvansson.GetControlNumber();
} catch (DoesNotMatchFormatException){
  // ...
} catch (InvalidChecksumException){
  // ...
}
```

If you don't want to handle potential exceptions then you use the TryParse method

```
if (PersonalIdentityNumbers.TryParse("121212-1212", out tolvansson)) 
{
  // to stuff
}
```

### `f#`

First make sure to import the relevant namespaces. In the code below we import the module `PersonalIdentityNumbers` for brevity. 
```
open SweIdNum;
open SweIdNum.Core.PersonalIdentityNumbers
```

Note that the parse below will throw an exception if you feed it an invalid value

```
let tolvansson = parse "121212-1212"
let birthdate = getDate tolvansson 
var controlNumber = getControlNumber tolvansson
```

If you are unsure if it's correct and want to handle it in a more f#y way, then use tryParse 

```
let maybeTolvansson = tryParse "121212-1212"
match maybeTolvansson with
| Choice1Of2 tolvansson -> 
    //...
| Choice2Of2 err ->
    match err with
    | DoesNotMatchFormat -> //...
    | InvalidChecksum (expected,actual) -> //...
```


## Influences

[R package sweidnumbr](https://github.com/rOpenGov/sweidnumbr)
