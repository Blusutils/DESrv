# Dedicated Extendible Server (DESrv or DES) v2.0 <img src="./DESrvLogo.svg" align="center" width="100">

[![GitHub Repo stars](https://img.shields.io/github/stars/Blusutils/DESrv?label=Stars&style=flat-square)](https://github.com/Blusutils/DESrv/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/Blusutils/DESrv?label=Forks&style=flat-square)](https://github.com/Blusutils/DESrv/network/members)
[![GitHub all releases](https://img.shields.io/github/downloads/Blusutils/DESrv/total?label=Downloads&style=flat-square)](https://github.com/Blusutils/DESrv/releases)
[![GitHub issues](https://img.shields.io/github/issues/Blusutils/DESrv?label=Issues&style=flat-square)](https://github.com/Blusutils/DESrv/issues)
[![GitHub pull requests](https://img.shields.io/github/issues-pr/Blusutils/DESrv?label=PRs&style=flat-square)](https://github.com/Blusutils/DESrv/pulls)
[![GitHub license](https://img.shields.io/github/license/Blusutils/DESrv?label=License&style=flat-square)](https://github.com/Blusutils/DESrv/blob/master/LICENSE.txt)

## ## ⚠ DESrv is currently in WIP state ⚠ ## ##

A major update 2.0 is now being prepared in which DESrv has been completely rewritten. The `desrv-2.0` branch is now the main branch, and use of the `master` branch is not recommended as it contains non-working code. I hope all this is not for nothing!

## Install

For 2.0 branch installation is available only from sources.

Prerequisites:

* An OS - either Linux or Windows
    * macOS NOT tested but SHOULD work
* Git (any version)
* (optional) Extensions what you'll test
* (optional) Internet connection
* (optional) Connection client
* .NET SDK 7.0 or above and optionally Visual Studio 2022 (17)
    * OR Docker + Docker Compose

At first clone this repository and switch to 2.0 branch:

```bash
git clone https://github.com/Blusutils/DESrv.git
cd DESrv
git switch desrv2.0
```

### I have .NET installed

Run following:

```bash
# or build solution directly from VS
dotnet restore
dotnet run -c Debug
# or: dotnet build -c Debug
# or publish
```

### I'm using Docker

Ensure you have Docker Compose and run:
```bash
docker compose build
docker compose up
```

## Contributing

See [CONTRIBUTING](./CONTRIBUTING.md) for more info.
