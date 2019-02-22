set /p a=请输入数据库地址(默认localhost):  
set /p b=请输入数据库端口(默认10080):
set /p c=请输入数据库账户(默认test):
@echo 请输入数据库密码:


@echo off
set CURR_PATH=%cd%
mysql -h %a% -u %c% -p --port=%b% < jdpark.sql

pause