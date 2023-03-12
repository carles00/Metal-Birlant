[//]: # (Title of the project)

![logo](https://raw.githubusercontent.com/carles00/Metal-Birlant/main/Assets/Branding/simple-logo.png)
# Metal Birlant: The PC Videogame

[//]: # (GPLv3 License indicator)

[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0.html)

[//]: # (README Body)

A PC videogame adaptation of the original Metal Birlant comicbook.

### Controls: 
| **Action**        | **Input**                            |
|-------------------|--------------------------------------|
| _Player movement_ | Arrows / WASD-keys / Gamepad L-stick |
| _Jump_            | Spacebar                             |
| _Dash_            | Left Shift                           |

### Debugging
```shell
git clone https://github.com/carles00/Metal-Birlant
```
1. Open up the cloned repository in the Unity Editor.
2. Load the scene to debug (main playable one is `Assets/Scenes/PlayScene.unity`).

**DISCLAIMER:** Game physics (such as jumping) do not work properly when debugging. To test the gameplay properly it is best to build to the desired target.

### Build
1. Within the Unity Editor, go to `File > Build...`.
2. In the pop-up window, select the target architecture to build (Windows, Linux, macOS).
3. Click the build button and wait for it to finish compiling.

**NOTE:** Global project's `Makefile` to build both the game and its documentation is still in development.

