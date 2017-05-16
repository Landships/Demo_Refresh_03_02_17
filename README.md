# Landships

Landships is a virtual reality multiplayer tank simulation created by students at UC Berkeley. To learn more about the project, visit https://landships.vrab.io/. To learn more about our virtual reality club, visit https://vr.berkeley.edu/. 


## Setup:
Designate one player as the driver; they will be the host computer. The other player will be the gunner, who will be a client. After starting the game, each player can position their chair in the Vive play space so that they line up with the chair in the tank. Each player must be running the build on their own computer and Vive headset, though both players can be in the same play space using just one set of light houses. 

### Adjusting Positions:
Each player's view can be reset to the original position by either clicking the trackpad on the Vive controller or pressing 'c' on the keyboard (make sure the game window is in focus when using hotkeys). Each player can move up and down by pressing the grip buttons on the side of the controllers or using 'w' and 's' on the keyboard. 

### Playing the Game:
There are currently three playable scenes. One is a tutorial for driving the tank, another is a tutorial for controlling the turret, and the last one allows players to operate their tank together in a military base and fight four AI controlled enemy tanks. The tutorials are single player and can be started by simply opening and running the appropriate scene (Demo_Refresh_03_02_17/Assets/_Project/Scenes/DriveTutorial for the driver and Demo_Refresh_03_02_17/Assets/_Project/Scenes/ShootingTutorial for the gunner). To start a multiplayer game follow these steps:
1. Open and run Ipscene (location should be Demo_Refresh_03_02_17/Assets/_Project/Scenes/IPScene) on both the host and client computers.
2. Type "host" and press enter on the host computer, and type the IP address of the host and press enter on the client computer. To find the IP address, run command prompt on the host computer (windows key + r, type, cmd, press enter) and then type "ipconfig" and press enter. Look for "IPv4 Address" which probably looks something like 192.168.12.345. 
3. Either player can do either tutorial if desired by clicking on the tutorial button. When ready to start the game, press spacebar (either in the menu scene or in one of the tutorials). 
4. Once both the client and host are in the main game scene the client should connect and both players should be inside the tank. At this point the players can control their tank and destroy the enemies. 

For single player just follow the above steps ignoring everything about a client computer. 


## Controlling the Tank:
Interact with objects by intersecting it with the Vive controller and pressing and holding trigger with the index finger. 

### As driver:
Use the two levers to control movement of tank. Each level corresponds to either the right tread or left tread. To move forward, move both levers forward. To do a gentle turn right turn, move the left lever forward, or vice versa. To do a sharp right turn, move the left lever forward and the right lever back, or vice versa. The driver can use the periscope to see outside, or can reach up to open the hatch and stand up to see out of the tank. 

### As gunner:
Turn the horizontal crank to move the turret to the left and right, and the vertical crank to move the turret up and down. Press trigger on the red button to fire the tank. Use the periscope to see outside and communicate with the driver to know how to adjust aiming. 

### Goal:
Players can drive around the military base and destroy enemy tanks (tanks will explode and start smoking when hit by a shot). There are a total of four tanks in the map, though the game continues after destroying all four. 


## Troubleshooting:
If the client did not join the host successfully in the game scene (causing neither of the player's actions to affect the other's game and making the players unable to see each other's hands), make sure both computers are on the same WIFI network and the IP of the host was entered correctly for the client. If the client still cannot connect, try switching which computer is host, restarting the build, and making sure Unity or the .exe is allowed through the firewall. If the client disconnects after connecting successfully, it should reconnect automatically. 
