@set "sitePath=%~dp0"
 
@echo ÐÂ½¨³ÌÐò³Ø
@C:\Windows\System32\inetsrv\appcmd.exe add apppool /name:"Smart.API.Adapter.Api" /managedRuntimeVersion:"v4.0"
@C:\Windows\System32\inetsrv\appcmd.exe add site /name:"Smart.API.Adapter.Api" /bindings:http/*:9901: /applicationDefaults.applicationPool:"Smart.API.Adapter.Api" /physicalPath:%sitePath%
 
 
echo y|cacls %cd% /t /e /g everyone:f
 
Pause