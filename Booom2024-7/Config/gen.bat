set WORKSPACE=..
set LUBAN_DLL=%WORKSPACE%\Config\Tools\Luban\Luban.dll
set CONF_ROOT=.

dotnet %LUBAN_DLL% ^
    -t all ^
    -c cs-simple-json ^
    -d json ^
    --conf %CONF_ROOT%\luban.conf ^
    -x outputDataDir=..\Assets\Resources\Datas\Config ^
    -x outputCodeDir=..\Assets\Scripts\Config 

pause