set wscriptObj = CreateObject("Wscript.Shell")   
If IsProcessEx("SignPressServer.exe") = True Then 

    MessageBox "发现进程"
	wscriptObj.run "taskkill /f /im SignPressServer.exe",hide
End if