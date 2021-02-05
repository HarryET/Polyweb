<h1 align="center">
  <br>
  <img src="./icon.png" alt="Polyweb Icon" width="256"></img>
  <br>
    Polyweb
  <br>
  <br>
</h1>
<br>
<h4 align="center">Fast, 0 Dependency Http For Everyone.</h4>
<br>
<p align="center">
  <a href="https://discord.gg/VWQfY2jn86"><img src="https://canary.discord.com/api/guilds/807179133569335328/widget.png?style=shield" alt="Discord Server Invite"/></a>
  <img src="https://img.shields.io/badge/License-GPLv3-blue.svg" alt="GPL v3 Licence"/>
  <a href="https://www.nuget.org/packages/Polyweb/"><img alt="Nuget (with prereleases)" src="https://img.shields.io/nuget/vpre/Polyweb"></a>
</p>
<br>
<p align="center">
  <a href="#Features">Features</a> •
  <a href="#Roadmap">Roadmap</a> •
  <a href="#Documentation">Documentation</a> •
  <a href="#Installing">Installing</a> •
  <a href="#Example">Example</a> •
  <a href="#credits">Credits</a>
</p>
<br>
<hr>

# Features

* Controllers
    * Routing
        * Params
        * Cookies
    * GET, POST, PUT, PATCH, DELETE
    * Json & Plain Text Responses
* Custom Services

# Roadmap

# Documentation

Poly web currently dosen't have any documentation. I would like to do this soon once the core features are done.

# Installing

## NuGet

### Install-Package

```
Install-Package Polyweb
```

### .Net CLI

```
dotnet add package Polyweb
```

### Package Reference

Make sure to fill in the version with the latest version from NuGet

```xml
<PackageReference Include="Polyweb" Version="" />
```

### Paket CLI

Make sure to fill in the version with the latest version from NuGet

```
paket add Polyweb
```

## BaGet

Add the BaGet Index (`http://harryet.me/v3/index.json`) as a source for NuGet, then follow the Nuget Instructions

# Example
```cs
namespace Polyweb.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            PolywebServer polyweb = new PolywebServer();
            
            polyweb.RegisterProvider<DataProvider>();
            polyweb.RegisterProvider<AuthenticationProvider>();
            
            polyweb.RegisterService<IServiceName, ServiceName>();
            
            polyweb.RegisterController<Controllers.Index>();
            
            polyweb.Listen(8080);
        }
    }
}
```
