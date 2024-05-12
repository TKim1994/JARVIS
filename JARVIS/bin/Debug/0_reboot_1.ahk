SetWorkingDir, %A_ScriptDir%

AScript := A_ScriptDir . "\0_reboot.cmd"
Run % AScript
Sleep 8000
SoundPlay, D:\D_Documents\AKim\02_KimIndustries\Local_Resources\Voice_Samples\rebooting.wav, 1
ExitApp