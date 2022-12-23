@echo off

:: �Ʒ��� �ط� ǥ�õ� ������ ã�� �ش� ������Ʈ�� �°� �����Ͽ� ������ֽø� �˴ϴ�.

:: [START] ��� ���� ���� �ءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءء�
:: �Ʒ��� ������ ���� ������Ʈ ������ �ִ� ��θ� �����ϼ���.

D:
cd GameCode\NFT_Project\Trunk\Client

:: [END] ��� ���� ���� �ءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءء�

mode con cols=150
color 0A

setlocal

:FIRSTSTEP
cls

:: [START] ����Ƽ ������Ʈ ��� ���� �ءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءء�
:: ��ġ�� ����Ƽ�� ������ ��θ� �����ּ���.

set defaultPath="D:\UnityHub\2021.3.11f1\Editor\Unity.exe" -batchmode -projectPath 

:: [END] ����Ƽ ������Ʈ ��� ���� �ءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءء�

echo ================================================================================================================
echo.
echo                     AssetBundles Patch Program_HC(ver1.0.0)
echo.
echo ================================================================================================================
echo.
echo   �� ���α׷��� ������Ʈ�� ���� ��ġ�� Unity�� Batch��带 ���ؼ� �̱� ���� ������� ���α׷� �Դϴ�.
echo.
echo   �� ���α׷��� ��������ν�, Unity ���� ���� Bundle Patch�� ������ �� �ֽ��ϴ�.
echo.
echo   �� ��ġ �� ����Ƽ�� �� �������ּ���.
echo.
echo    ���� �� ���� ��ġ�� �����ϼ���.
echo.
echo.
echo    [ 1. AM ]
echo    [ 2. QA ]
echo    [ 3. Live ]
echo    [ 4. Dev ]
echo    [ 5. iOS ]
echo.
echo ================================================================================================================

:: str ���� �ʱ�ȭ
set patchPath=0

:REDO_A
set /p patchPath=   1 ~ 5 �� �����ϼ��� :

:: �ɺ� ���� ����
if "%patchPath%" == "1" (
set symbolsList=_BUILD_AM_
)
if "%patchPath%" == "2" (
set symbolsList=_BUILD_QA_;_ACTIVE_CHEAT_
)
if "%patchPath%" == "3" (
set symbolsList=_BUILD_LIVE_
)
if "%patchPath%" == "4" (
set symbolsList=_BUILD_QA_;_ACTIVE_CHEAT_
)
if "%patchPath%" == "5" (
set symbolsList=_BUILD_LIVE_
)

:: ��ġ ��� ����
if "%patchPath%" == "1" (
set patchPath=AM
) else if "%patchPath%" == "2" (
set patchPath=QA
) else if "%patchPath%" == "3" (
set patchPath=LIVE
) else if "%patchPath%" == "4" (
set patchPath=DEV
) else if "%patchPath%" == "5" (
set patchPath=iOS
) else (
echo.
echo    �߸��� ���Դϴ�. �ٽ� �Է��ϼ���.
echo.
echo ================================================================================================================
goto REDO_A
)

:CONTINUE_A

echo.
echo ================================================================================================================
echo.
echo.
echo    �ȵ���̵� / ���� �� �����ϼ���.
echo.
echo.
echo    [ 1. AOS - �ȵ���̵� ]
echo    [ 2. iOS - ���� ]
echo.
echo ================================================================================================================

:REDO_B
set /p patchType=   1 ~ 2 �� �����ϼ��� :

if "%patchType%" == "1" (
set patchType=Android
) else if "%patchType%" == "2" (
set patchType=iOS
) else (
echo.
echo    �߸��� ���Դϴ�. �ٽ� �Է��ϼ���.
echo.
echo ================================================================================================================
goto REDO_B
)

:CONTINUE_B

echo.
echo ================================================================================================================

:: [START] ����Ƽ ������Ʈ ��� ���� �ءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءء�
:: PatchPatch ��θ� �������ּ���.

set patchPath=D:\GameCode\NFT_Project\Trunk\Client

:: [END] ����Ƽ ������Ʈ ��� ���� �ءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءء�

echo.
echo    ���õ� ��ġ ������ 
echo    [ %defaultPath%"%patchPath%" ] �Դϴ�.
echo.

echo.
echo ================================================================================================================
echo.
echo      ���� ������ �Ʒ� ���ÿ� ���� �������ּ���.
echo.
echo      Ex)    0.9.2   /    1.0.000    /    1.5.2
echo.
echo.
echo ================================================================================================================
echo.
set /p  versionCode=   Version :

timeout 3

cls

echo.
echo ================================================================================================================
echo.
echo      ������Ʈ ������Ʈ�� �����մϴ�...
echo.
echo      SVN ������Ʈ ��, �浹�� �߻��� ��� �������� ó�����ּž� ���� �ܰ�� �Ѿ�ϴ�.
echo.
echo ================================================================================================================
echo.

:: SVN ��� ����
call C:\"Program Files"\TortoiseSVN\bin\TortoiseProc.exe /command:update /path:%patchPath%  /notempfile /closeonend:2

:: ��ġ ���� ���� ���
set settingsPath=0

:: ���� ��ġ �̴� �Լ�
set patchMethod=0

echo.
echo ================================================================================================================
echo.
echo      ��ġ�� �����մϴ�...
echo.
echo      ���� �̴� ���, �ð��� �ټ� �ҿ�Ǵ�, ��ġ������ �����ִ��� ���� ���� ��ٷ��ּ���.
echo.
echo ================================================================================================================
echo.
echo   [ ���õ� ���� ]
echo.
echo   [DefaultPath]
echo   %defaultPath%
echo.
echo   [PatchPath]
echo   %patchPath%
echo.
echo   [VersionCode]
echo   %versionCode%
echo.
echo   [Symbols]
echo   %symbolsList%
echo.
echo ================================================================================================================
echo.
echo    [ ����ð� ]
echo.
echo    �� ��ġ �ܼ��� ��������ν�, Hold On�� ���� ���ʿ��� �۾� ���� ����Ǳ� ������ �ҿ�Ǵ� �ð��� �پ��ϴ�.
echo.
echo    1. ��ġ ����
echo          ���� �ҿ� �ð� : 1~5min
echo.
echo    2. ���� ��ġ
echo          ���� �ҿ� �ð� : 2~8min
echo. 
echo    �� �̻��ϰ� ��ġ �ð��� ���� �ɸ��� ���, �۾������ڸ� �����Ͽ� ���μ����� Ȯ���ϼ���.
echo         Window ��� ó���� �ȿ� Unity�� ����Ǿ� ���� �ʰų� CPU �������� 0~1% ���� ���� ���°� �����Ǹ�, ��ġ�� ������� �ʰ� ���� Ȯ���� �����ϴ�.
echo         ���� ��Ȳ�� �߻��Ǹ�, ��ġ������ ���� �� ������ �ùٸ��� �ٽ� �����Ͽ� ������Ͻñ� �ٶ��ϴ�.
echo.
echo ================================================================================================================
echo.

:CONTINUE_PATCH

:: [START] ���� ���� gBuild���� ��� ���� �ءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءء�

if %patchType% == Android set settingsPath=\Settings\Build\aOS\aOS_10_abPatchSetup.gBuild
if %patchType% == iOS set settingsPath=\Settings\Build\iOS\iOS_10_abPatchSetup.gBuild

:: [END] ���� ���� gBuild���� ��� ���� �ءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءء�

set patchMethod= -executeMethod Gong.Build.AutoBuilder.BuildFromArgs --BuildScriptSymbols "%symbolsList%" --BuildSettingFile "%patchPath%%settingsPath%"

if %patchType% == Android set patchMethod= -executeMethod Gong.Build.AutoBuilder.BuildFromArgs --BuildTargetGroup "Android" --BuildTarget "Android" --BuildScriptSymbols "%symbolsList%" --BuildVersion "%versionCode%" --BuildSettingFile "%patchPath%%settingsPath%"
if %patchType% == iOS 	set patchMethod= -executeMethod Gong.Build.AutoBuilder.BuildFromArgs --BuildTargetGroup "iPhone" --BuildTarget "iPhone" --BuildScriptSymbols "%symbolsList%" --BuildVersion "%versionCode%" --BuildSettingFile "%patchPath%%settingsPath%"


:: ��ġ ���� ���� �ð� üũ
timeout 2
echo.
echo    ���� ��ġ ������ �����մϴ�.
echo.
echo    [ ��ġ ���� ���� �ð� : %time% ]
set h0=%time:~0,2%
set m0=%time:~3,2%
set s0=%time:~6,2%
echo.
echo   Command : [ %defaultPath%"%patchPath%"%patchMethod% -quit ]
%defaultPath%%patchPath%%patchMethod% -quit
echo.

:: ��ġ ���� ���� �ð� üũ
echo    [ ��ġ ���� ���� �ð� : %time% ]
set h1=%time:~0,2%
set m1=%time:~3,2%
set s1=%time:~6,2%

:: ��ġ ���� �۾� �ð� ���
set /a h=h1-h0
set /a m=m1-m0
if %m% lss 0 (
set /a h=h-1
set /a m=m+60
)
set /a s=s1-s0
if %s% lss 0 (
set /a m=m-1
set /a s=s+60
)

:: ���
echo.
echo    [ ���� ��ġ ���ÿ� �ҿ�� �ð��� : %h% �ð� %m% �� %s% �� �Դϴ�. ]
echo.

:PatchArea

:: [START] ���� ���� gBuild���� ��� ���� �ءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءء�

if %patchType% == Android set settingsPath=\Settings\Build\aOS\aOS_11_abPatchOutput.gBuild
if %patchType% == iOS set settingsPath=\Settings\Build\iOS\iOS_11_abPatchOutput.gBuild

:: [END] ���� ���� gBuild���� ��� ���� �ءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءء�

set patchMethod= -executeMethod Gong.Build.AutoBuilder.BuildFromArgs --BuildVersion "%versionCode%" --BuildSettingFile "%patchPath%%settingsPath%"

:: ��ġ ���� �ð� üũ
timeout 2
echo.
echo    ���� ��ġ�� �����մϴ�.
echo.
echo    [ ��ġ ���� �ð� : %time% ]
set h0=%time:~0,2%
set m0=%time:~3,2%
set s0=%time:~6,2%
echo.
echo   Command : [ %defaultPath%"%patchPath%"%patchMethod% -quit ]
%defaultPath%%patchPath%%patchMethod% -quit
echo.

:: ��ġ ���� �ð� üũ
echo    [ ��ġ ���� �ð� : %time% ]
set h1=%time:~0,2%
set m1=%time:~3,2%
set s1=%time:~6,2%

:: ��ġ �۾� �ð� ���
set /a h=h1-h0
set /a m=m1-m0
if %m% lss 0 (
set /a h=h-1
set /a m=m+60
)
set /a s=s1-s0
if %s% lss 0 (
set /a m=m-1
set /a s=s+60
)

:: ���
echo.
echo    [ ���� ��ġ �ҿ� �ð��� : %h% �ð� %m% �� %s% �� �Դϴ�. ]
echo.
echo ================================================================================================================
echo.
echo    �۾��� �Ϸ�Ǿ����ϴ�. ���� ������ ���� ���α׷��� �����մϴ�.
echo.

start %patchPath%/PatchResources/assetbundles/%patchType%

:CMDQUIT
PAUSE