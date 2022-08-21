# Dedicated Extendable Server (DESrv or DES) <img src="./des_logo.png" align="center" width="100">
Flexible and extendable server for usage in different tasks. Includes Core and PDK.

## About this
**DESrv** is an all-in-one server for usage in different tasks. 

This software bases on theese key components:
* Core (server runtime basic logic)
  * Runner based on CEnd
  * PDK loader
* Plugin development kit (PDK) and Extensions
  * Plugin(s) (3rd-party extensions for server)
  * Add-ons (3rd- or 1st-party extensions for Plugins)

**DESrv** have APIs for Plugins and Add-ons (PDK, Plugin Development Kit), also have support for some databases and connection types out-of-the-box.
You can easily implement or extend needed functionality (in example socket data handler) for server by writing simple (or more complex) plugin or using existing by other developers.

## Installing
<details>
<summary><h3 id="des-for-standard">For standard usage</h3> (click to reveal...)</summary>

0. Prerequesties:
    * .NET 6.0
    * Windows or *nix system
    * Extensions what you need
    * (optional) Internet connection
    * (optional) Connection client

1. Download binaries for your OS and platform on [releases page](https://github.com/Blusutils/DESrv/releases/latest).

2. Open terminal, `cd` (change directory) to with downloaded binaries.

3. Type:

```batch
:: on Windows
des-config
```
```bash
# on *nix
./des-config
```

Follow the instructions in console to perform basic configuration of server.

4. Run DESrv:

```batch
:: on Windows
des-run <optional params>
```
```bash
# on *nix
./des-run <optional params>
```
</details>
<details>
<summary><h3>For plugin/add-on development</h3> (click to reveal...)</summary>

1. Make sure that you have already installed DESrv. 
If not, [go here](#des-for-standard). 

2. Go to the [docs](https://github.com/Blusutils/DESrv/wiki) for more information and tutorials. 

</details>
<details>
<summary><h2>Quick guides & recipes</h2> click here to reveal...</summary>

### Quick guide to configuration and command line arguments
DESrv needs configuration to run. You can set it using `des-config` in binaries. Out config file can be found in same directory with all binaries (file named as `desconfig.json`).
All values in this file is overridable on server run. Just pass commandline argument with same name as field name in config. In example:
```jsonc
// config file 
{
  "loglevel": "debug",
  "port": 9090,
  // other config params
}
```
```batch
:: on Windows
des-run --loglevel warn
```
In this example loglevel will be overriden for this run of server but port will stay 9090. 

All configuration parameters is available in docs. 
<!-- <details>
<summary><h3>List of all configuration parameters</h3></summary>

* host 
  * `string` `not required`
  * Default host IP to bind it to sockets. If not set, server will run on `localhost` (`127.0.0.1`). 

* port
  * `int` `not required`
  * Default port used to connect to the server. If not set, server will pick `9090` port. 

* loglevel
  * `string` `not required`
  * DES CEnd logger level. If not set, "debug" will used by default. 

* superuser
  * `string` `not required` 
  * Super-user login credentails in `name:password`. If not set, Super-user feature will not be used.

* sidetunnel 
  * `bool` `not required` 
  * Enables "SideTunnel" feature (only for Add-ons that supports it). 

* sequredchannel
  * `bool` `not required` 
  * Enables "SequredChannel" feature (only for Plugins and Add-ons that supports it). And all ok with name of this thing, I didn't make a typo. 

* prefersecure 
  * `bool` `not required` 
  * Prefers all sockets to use secured connection (in example WSS instead standard Websockets). 

* randommode
  * `bool` `not required`
  * Sets random integers generator (`dotrand`, `cpprand`, `randomorg` or any other from plugins). By default set to `dotrand` (standard .NET random). 
</details> --> 

### How to use extensions (Plugins or Addons)
That's very simple! Just put `.desext` file in `./extensions` folder in DESrv directory. 

By default DESrv runs with all found extensions. So, you need to run server with `use-exts` argument:
```batch
:: on Windows
des-run --use-exts ExamplePlugin ExampleAddon_ExamplePlugin
```
You can also put extensions what you'll use to configuration. 

Plugins should be named like `PluginName.desext`, and addons like `AddonName_TargetPlugiNname.desext` (`.desext` files is actually a .NET DLLs; don't change extension: it needed to detect files what is valid extensions).

### "Bad random" issues
DESrv was written on .NET C#, which has very bad pseudorandom. But you can choose what random you'll use. By default, four (three working) methods available:
* Standard System.Random
* C++ random
* [Random.org](https://random.org) API random
* [LavaRnd](https://www.lavarand.org/) random *TODO*

If you want to use another random implementation, create plugin with class, implements `IRandom` interface, then add it to configuration. More info in docs.
</details>

## Contributing 
See [CONTRIBUTING](./CONTRIBUTING.md) for more info. 

There's also contains inforamtion about builds from source code. 

## License 
DESrv licensed under `MIT License`


