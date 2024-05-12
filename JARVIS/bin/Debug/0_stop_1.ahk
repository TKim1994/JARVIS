SetWorkingDir, %A_ScriptDir%

AScript := A_ScriptDir . "\0_stop.cmd"
Run % AScript
SoundPlay, D:\D_Documents\AKim\02_KimIndustries\Local_Resources\Voice_Samples\shutdown_aborted.wav, 1
ExitApp