set WORKSPACE=..
set LUBAN_DLL=%WORKSPACE%\Config\Tools\Luban\Luban.dll
set CONF_ROOT=.

dotnet %LUBAN_DLL% ^
    -t all ^
    -c cs-simple-json ^
    -d json ^
    --conf %CONF_ROOT%\luban.conf ^
    -x outputCodeDir=..\Assets\Scripts\DataConfig\CodeData ^
    -x outputDataDir=..\Assets\Scripts\DataConfig\JsonData

pause