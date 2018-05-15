# Asymmetrical VR escape room like game

## Description
An asymmetrical, escape room like, VR game done with unity.
Developed in the context of the course _CS-444 Virtual Reality_ at École polytechinque fédérale de Lausanne (EPFL).

The scenario of the game is a spy mission and it involves two player: the _VR player_ and the _Oracle player_.
The first is inside the VR world (using an HTC vive) and acts as the spy.
The second - the so called _Oracle_ - act has the guy behind the scenes, which guides the spy through its mission.
The _Oracle_ see the scene from the top and, hopefully, has a simple command line at his disposal.
Obviously, in order to reach their goal, the two player have to collaborate and communicate.


## Commands
### Oracle Command line
The _Oracle player_ has a command line at his disposal, which can be used to execute the following commands.
- `pc <ID> hack` where <id> is the unique computer ID
- `light <ID> <off/on>` where <id> is the unique light ID

### HTC Vive Controllers


## Game Plot & Levels Description

#### Main Objects
- A computer
- A printer
- A Key
- Alarm cables
- Something to cut the cables

#### _VR player_ task
- Find the computer ID and communicate it to the _Oracle player_.
- Find the key
- Find the object to cut the alarm cables
- Find the alarm cables and use the key to open the "cable box"
- Cut the correct cable in order to deactivate the alarm

#### _Oracle player_ task
- Hack the computer, this will result in the printer printing the light IDs
- Turn off the light, so that the hidden message is shown
- Communicate to the VR player which cable to cut (based on the hidden message

#### Failure
- The _VR player_ cuts the wrong cable
- The timer counts down to zero

#### Success
- The _VR player_ cut the correct cable: an hidden room appears

## Authors
- Collaud Jonathan - [JonathanCollaud](https://github.com/JonathanCollaud)
- Hirt Grégoire - [stdgregwar](https://github.com/stdgregwar)
- Romerio Lucio - [lromerio](https://github.com/lromerio)
