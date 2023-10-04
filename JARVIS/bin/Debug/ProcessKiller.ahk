;JAQHES_XX by KimIndustries
;Rev: 2
;Author: Anthony Kim A.
;Released: 25_10_2019
;Descrip: Descarga de Configuraciones de Reles ABB

Global 1

main()

^f3::Pause

;----------------------------------------------------------------------------------------------------------------

main()
{
	aux = %1%
	SetWorkingDir, %A_ScriptDir%

	If Not (aux)
	{
		Process,Close,cmd.exe
		;
	}
	else
	{
		;ProcessKiller(aux)
		Process,Close,cmd.exe
	}
	ExitApp
}


;///////////////////////////////////////////////////////////// F U N C I O N E S //////////////////////////////////////////////////////////////////
;///////////////////////////////////////////////////////////// F U N C I O N E S //////////////////////////////////////////////////////////////////
;///////////////////////////////////////////////////////////// F U N C I O N E S //////////////////////////////////////////////////////////////////
;///////////////////////////////////////////////////////////// F U N C I O N E S //////////////////////////////////////////////////////////////////
;///////////////////////////////////////////////////////////// F U N C I O N E S //////////////////////////////////////////////////////////////////

;[1]=============================================================================================================================================================================================
Popup(TEXT, TITLE := "AutoHotkey Info", OPTIONS := "")
{
        ;; Parse OPTIONS.
        OPTIONS := "," . OPTIONS . ","
        NOTITLE := InStr(OPTIONS, ",notitle,")
        NOWRAP := InStr(OPTIONS, ",nowrap,")

        If (RegExMatch(OPTIONS, ",delay=([0-9]+),", DELAY))
                DELAY := DELAY1
        Else
                DELAY := 5000

        If (RegExMatch(OPTIONS, ",color=([^,]+),", COLOR))
                COLOR := COLOR1
        Else
                COLOR := "0x000000"

        If (RegExMatch(OPTIONS, ",xoffset=([0-9]+),", XOFFSET))
                XOFFSET := XOFFSET1
        Else
                XOFFSET := A_SCREENWIDTH - 310

        If (RegExMatch(OPTIONS, ",yoffset=([0-9]+),", YOFFSET))
                YOFFSET := YOFFSET1

        If (TITLE == "")
                TITLE := "AutoHotkey Info"

        If (!NOWRAP)
                WIDTH := "W250"

        ;; Count the number of lines in TEXT.  This doesn't modify TEXT.
        LINES := StrSplit(TEXT, "`n", "`n")

        If (!YOFFSET)
        {
                ;; The caller did not specify a Y offset, so we compute one.
                YOFFSETBASE := 105      

                If (NOWRAP)
                {
                        ;; No word-wrapping is happening.  The window is as wide as the text
                        ;; requires.  Move the window up by 15 pixels for each line of text.
                        YOFFSET := A_SCREENHEIGHT - YOFFSETBASE - 15 * LINES.Length()
                }
                Else
                {
                        ;; We have word-wrapped text in a fixed width window that is WIDTH pixels
                        ;; wide.  Set YOFFSET based on how many wrapped lines will appear.
                        YOFFSET := A_SCREENHEIGHT - YOFFSETBASE

                        For INDEX, VALUE in LINES
                        {
                                ;; Move the window 15 pixels up for each line of wrapped text.
                                YOFFSET := YOFFSET - 15 * (StrLen(VALUE) // 46 + 1)
                        }
                }
        }

        ;; Create the base window.
        Gui MyPopup: New
        Gui MyPopup: +AlwaysOnTop +Owner -Resize -Caption +Border
        Gui MyPopup: Color, 0xF9F9FB  ;; Background color that matches system tray balloon.

        TEXTPOSITION := ""

        If (!NOTITLE)
        {
                ;; When TITLE is shown, position the text below the icon and TITLE.
                TEXTPOSITION := "X12"

                ;; Show TITLE to the right of a 16x16 icon.
                Gui MyPopup: Add, Picture, W16 H-1 GPopupClose, %ICONSDIR%\yellow-button.ico
                Gui MyPopup: Font, s11 c0x000088, Verdana
                Gui MyPopup: Add, Text, R1 XP+23 YP-2 GPopupClose, %TITLE%
        }

        ;; Create the text control showing TEXT.
        Gui MyPopup: Font, s9 c%COLOR%, Segoe UI
        Gui MyPopup: Add, Text, %TEXTPOSITION% %WIDTH% GPopupClose, %TEXT%
        Gui MyPopup: Show, AutoSize NoActivate X%XOFFSET% Y%YOFFSET%

        ;; If DELAY is 0, display the popup window indefinitely.  Call PopupClose() to close it.
        If (DELAY == 0)
                Return

        ;; Close the popup window DELAY milliseconds from now.
        SetTimer PopupClose, -%DELAY%
}

;[6]=============================================================================================================================================================================================
PopupClose()
{
        Gui MyPopup: Destroy
        SetTimer PopupClose, Delete
}

;[7]=============================================================================================================================================================================================
PopupError(TEXT)
{
        Popup(TEXT, "AutoHotkey Error")
}

;[11]=============================================================================================================================================================================================
CurrentUserProcessKiller(process_name)
{
	current_user = %A_UserName%
	
	;Solo elimina los procesos que creo el usuario actual
	INIKILLER1: ;<--------------
	Sleep 100
	for process in ComObjGet("winmgmts:\\.\root\cimv2").ExecQuery("Select * from Win32_Process")
	{
		UserName := "null"
		temp_pid := process.processId
		
		needle_1 = %process_name%.exe
		process_name_current := process.Name
		
		;process.GetOwner(UserName, UserDomain)
		;msgbox %process_name_current% y %process_name% - %needle_1% 
		
		StringUpper, process_name_current_M, process_name_current 
		StringUpper, process_name_M, process_name 
		StringUpper, needle_1_M, needle_1 
		
		;msgbox %process_name_current_M% y %process_name_M% - %needle_1_M% 
		
		;IfInString, process_name_current_M, %process_name_M%
		;{
		;	msgbox suave
		;}
		
		if (process_name_current_M == process_name_M or process_name_current_M == needle_1_M)
		{
			;msgbox %temp_pid%
			;msgbox %current_user%
			
			UserName := GetProcessOwner(temp_pid, "owner", false)
			;msgbox %UserName%
		
			if (UserName == "null")
			{
				;
			}
			else
			{
				StringUpper, current_user_M, current_user 
				StringUpper, UserName_M, UserName 
				;msgbox %UserName%
				
				if (current_user_M == UserName_M)
				{
					;msgbox va matar
					Process,Close,%temp_pid%
					Sleep 100
					;msgbox debio matar
					Goto, INIKILLER1
				}
			}
			
		}
	}
}

;[11]=============================================================================================================================================================================================
ProcessKiller(process_name)
{
	;Solo elimina los procesos que creo el usuario actual
	INIKILLER2: ;<--------------
	Sleep 100
	for process in ComObjGet("winmgmts:").ExecQuery("Select * from Win32_Process")
	{
		temp_pid := process.processId
		
		needle_1 = %process_name%.exe
		process_name_current := process.Name
		
		;msgbox %process_name_current% y %process_name% - %needle_1% 
		
		StringUpper, process_name_current_M, process_name_current 
		StringUpper, process_name_M, process_name 
		StringUpper, needle_1_M, needle_1 
		
		;msgbox %process_name_current_M% y %process_name_M% - %needle_1_M% 
		
		;IfInString, process_name_current_M, %process_name_M%
		;{
		;	msgbox suave
		;}
		
		if (process_name_current_M == process_name_M or process_name_current_M == needle_1_M)
		{
			;msgbox %temp_pid% 
			Process,Close,%temp_pid%
			Sleep 100
			;msgbox debio cerrar
			
			Goto, INIKILLER2
		}
	}
	;msgbox salio
}

;[11]=============================================================================================================================================================================================
GetProcessOwner(PID, value, runAsAdmin := false)  {
   static PROCESS_QUERY_INFORMATION := 0x400, TOKEN_QUERY := 0x8
        , TokenUser := 1, TokenOwner := 4, MAX_NAME := 32, isAdmin
    
   if (runAsAdmin && !isAdmin)
      IsAdminChecking(), SetDebugPrivilege(), isAdmin := true
        
   if !hProcess := DllCall("OpenProcess", UInt, PROCESS_QUERY_INFORMATION, UInt, false, UInt, PID, Ptr)
      ;Return ErrorHandling("OpenProcess")
	  sName := "null"
   if !DllCall("Advapi32\OpenProcessToken", Ptr, hProcess, UInt, TOKEN_QUERY, PtrP, hToken)
      ;Return ErrorHandling("OpenProcessToken", hProcess)
	  sName := "null"
   
   tokenType := value = "user" ? TokenUser : TokenOwner
   DllCall("Advapi32\GetTokenInformation", Ptr, hToken, Int, tokenType, Ptr, 0, Int, 0, UIntP, bites)
   VarSetCapacity(buff, bites, 0)
   if !DllCall("Advapi32\GetTokenInformation", Ptr, hToken, Int, tokenType, Ptr, &buff, Int, bites, UIntP, bites)
      ;Return ErrorHandling("GetTokenInformation", hProcess, hToken)
	  sName := "null"
   
   VarSetCapacity(sName, MAX_NAME << !!A_IsUnicode, 0)
   VarSetCapacity(sDomainName, MAX_NAME << !!A_IsUnicode, 0)
   VarSetCapacity(szName, 4, 0), NumPut(MAX_NAME, szName)
   if !DllCall( "Advapi32\LookupAccountSid", Ptr, 0, Ptr, NumGet(buff), Str, sName, Ptr, &szName
                                           , Str, sDomainName, Ptr, &szName, IntP, SID_NAME_USE )
      ;Return ErrorHandling("LookupAccountSid", hProcess, hToken)
	  sName := "null"
   DllCall("CloseHandle", Ptr, hProcess), DllCall("CloseHandle", Ptr, hToken)
   Return sName
}


ErrorHandling(function, hProcess := "", hToken := "")  {
   ;MsgBox, % "Failed: " . function . "`nerror: " . SysError()
   for k, v in [hProcess, hToken]
      ( v && DllCall("CloseHandle", Ptr, v) )
}

IsAdminChecking()  {
   restart := RegExMatch( DllCall("GetCommandLine", "str"), " /restart(?!\S)" )
   if !(A_IsAdmin || restart)  {
      try  {
         if A_IsCompiled
            Run *RunAs "%A_ScriptFullPath%" /restart
         else
            Run *RunAs "%A_AhkPath%" /restart "%A_ScriptFullPath%"
      }
      ExitApp
   }
   if (restart && !A_IsAdmin)
      MsgBox, Failed to run as admin!
}

SetDebugPrivilege(enable := true)  {
   static PROCESS_QUERY_INFORMATION := 0x400, TOKEN_ADJUST_PRIVILEGES := 0x20, SE_PRIVILEGE_ENABLED := 0x2
   
   hProc := DllCall("OpenProcess", UInt, PROCESS_QUERY_INFORMATION, Int, false, UInt, DllCall("GetCurrentProcessId"), Ptr)
   DllCall("Advapi32\OpenProcessToken", Ptr, hProc, UInt, TOKEN_ADJUST_PRIVILEGES, PtrP, token)
   
   DllCall("Advapi32\LookupPrivilegeValue", Ptr, 0, Str, "SeDebugPrivilege", Int64P, luid)
   VarSetCapacity(TOKEN_PRIVILEGES, 16, 0)
   NumPut(1, TOKEN_PRIVILEGES, "UInt")
   NumPut(luid, TOKEN_PRIVILEGES, 4, "Int64")
   NumPut(SE_PRIVILEGE_ENABLED, TOKEN_PRIVILEGES, 12, "UInt")
   DllCall("Advapi32\AdjustTokenPrivileges", Ptr, token, Int, !enable, Ptr, &TOKEN_PRIVILEGES, UInt, 0, Ptr, 0, Ptr, 0)
   res := A_LastError
   DllCall("CloseHandle", Ptr, token)
   DllCall("CloseHandle", Ptr, hProc)
   Return res  ; success — 0
}

SysError(ErrorNum = "")  {
   static FORMAT_MESSAGE_ALLOCATE_BUFFER := 0x100, FORMAT_MESSAGE_FROM_SYSTEM := 0x1000
   (ErrorNum = "" && ErrorNum := A_LastError)
   DllCall("FormatMessage", UInt, FORMAT_MESSAGE_ALLOCATE_BUFFER|FORMAT_MESSAGE_FROM_SYSTEM
                          , UInt, 0, UInt, ErrorNum, UInt, 0, PtrP,  pBuff, UInt, 512, Str, "")
   Return (str := StrGet(pBuff)) ? str : ErrorNum
}