@set "sitePath=%~dp0"
 
@echo ɾ��վ��
@C:\Windows\System32\inetsrv\appcmd.exe delete site /site.name:"Smart.API.Adapter.Api"
@C:\Windows\System32\inetsrv\appcmd.exe delete apppool /apppool.name:"Smart.API.Adapter.Api"

Pause