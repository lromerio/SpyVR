# Asymmetrical VR escape room like game

## Description
An asymmetrical, escape room like, VR game done with Unity and SteamVR.
Developed in the context of the course _CS-444 Virtual Reality_ at École polytechinque fédérale de Lausanne (EPFL).

The scenario of the game is a spy mission and it involves two players: the _VR player_ and the _Oracle player_.
The first is inside the VR world (using an HTC vive) and acts as the spy.
The second - the so called _Oracle_ - act has the guy behind the scenes, which guides the spy through his mission.
The _Oracle_ sees the scene from the top and has a simple command line at his disposal.
Obviously, in order to reach their goal, the two players have to collaborate and communicate.


## Commands
### Oracle Command line
The _Oracle player_ has a command line at his disposal, which can be used to execute the following commands.
- `pc <ID> hack` where <id> is the unique computer ID
- `light <ID> <off/on>` where <id> is the unique light ID

### HTC Vive Controllers
The image here below summarise the commands, for a more detailed explanation please refer to the [report](https://github.com/lromerio/vr_project/blob/master/doc/Group2_ProjectReport.pdf).

![](https://github.com/lromerio/vr_project/blob/master/VR_Project/Assets/Textures/controllers.png)

## Game Description
This is just a quick overview, for more details please refer - again - to the  [report](https://github.com/lromerio/vr_project/blob/master/doc/Group2_ProjectReport.pdf).

#### Main Objects
- A computer
- A printer
- A Key
- Alarm cables

#### _VR player_ tasks
- Find the computer ID and communicate it to the _Oracle player_.
- Find the key
- Find the alarm cables and use the key to open the "cable box"
- Cut the correct cables in order to deactivate the alarm

#### _Oracle player_ tasks
- Hack the computer, this will result in the printer printing the light specifications
- Turn off the lights, so that the hidden message is shown
- Communicate to the _VR player_ which cable to cut (based on the hidden message)

#### Failure
- The _VR player_ cuts the wrong cable
- The timer counts down to zero

#### Success
- The _VR player_ cut the correct cables: an hidden room appears

## Authors
- Collaud Jonathan - [JonathanCollaud](https://github.com/JonathanCollaud)
- Hirt Grégoire - [stdgregwar](https://github.com/stdgregwar)
- Romerio Lucio - [lromerio](https://github.com/lromerio)
