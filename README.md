# Dedicated Extendible Server (DESrv or DES) <img src="./DESrvLogo.svg" align="center" width="100">

[![GitHub Repo stars](https://img.shields.io/github/stars/Blusutils/DESrv?label=Stars&style=flat-square)](https://github.com/Blusutils/DESrv/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/Blusutils/DESrv?label=Forks&style=flat-square)](https://github.com/Blusutils/DESrv/network/members)
[![GitHub all releases](https://img.shields.io/github/downloads/Blusutils/DESrv/total?label=Downloads&style=flat-square)](https://github.com/Blusutils/DESrv/releases)

[![GitHub issues](https://img.shields.io/github/issues/Blusutils/DESrv?label=Issues&style=flat-square)](https://github.com/Blusutils/DESrv/issues)
[![GitHub pull requests](https://img.shields.io/github/issues-pr/Blusutils/DESrv?label=PRs&style=flat-square)](https://github.com/Blusutils/DESrv/pulls)

[![GitHub release (latest SemVer)](https://img.shields.io/github/v/release/Blusutils/DESrv?label=Latest%20release&style=flat-square)](https://github.com/Blusutils/DESrv/releases/latest)
[![GitHub Build Status](https://img.shields.io/github/workflow/status/Blusutils/DESrv/.NET%20build?label=Build&style=flat-square)](https://github.com/Blusutils/DESrv/actions/workflows/dotnet.yml)
[![GitHub CodeQL Status](https://img.shields.io/github/workflow/status/Blusutils/DESrv/CodeQL?label=CodeQL&style=flat-square)](https://github.com/Blusutils/DESrv/actions/workflows/codeql.yml)

[![GitHub license](https://img.shields.io/github/license/Blusutils/DESrv?label=License&style=flat-square)](https://github.com/Blusutils/DESrv/blob/master/LICENSE.txt)

Flexible and extendible framework-like server for usage in different tasks. This repo contains Core and PDK.

## About this

**DESrv** is a framework-like server for usage in different tasks.

This software bases on these key components:

* Core (server runtime basic logic)
  * Runner based on [DESCEndLib](https://github.com/Blusutils/DESCEndLib)
  * PDK loader
* Plugin development kit (PDK, plugin framework) and Extensions
  * Plugin(s) (3rd-party extensions for server)
  * Add-ons (3rd- or 1st-party extensions for Plugins)

**DESrv** have APIs for Plugins and Add-ons (PDK, Plugin Development Kit), also have support for some databases and connection types out-of-the-box.
You can easily implement or extend needed functionality (in example socket data handler) for server by writing simple (or more complex) plugin or using existing by other developers.

<h2 id="des-for-standard" id="des-for-standard">Install</h2>

0. Prerequesties:
    * An OS - either Linux with installed .NET 6.0 runtime or Windows
    * Extensions what you'll use
    * (optional) Internet connection
    * (optional) Connection client

1. Download binaries on [releases page](https://github.com/Blusutils/DESrv/releases/latest) or from artifact in [Actions](https://github.com/Blusutils/DESrv/actions/workflows/dotnet.yml). DESrv builds starting from v1.2.0 is **standalone**, so you didn't need to download .NET runtime.

2. Unzip binaries.

3. Open terminal, `cd` (change directory) to with downloaded binaries.

4. Run DESrv. Follow the instructions in console to perform basic configuration of server.

```batch
:: in Windows cmd
des-run <optional params>
```

```bash
# on *nix
./des-run <optional params>
```

### Plugin/add-on development

Make sure that you have already installed DESrv.
If not, [go here](#des-for-standard).

Go to the [docs](https://github.com/Blusutils/DESrv/wiki) for more information and tutorials. Also recommended to use [template](https://github.com/Blusutils/desrv-pdk-example).

### Builds from source code
#### Prerequesties

* Visual Studio 2022 (17)
* .NET SDK 6.0 or above
* Git (any version)

1. Clone this repository:

    ```batch
    git clone https://github.com/Blusutils/DESrv.git
    ```

2. Run:

    ```batch
    :: or build solution directly from VS
    dotnet restore
    dotnet run -c Debug
    :: or: dotnet build -c Debug
    ```

## Contributing

See [CONTRIBUTING](./CONTRIBUTING.md) for more info.

There's also contains information about builds from source code.
