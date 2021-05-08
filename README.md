# Template Patch

Template Patch -- a template mod for Hardspace: Shipbreaker that builds cleanly and provides the bare essentials to get you started on changing it's behaviour.

## Requirements

The obvious ones:
- Hardspace Shipbreaker
- BepInEx: https://github.com/BepInEx/BepInEx/releases (You want the x64 .zip)

For building this repo you'll need Visual Studio Code and the C# extension. It probably will suggest installing everything that's required.

## Getting Dependencies

The HintPath in TemplatePatch.csproj references the DLLs under a out-of-repo path DLLs. It should have this structure:

- `DLLs\hssb\` should contain all DLLs from `C:\Program Files (x86)\Steam\steamapps\common\Hardspace Shipbreaker\Shipbreaker_Data\Managed` (if you installed the game on `C:`)
- `DLLs\bepinex\` should contain all DLLs from `BepInEx\core\` in the BepinEx zip file.

For building this repo in GitHub actions this repo depends on a private repo that contains the game's DLLs, where access is granted via a deploykey in Github itself. If you're interested in how that works and how to replicate it ask @klaernie for help.

## Building

The simplest way to build the project is running `msbuild` from the VS Code terminal.

This repo also contains the instructions to build using github actions.

## Installation

# Windows

Extract BepInEx to the root Hardspace Shipbreaker folder so `winhttp.dll` is in the same folder as `Shipbreaker.exe`.

Extract the compiled DLL so it is placed like this: `Hardspace Shipbreaker\BepInEx\plugins\TemplatePatch.dll`.

# Linux

Follow the Windows instructions. Afterwards you need to enable the DLL override.

For this either follow [the official instructions of BepInEx](https://bepinex.github.io/bepinex_docs/master/articles/advanced/steam_interop.html?tabs=tabid-1#protonwine) or do it manually.

In order to manually enable the override open `steamapps/compatdata/1161580/pfx/user.reg` in a text editor and go to section `[Software\\Wine\\DllOverrides]`.
Add a line to it:

```ini
[Software\\Wine\\DllOverrides]
…
"winhttp"="native,builtin"
```

Afterwards you can start the game as regular.

## Viewing the BepinEx console to see logging info in realtime

It can be helpful for debugging to see what log messages are spit out. For this you can enable the BepinEx debug console.

After following Installation instructions (above), run `Shipbreaker.exe` once. You can close it after you get to the main menu.

After this, navigate to `Hardspace Shipbreaker\BepinEx\config\` and edit `BepinEx.cfg`. Under `Logging.Console`, set `Enabled = true`.

Next time you run `Shipbreaker.exe` (directly or through steam), a console window should pop up too.

## Mod configuration
The mod's config file will be in `Hardspace Shipbreaker\BepInEx\config\com.github.hssb-racers.templatepatch.cfg` after you've run `Shipbreaker.exe` with `TemplatePatch.dll` installed properly at least once. 

Config options:

|      key     |                                              description                                             | default                             |
|:------------:|:----------------------------------------------------------------------------------------------------:|-------------------------------------|
| `DataFolder` | Where to store the CSVs of your salvage summaries ~or whatever other bullshit i decide to put there~ | `HardspaceShipbreaker\RACErsLedger` |

## Un-installation

BepInEx uses `winhttp.dll` as an injector/loader. Renaming or deleting this file is enough to disable both this mod and the loader.

## Support

This mod is provided on an AS-IS basis, with no implied warranty or guarantee that it will work at all. It might fuck up, it might accidentally delete your save, it might destroy spacetime, it might punch you in the face. 

Be prepared for that harsh reality. Back up your saves, etc.

## Credits

Thank you to Synthlight for making the [Furnace Performance Improvements mod](https://github.com/Synthlight/Hardspace-Shipbreaker-Furnace-Performance-Improvement-Mod) -- 
The structure and README of this mod borrow heavily from them, so thank you Synthlight for helping get me started on this route!

@sariyamelody for building the RACErsLedger, a mod that does awesome datalogging and enables visualization of that data.

This repo is an educational template built upon the shoulders of giants.