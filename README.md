# Github repository for B.L.U.

## Controls ##

 Player Movement:
 ``` 
 WSAD:  up, down, left and right respectively 
 Space:  Jump
 ```

 RGB Goggle controls: 
 ```
    E --  Activate/Deactivate RGB Goggles
    Mouse Wheel/Arrow Keys Left + Right --  Changes RGB Colour
 ```

Magnet controls: 
 ```
    Q --  Activate/Deactivate Magnet
 ```

## Camera Information
The camera can do the following:
>1. Look at the player
2. Look at a focal point (which can be any game object)
3. Use camera presets (which can have their own focal point)

These can all be turned off.


##### Adding camera Presets to a scene.
>1. Create an empty game object
2. Add your camera positions as children of this object
3. Add the CameraPositionObject component to your camera
4. Set your position and rotation of your camera
5. (Optional) add a focal point for the camera to look at


## RGB Goggles Information
##### Adding RGB goggles to a scene

>1. Create empty game object and place in scene.
2. Add your RgbGoggles Component to it
3. Add a  canvas, add an Image (UI/Image) as a child of the canvas to your scene   (this part is likely to change to use a camera shader)
4. Connect the RawImage to the RgbGoggles component's Color Filter parameter
5. (Optional) Connect DebugText to a TextMeshProGUI to see the state change

It can technically be placed on any object 

##### Adding RGB Filterable Objects to a scene

>1. Create an empty game object and place in scene
2. Add your filterable objects as children of this empty game object
3. Highlight your objects and add the RgbFilterObject as a component
4. Add the parent of your filtered objects to your RgbGoggles component
5. (Optional) Go back to your filter objects and set their FilterLevel -- This sets which RGB setting the object is visible on. e.g. if set to R, then your object will only be visible when the user selects R for the goggles

##### RGB Filterable Objects
Rgb objects can be anything provided it has the RgbFilterObject component.

##### Screen effects?
There is an optional CRTCameraBehaviour component on the active camera. (CameraController/Main Camera)
Untick the component to turn off the effect. We might be using this to handle the filter changes instead of the Image texture on the canvas.

`https://assetstore.unity.com/packages/vfx/shaders/crt-free-248066`
