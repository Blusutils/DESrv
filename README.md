# DESrv v2.0 <img src="./DESrvLogo.svg" align="center" width="100">

Dedicated Extensible Server that deserves your attention!

[![GitHub stars](https://img.shields.io/github/stars/Blusutils/DESrv?label=Stars&style=flat-square)](https://github.com/Blusutils/DESrv/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/Blusutils/DESrv?label=Forks&style=flat-square)](https://github.com/Blusutils/DESrv/network/members)
[![GitHub all releases](https://img.shields.io/github/downloads/Blusutils/DESrv/total?label=Downloads&style=flat-square)](https://github.com/Blusutils/DESrv/releases)

[![GitHub issues](https://img.shields.io/github/issues/Blusutils/DESrv?label=Issues&style=flat-square)](https://github.com/Blusutils/DESrv/issues)
[![GitHub pull requests](https://img.shields.io/github/issues-pr/Blusutils/DESrv?label=PRs&style=flat-square)](https://github.com/Blusutils/DESrv/pulls)

[![GitHub Build Status](https://img.shields.io/github/actions/workflow/status/Blusutils/DESrv/per_commit.yml?branch=desrv2.0&label=Build&style=flat-square)](https://github.com/Blusutils/DESrv/actions/workflows/per_commit.yml)
[![GitHub CodeQL Status](https://img.shields.io/github/actions/workflow/status/Blusutils/DESrv/codeql.yml?branch=desrv2.0&label=CodeQL&style=flat-square)](https://github.com/Blusutils/DESrv/actions/workflows/codeql.yml)

[![GitHub release (latest SemVer)](https://img.shields.io/github/v/release/Blusutils/DESrv?label=Latest%20release&style=flat-square)](https://github.com/Blusutils/DESrv/releases/latest)
[![GitHub Releases Build Status](https://img.shields.io/github/actions/workflow/status/Blusutils/DESrv/release_build.yml?branch=desrv2.0&label=Release%20build&style=flat-square)](https://github.com/Blusutils/DESrv/actions/workflows/release_build.yml)

[![GitHub license](https://img.shields.io/github/license/Blusutils/DESrv?label=License&style=flat-square)](https://github.com/Blusutils/DESrv/blob/master/LICENSE.txt)

## DESrv seems to be done!

Finally, the writing of version 2.0 is nearing completion. Some plans (e.g. native and JAR extensions) have been dropped until DESrv 3.0. Now DESrv is almost stable and already supports independent extensions (plugins). Soon the contents of this branch (`desrv2.0`) will be merged into the `master` branch.

## What is DESrv?

**DESrv** is a framework-like server that helps you create and manage server assemblies using one convient interface.

Think that is a framework library that provides a little interface for networking apps and also a orchestration utility for them.

The key component of **DESrv** is the *extensions* - thoose "apps" that works with networking (but not limited to that!). There are APIs for them to make it easier to work with. For example, wrappers for some protocols, command manager, logger... All extensions are loaded into the Core and, in theory, can communicate with each other via reflection, but DESrv supports a system of addons and dependencies for convenience.

## Install

For 2.0 branch installation is available only from the sources.

Prerequisites:

* An OS that is supported by .NET - either Linux or Windows
  * macOS NOT tested but SHOULD work
* Git (any version)
* (optional) Extensions what you'll test
* (optional) Internet connection
* (optional) Connection client
* .NET SDK 7.0 or above and optionally Visual Studio 2022 (17)
  * OR Docker + Docker Compose

At first clone this repository:

```bash
git clone https://github.com/Blusutils/DESrv.git
cd DESrv
# if in some reason the branch is not desrv2.0, switch to it manually
git switch desrv2.0
```

### I'm have installed .NET

Run following:

```bash
dotnet restore DESrv.sln
dotnet run DESrv/DESrv.csproj -c Debug
# or: dotnet build DESrv/DESrv.csproj -c Debug
# or dotnet publish DESrv/DESrv.csproj -c Debug
# or build solution directly from VS
```

### I'm using Docker

Ensure you have Docker Compose and run:

```bash
docker compose build
docker compose up
```

## Wiki/docs

Documentation for DESrv is available on [GitHub Wiki](https://github.com/Blusutils/DESrv/wiki) page.

## Contributing

See [CONTRIBUTING](./CONTRIBUTING.md) for more info.

## License

DESrv is licensed under the [GNU Affero General Public License v3.0](./LICENSE-AGPL.txt).

```
Dedicated Extendible Server - framework-like solution designed to simplify creation and management of servers in one place, without trivial boilerplates. 
Copyright (C) 2023  Blusutils, EgorBron

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published
by the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
```

Following components are licensed under the [GNU Lesser General Public License v3.0](./LICESNSE-LGPL.txt):

* [DESrv.Configuration](./DESrv.Configuration/)
* [DESrv.Localization](./DESrv.Localization/)
* [DESrv.Logging](./DESrv.Logging/)
* [DESrv.LuaScriptingApi](./DESrv.LuaScriptingApi/)
* [DESrv.PDK](./DESrv.PDK/)
* [DESrv.Versions](./DESrv.Versions/)
