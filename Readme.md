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

# Credits

## Contributers

<h4 align="center">

|   User                     | Emojis                                        |
|:---------------------------|:----------------------------------------------|
|   HarryET                  | :tada: :sparkles: :bookmark: :bug:            |

</h4>

## Key

<h4 align="center">

|   Commit type              | Emoji                                         |
|:---------------------------|:----------------------------------------------|
| Initial commit             | :tada: `:tada:`                               |
| Version tag                | :bookmark: `:bookmark:`                       |
| New feature                | :sparkles: `:sparkles:`                       |
| Bugfix                     | :bug: `:bug:`                                 |
| Metadata                   | :card_index: `:card_index:`                   |
| Documentation              | :books: `:books:`                             |
| Documenting source code    | :bulb: `:bulb:`                               |
| Performance                | :racehorse: `:racehorse:`                     |
| Cosmetic                   | :lipstick: `:lipstick:`                       |
| Tests                      | :rotating_light: `:rotating_light:`           |
| Adding a test              | :white_check_mark: `:white_check_mark:`       |
| Make a test pass           | :heavy_check_mark: `:heavy_check_mark:`       |
| General update             | :zap: `:zap:`                                 |
| Improve format/structure   | :art: `:art:`                                 |
| Refactor code              | :hammer: `:hammer:`                           |
| Removing code/files        | :fire: `:fire:`                               |
| Continuous Integration     | :green_heart: `:green_heart:`                 |
| Security                   | :lock: `:lock:`                               |
| Upgrading dependencies     | :arrow_up: `:arrow_up:`                       |
| Downgrading dependencies   | :arrow_down: `:arrow_down:`                   |
| Lint                       | :shirt: `:shirt:`                             |
| Translation                | :alien: `:alien:`                             |
| Text                       | :pencil: `:pencil:`                           |
| Critical hotfix            | :ambulance: `:ambulance:`                     |
| Deploying stuff            | :rocket: `:rocket:`                           |
| Fixing on MacOS            | :apple: `:apple:`                             |
| Fixing on Linux            | :penguin: `:penguin:`                         |
| Fixing on Windows          | :checkered_flag: `:checkered_flag:`           |
| Work in progress           | :construction:  `:construction:`              |
| Adding CI build system     | :construction_worker: `:construction_worker:` |
| Analytics or tracking code | :chart_with_upwards_trend: `:chart_with_upwards_trend:` |
| Removing a dependency      | :heavy_minus_sign: `:heavy_minus_sign:`       |
| Adding a dependency        | :heavy_plus_sign: `:heavy_plus_sign:`         |
| Docker                     | :whale: `:whale:`                             |
| Configuration files        | :wrench: `:wrench:`                           |
| Package.json in JS         | :package: `:package:`                         |
| Merging branches           | :twisted_rightwards_arrows: `:twisted_rightwards_arrows:` |
| Bad code / need improv.    | :hankey: `:hankey:`                           |
| Reverting changes          | :rewind: `:rewind:`                           |
| Breaking changes           | :boom: `:boom:`                               |
| Code review changes        | :ok_hand: `:ok_hand:`                         |
| Accessibility              | :wheelchair: `:wheelchair:`                   |
| Move/rename repository     | :truck: `:truck:`                             |
| Other                      | [Be creative](http://www.emoji-cheat-sheet.com/)  |

</h4>
