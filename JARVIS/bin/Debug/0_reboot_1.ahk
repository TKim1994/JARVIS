SetWorkingDir, %A_ScriptDir%

AScript := A_ScriptDir . "\0_reboot.cmd"
Run % AScript

if computername=FRIDAY10
{
    Sleep 5000
}
else if computername=FRIDAY8
{
    Sleep 200
}

SoundPlay, D:\D_Documents\AKim\02_KimIndustries\Local_Resources\Voice_Samples\rebooting.wav, 1
ExitApp