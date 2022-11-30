# Dedicated Extendible Server (DESrv or DES) <img src="./DESrvLogo.svg" align="center" width="100">

[![GitHub Repo stars](https://img.shields.io/github/stars/Blusutils/DESrv?label=Stars&style=flat-square)](https://github.com/Blusutils/DESrv/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/Blusutils/DESrv?label=Forks&style=flat-square)](https://github.com/Blusutils/DESrv/network/members)
[![GitHub all releases](https://img.shields.io/github/downloads/Blusutils/DESrv/total?label=Downloads&style=flat-square)](https://github.com/Blusutils/DESrv/releases)

[![GitHub issues](https://img.shields.io/github/issues/Blusutils/DESrv?label=Issues&style=flat-square)](https://github.com/Blusutils/DESrv/issues)
[![GitHub pull requests](https://img.shields.io/github/issues-pr/Blusutils/DESrv?label=PRs&style=flat-square)](https://github.com/Blusutils/DESrv/pulls)

[![GitHub release (latest SemVer)](https://img.shields.io/github/v/release/Blusutils/DESrv?label=Latest%20release&style=flat-square)](https://github.com/Blusutils/DESrv/releases/latest)
[![GitHub Workflow Status](https://img.shields.io/github/workflow/status/Blusutils/DESrv/.NET%20build?label=Build&style=flat-square)](https://github.com/Blusutils/DESrv/actions/workflows/dotnet.yml)

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
    * Windows/Linux (macOS not tested yet)
    * Extensions what you'll use
    * (optional) Internet connection
    * (optional) Connection client

1. Download binaries on [releases page](https://github.com/Blusutils/DESrv/releases/latest) or from artifact in [Actions](https://github.com/Blusutils/DESrv/actions/workflows/dotnet.yml). DESrv builds starting from v1.2.0 is **standalone**, so you didn't need to download .NET runtime.

2. Unzip binaries.

3. Open terminal, `cd` (change directory) to with downloaded binaries.



5. Run DESrv. Follow the instructions in console to perform basic configuration of server.

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
* .NET SDK 7.0
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

<!--<hr>
<details>
<summary>
<h2>Quick guides & recipes</h2> click here to reveal...</summary>

### Quick guide to configuration and command line arguments

DESrv has configuration to remember some settings for each run. You can set them using `des-config` in binaries. Out config file can be found in same directory with all binaries (file named as `config.json`).
All values in this file is overridable on server run. Just pass command line argument with same name as field name in config. In example:

```jsonc
// config file 
{
  "loglevel": "debug",
  "ipaddress": "127.0.0.1",
  // other config params
}
```

```batch
:: in Windows cmd
des-run --loglevel warn
```

In this example loglevel will be overriden for this run of server but IP address will stay 127.0.0.1.

All configuration fields available in docs.

<details>
<summary><h3>List of all configuration parameters</h3></summary>

* host 
  * `string` `not required`
  * Default host IP to bind it to sockets. If not set, server will run on `localhost` (`127.0.0.1`). 

* loglevel
  * `string` `not required`
  * DES CEnd logger level. If not set, "debug" will used by default. 

-- depreceted in case of usage "RSUDo" plugin
* superuser
  * `string` `not required` 
  * Super-user login credentails in `name:password`. If not set, Super-user feature will not be used.

* sequredchannel
  * `bool` `not required` 
  * Enables "SequredChannel" feature (only for Plugins and Add-ons that supports it). And all ok with name of this thing, I didn't make a typo. 

* prefersecure 
  * `bool` `not required` 
  * Prefers all sockets to use secured connection (in example WSS instead standard Websockets). 

* randommode
  * `bool` `not required`
  * Sets random integers generator (`dotnet`, `cpp`, `randomorg`, `lava` or any other from plugins). By default set to `dotrand` (standard .NET random). 
</details>
-->

## Contributing

See [CONTRIBUTING](./CONTRIBUTING.md) for more info.

There's also contains information about builds from source code.
