;StretchingProt (JAQHES) by KimIndustries
;Rev: 1
;Author: Anthony Kim A.
;Released: 08_07_2019
;Descrip: StretchingProt especialmente para RPAs que necesitan correr en el 5to plano. Tambien sirve para mantener sincronizados los Creds de SAP y WIN (nube-local)
;OJO: Para actualizar en todos lados las credenciales (SAP, WIN) el cambio debe hacerse por JARVIS_KimIndustries.exe o en el archivo en OneDrive - NO EN EL ARCHIVO LOCAL

#SingleInstance, Force

main()

;----------------------------------------------------------------------------------------------------------------

main()
{
	SetWorkingDir, %A_WorkingDir%
	
	Loop,
	{
		; TRAE LAS CREDENCIALES DEL ONEDRIVE
		StringTrimRight, root_path, A_ScriptDir, 24
		sysDir = %root_path%\01_JAQHES\
		
		FileRead, oneDrive, %sysDir%\oneDrive.txt
		
		creds_folder_local1 = %root_path%\ExtraJAQHES\credenciales
		creds_folder_local2 = %root_path%\26_JAQHES\sys\Queries
		
		creds_folder_oneDrive1 = %oneDrive%\SAP
		creds_folder_oneDrive2 = %oneDrive%\WIN
		creds_folder_oneDrive3 = %oneDrive%\JAQHES_XXVI\Queries
		
		;Copia los txt de credenciales
		FileCopy, %creds_folder_oneDrive1%\SAP_creds.txt, %creds_folder_local1%\SAP_creds.txt, 1
		FileCopy, %creds_folder_oneDrive2%\WIN_creds.txt, %creds_folder_local1%\WIN_creds.txt, 1
		
		;Copia los qry de los Queries
		
		; 1st Way: PC -> Cloud
		Loop %creds_folder_local2%\*.*
		{
			oneDrive_file := creds_folder_oneDrive3 . "\" . A_LoopFileName
			;msgbox %oneDrive_file%
			if FileExist(oneDrive_file)
			{
				; Si existe compara las fechas de modificiacion
				FileGetTime, LocalDate, %A_LoopFileFullPath%, M  ; Retrieves the modification time
				FileGetTime, OneDriveDate, %oneDrive_file%, M  ; Retrieves the modification time
				
				;msgbox %LocalDate% y %OneDriveDate%
				if LocalDate>%OneDriveDate%
				{
					;msgbox es mayor el local que el onedrive
					;Copia el local a nube
					FileCopy, %A_LoopFileFullPath%, %oneDrive_file%, 1
				}
				else
				{
					;msgbox es mayor el onedrive que el local
					;No hace nada
				}
			}
			else
			{
				; No existe, copia de frente
				FileCopy, %A_LoopFileFullPath%, %oneDrive_file%, 1
				;msgbox copio!
			}
		}
		
		; 2nd Way: Cloud -> PC
		Loop %creds_folder_oneDrive3%\*.*
		{
			local_file := creds_folder_local2 . "\" . A_LoopFileName
			;msgbox %oneDrive_file%
			if FileExist(local_file)
			{
				; Si existe compara las fechas de modificiacion
				FileGetTime, OneDriveDate, %A_LoopFileFullPath%, M  ; Retrieves the modification time
				FileGetTime, LocalDate, %local_file%, M  ; Retrieves the modification time
				
				;msgbox %LocalDate% y %OneDriveDate%
				if OneDriveDate>%LocalDate%
				{
					;msgbox es mayor el onedrive que el local
					;Copia nube a local
					FileCopy, %A_LoopFileFullPath%, %local_file%, 1
				}
				else
				{
					;msgbox es mayor el local que el onedrive
					;No hace nada
				}
			}
			else
			{
				; No existe, copia de frente
				FileCopy, %A_LoopFileFullPath%, %local_file%, 1
				;msgbox copio!
			}
		}
		
		Sleep 2000
	}
}