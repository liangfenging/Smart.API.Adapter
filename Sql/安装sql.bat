set /p a=���������ݿ��ַ(Ĭ��localhost):  
set /p b=���������ݿ�˿�(Ĭ��10080):
set /p c=���������ݿ��˻�(Ĭ��test):
@echo ���������ݿ�����:


@echo off
set CURR_PATH=%cd%
mysql -h %a% -u %c% -p --port=%b% < jdpark.sql

pause