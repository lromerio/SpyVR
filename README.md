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
The _Oracle player_ has a command line at his disposal, which can be used to execute the three following commands.
- `pc <ID> hack` where <id> is the unique computer ID
- `door <ID> <open/close>` where <id> is the unique door ID
- `light <ID> <off/on>` where <id> is the unique light ID

### HTC Vive Controllers


## Game Plot & Levels Description

### Level 1
##### Scenario & Key Objects
- Alarm on the elevator
- A computer
- Alarm cables
- Something to cut the cables

##### _VR player_ task
- Find the computer ID and communicate it to the _Oracle player_.
- Find the object to cut the alarm cables
- Find the alarm cables
- Cut the correct cable in order to deactivate the alarm

##### _Oracle player_ task
- Hack the computer in the room and obtain the alarm specification
- Communicate to the VR player which cable to cut

##### Failure
- The _VR player_ cuts the wrong cable

### Level 2
##### Scenario & Key Objects
- Another elevator in the other side of the room
- Multiple doors (resulting in a sort of labirint)
- A key
- A guard

##### _VR player_ task
- May communicate doors IDs to the _Oracle player_
- Find the key
- Reach the second elevator and use the key to open it

##### _Oracle player_ task
- Guide the _VR player_ through the room
- Open/close the doors when necessary

##### Failure
- The guard see the _VR player_.

### Level 3
##### Scenario & Key Objects
###### Floor 1
- Something covering the floor
- A printer (turned off)
- Each light as an ID
- Symbol on the floor, visible only when the lights are turned off

###### Floor 2
- A computer
- Each light as an ID
- Symbol on the floor, visible only when the lights are turned off

###### Floor 3
- All light turned off at the beginning
- A poster mapping symbols to a numbers
- A door behind protected by a code
- An hint suggesting to turn off lights

##### _VR player_ task
- Turn of the printer
- Comunicate the computer ID to the _Oracle player_
- Take the paper printed by the _Oracle player_ and communicate lights IDs to him
- Find the symbols described by the _Oracle_ inside the poster, and communicate the corresponding numbers to the _Oracle_

##### _Oracle player_ task
- Hack the computer and obtain the lights specification file
- Print the lights specification file
- Turn on/off lights and memorize symbols appearing on the floor (when the lights are turned off)
- Describe the symbols to the _VR player_
- Use the ID resulting from the poster mapping to hack and open the door

##### Failure
- Wrong ID used to hack the port


## Authors
- Collaud Jonathan - [JonathanCollaud](https://github.com/JonathanCollaud)
- Hirt Grégoire - [stdgregwar](https://github.com/stdgregwar)
- Romerio Lucio - [lromerio](https://github.com/lromerio)
