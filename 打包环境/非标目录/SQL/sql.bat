set /p a=���������ݿ��ַ(Ĭ��localhost):  
set /p b=���������ݿ�˿�(Ĭ��10080):
set /p c=���������ݿ��˻�(Ĭ��test):
set /p d=���������ݿ�����:


@echo off
set CURR_PATH=%cd%
mysql -h %a% -u %c% --password=%d% --port=%b% < jdpark.sql

pause