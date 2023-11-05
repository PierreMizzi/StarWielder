
## Sound Manager Tool
___


###### Note : Every word bewteen brackets ([like this]) is an implemented and usefull class

### Introduction :

**SoundManagerTool** is a static class used to Play/Stop/FadeIn/FadeOut musics and SFX.
It's coupled with a setting class nammed **SoundManagerToolSettings**, which is located in the Resource Folder and stores a collection of **SoundDataLibrary**.

A **SoundDataLibrary** is a library for **SoundData**.

**SoundData** is a custom class to store AudioClip of a specific sound, identified by an ID (string), along with other parameters.

**SoundManagerTool**'s role is to manage **SoundSources**, mute or unmute them as you please.

**SoundSource** is a custom class used to play a **SoundData**.
It can also Mute/Unmute **SoundSource** of the same type.

Important note : 

When you call **SoundManagerTool**.PlaySound, the function return the **SoundSource** used to play the sound. You can then use it to alter its behaviour


### Getting Started : 

- First import SoundManager inside Assets/Plugins
- Left Click -> Extensions -> Sound Manager -> SoundManagerToolSettings
- Left Click -> Extensions -> Sound Manager -> SoundDataLibrary
- Once you have a **SoundDataLibrary**, you need to add **SoundData**. A **SounData** is composed of a an ID(string) and an AudioClip. You can add an optional AudioMixer for balancing
Note : You can fast fill a library by filling the list below with Audioclip. It will automaticaly generate **SoundData** with AudioClip's name has ID(string)
- Add **SoundDataLibraries** to **SoundManagerToolSettings**
- Select **SoundManagerToolSettings** and click "Generate SoundDataID static class"
- Copy the content in a newly create .cs file named "SoundDataID"
- Give a reference of **SoundManagerToolSettings** to any script in your scene so it can be initialized

Then from any scripts, you can call SoundManager.PlaySound(string SoundDataID) and play a sound