SetWorkingDir, %A_ScriptDir%

AScript := A_ScriptDir . "\0_reboot.cmd"
Run % AScript
Sleep 10000
SoundPlay, D:\D_Documents\AKim\02_KimIndustries\Local_Resources\Voice_Samples\rebooting.wav, 1
ExitApp