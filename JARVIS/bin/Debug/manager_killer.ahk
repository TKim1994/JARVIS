;JAQHES_XX by KimIndustries
;Rev: 2
;Author: Anthony Kim A.
;Released: 25_10_2019
;Descrip: Simple script que cierra todas las instancias de JAQHES_MANAGER.exe


main()

;----------------------------------------------------------------------------------------------------------------

main()
{
	Loop,
	{
		if ProcessExist("J4QH3S_MANAGER.exe")
		{
			Process,Close,J4QH3S_MANAGER.exe
		}
		else
		{
			break
		}
	}
	
	ExitApp
}

;[5]=============================================================================
ProcessExist(Name)
{
	Process,Exist,%Name%
	return Errorlevel
}