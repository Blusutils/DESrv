# DES
Powerful and flexible Dedicated External Server for usage in different tasks. Includes Core and PDK.

## About this
**DES** is a server for usage in different tasks: websocket-based server, game UDP server, simple database, etc.
It basess on three key components:
* Core (server basic logic)
* Plugin(s) (3rd-party extensions for server)
* Add-ons (3rd- or 1st-party extensions for Plugins)

DES have good APIs for Plugins and Add-ons (PDK and ADK), and also have support for some databases and connection types (TCP/UDP sockets, websockets, HTTP) out-of-the-box.
You can easily add needed functionality (in example advanced socket data handler) for server by writing simple (or more complex) plugin or using existing by other developers.

## Installing
<details>
<summary><h3>For production</h3></summary>

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
des-run --loglevel debug --port <port; leave this if you want standard (6750)>
```
Linux:
```bash
des-unix-prepare
des-run --loglevel debug --port <port; leave this if you want standard (6750)>
```
</details>
<details>
<summary><h3>For plugin/add-on development</h3></summary>

1. Make sure that you have already installed DES. If not, [go here](#for-production). 

2. Download PDK on [releases page](). 

3. Go to [docs]() or more information and tutorials. 

</details>
