# DES
Powerful and flexible Dedicated External Server for usage in different tasks. Includes Core and PDK.

## About this
**DES** is an all-in-one server for usage in different tasks: websocket-based server, game server under UDP sockets, simple database, etc.
It bases on three key components:
* Core (server basic logic)
* Plugin(s) (3rd-party extensions for server)
* Add-ons (3rd- or 1st-party extensions for Plugins)

DES have good APIs for Plugins and Add-ons (PDK, Plugin Development Kit), and also have support for some databases and connection types (TCP/UDP sockets, websockets, HTTP) out-of-the-box.
You can easily add needed functionality (in example advanced socket data handler) for server by writing simple (or more complex) plugin or using existing by other developers.

## Installing
<details>
<summary><h3>For standard usage</h3></summary>

1. Download binaries for your OS and platform on [releases page]().

2. Open terminal, `cd` (change directory) to with downloaded binaries.

3. Type:

* on Windows:

```batch
des-config
```

* on Linux:

```bash
./des-config
```

4. Follow the instructions in console to configure server.

5. Run DES:

Windows:
```batch
des-run --servermode <server mode> --loglevel debug --port <port; leave this if you want standard (9090)>
```
Linux:
```bash
./des-unix-prepare && ./des-run --servermode <server mode> --loglevel debug --port <port; leave this if you want standard (9090)>
```
</details>
<details>
<summary><h3>For plugin/add-on development</h3></summary>

1. Make sure that you have already installed DES. If not, [go here](#for-production). 

2. Download PDK on [releases page](). 

3. Go to [docs]() or more information and tutorials. 

</details>

## Guide to configuration and commandline
DES needs configuration to run. You can set it using `des-config` in binaries. Out config file can be found in `AppData/Local/DES` on Windows or in `/user/DES` on Linux. 
All theese values can be overrided when you pass commandline argument with same name as parameter in config. In example:
```json
// config file 
{
  "servermode": "tcpsock", 
  "loglevel": "debug",
  "port": 9090,
  // other config params
}
```
```batch
:: cmd
des-run --servermode udpsock --loglevel warn
:: servermode and loglevel will be overrided for this run of server but port will stay 9090
```
<details>
<summary><h3>List of all configuration parameters</h3></summary>

* servermode 
* * `string`
* * What type of connection server will use. 

* host 
* * `string`
* * Default host IP to bind it to sockets. 

* port
* * `int`
* * Default port used to connect to the server. 

* loglevel
* * `string`
* * `not required` DES CEnd logger level. If not set, "debug" will used by default. 

* superuser
* * `string`
* * `not required` Super-user login credentails in `name:password`. If not set, Super-user feature will not be used.

* sidetunnel 
* * `bool`
* * `not required` Enables "SideTunnel" feature (only for Add-ons that supports it). 

* SequredChannel ||or securedchannel||
* * `bool` 
* * `not required` Enables "SequredChannel" feature (only for Plugins and Add-ons that supports it). 

* prefersecure 
* * `bool`
* * `not required` Prefers all sockets to use secured connection (in example WSS instead straight Websockets). 
</details>
