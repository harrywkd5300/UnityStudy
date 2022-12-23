@echo off

:: 아래에 ※로 표시된 영역을 찾아 해당 프로젝트에 맞게 세팅하여 사용해주시면 됩니다.

:: [START] 경로 세팅 영역 ※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※
:: 아래에 번들을 뽑을 프로젝트 폴더가 있는 경로를 설정하세요.

D:
cd GameCode\NFT_Project\Trunk\Client

:: [END] 경로 세팅 영역 ※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※

mode con cols=150
color 0A

setlocal

:FIRSTSTEP
cls

:: [START] 유니티 프로젝트 경로 세팅 ※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※
:: 설치된 유니티의 버전과 경로를 맞춰주세요.

set defaultPath="D:\UnityHub\2021.3.11f1\Editor\Unity.exe" -batchmode -projectPath 

:: [END] 유니티 프로젝트 경로 세팅 ※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※

echo ================================================================================================================
echo.
echo                     AssetBundles Patch Program_HC(ver1.0.0)
echo.
echo ================================================================================================================
echo.
echo   이 프로그램은 프로젝트의 번들 패치를 Unity의 Batch모드를 통해서 뽑기 위해 만들어진 프로그램 입니다.
echo.
echo   이 프로그램을 사용함으로써, Unity 실행 없이 Bundle Patch를 진행할 수 있습니다.
echo.
echo   ※ 패치 전 유니티를 꼭 종료해주세요.
echo.
echo    다음 중 뽑을 패치를 선택하세요.
echo.
echo.
echo    [ 1. AM ]
echo    [ 2. QA ]
echo    [ 3. Live ]
echo    [ 4. Dev ]
echo    [ 5. iOS ]
echo.
echo ================================================================================================================

:: str 변수 초기화
set patchPath=0

:REDO_A
set /p patchPath=   1 ~ 5 중 선택하세요 :

:: 심볼 세팅 영역
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

:: 패치 대상 선정
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
echo    잘못된 값입니다. 다시 입력하세요.
echo.
echo ================================================================================================================
goto REDO_A
)

:CONTINUE_A

echo.
echo ================================================================================================================
echo.
echo.
echo    안드로이드 / 애플 중 선택하세요.
echo.
echo.
echo    [ 1. AOS - 안드로이드 ]
echo    [ 2. iOS - 애플 ]
echo.
echo ================================================================================================================

:REDO_B
set /p patchType=   1 ~ 2 중 선택하세요 :

if "%patchType%" == "1" (
set patchType=Android
) else if "%patchType%" == "2" (
set patchType=iOS
) else (
echo.
echo    잘못된 값입니다. 다시 입력하세요.
echo.
echo ================================================================================================================
goto REDO_B
)

:CONTINUE_B

echo.
echo ================================================================================================================

:: [START] 유니티 프로젝트 경로 세팅 ※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※
:: PatchPatch 경로를 세팅해주세요.

set patchPath=D:\GameCode\NFT_Project\Trunk\Client

:: [END] 유니티 프로젝트 경로 세팅 ※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※

echo.
echo    선택된 패치 영역은 
echo    [ %defaultPath%"%patchPath%" ] 입니다.
echo.

echo.
echo ================================================================================================================
echo.
echo      번들 버전을 아래 예시와 같이 세팅해주세요.
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
echo      프로젝트 업데이트를 진행합니다...
echo.
echo      SVN 업데이트 중, 충돌이 발생한 경우 수동으로 처리해주셔야 다음 단계로 넘어갑니다.
echo.
echo ================================================================================================================
echo.

:: SVN 경로 설정
call C:\"Program Files"\TortoiseSVN\bin\TortoiseProc.exe /command:update /path:%patchPath%  /notempfile /closeonend:2

:: 패치 세팅 파일 경로
set settingsPath=0

:: 번들 패치 뽑는 함수
set patchMethod=0

echo.
echo ================================================================================================================
echo.
echo      패치를 진행합니다...
echo.
echo      새로 뽑는 경우, 시간이 다소 소요되니, 배치파일이 멈춰있더라도 끄지 말고 기다려주세요.
echo.
echo ================================================================================================================
echo.
echo   [ 선택된 정보 ]
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
echo    [ 예상시간 ]
echo.
echo    ※ 배치 콘솔을 사용함으로써, Hold On과 같은 불필요한 작업 없이 진행되기 때문에 소요되는 시간이 줄어듭니다.
echo.
echo    1. 패치 세팅
echo          예상 소요 시간 : 1~5min
echo.
echo    2. 번들 패치
echo          예상 소요 시간 : 2~8min
echo. 
echo    ※ 이상하게 패치 시간이 오래 걸리는 경우, 작업관리자를 실행하여 프로세스를 확인하세요.
echo         Window 명령 처리기 안에 Unity가 실행되어 있지 않거나 CPU 점유율이 0~1% 가량 낮은 상태가 유지되면, 패치가 진행되지 않고 있을 확률이 높습니다.
echo         위의 상황이 발생되면, 배치파일을 종료 후 세팅을 올바르게 다시 수정하여 재실행하시기 바랍니다.
echo.
echo ================================================================================================================
echo.

:CONTINUE_PATCH

:: [START] 번들 세팅 gBuild파일 경로 세팅 ※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※

if %patchType% == Android set settingsPath=\Settings\Build\aOS\aOS_10_abPatchSetup.gBuild
if %patchType% == iOS set settingsPath=\Settings\Build\iOS\iOS_10_abPatchSetup.gBuild

:: [END] 번들 세팅 gBuild파일 경로 세팅 ※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※

set patchMethod= -executeMethod Gong.Build.AutoBuilder.BuildFromArgs --BuildScriptSymbols "%symbolsList%" --BuildSettingFile "%patchPath%%settingsPath%"

if %patchType% == Android set patchMethod= -executeMethod Gong.Build.AutoBuilder.BuildFromArgs --BuildTargetGroup "Android" --BuildTarget "Android" --BuildScriptSymbols "%symbolsList%" --BuildVersion "%versionCode%" --BuildSettingFile "%patchPath%%settingsPath%"
if %patchType% == iOS 	set patchMethod= -executeMethod Gong.Build.AutoBuilder.BuildFromArgs --BuildTargetGroup "iPhone" --BuildTarget "iPhone" --BuildScriptSymbols "%symbolsList%" --BuildVersion "%versionCode%" --BuildSettingFile "%patchPath%%settingsPath%"


:: 패치 세팅 시작 시간 체크
timeout 2
echo.
echo    번들 패치 세팅을 시작합니다.
echo.
echo    [ 패치 세팅 시작 시간 : %time% ]
set h0=%time:~0,2%
set m0=%time:~3,2%
set s0=%time:~6,2%
echo.
echo   Command : [ %defaultPath%"%patchPath%"%patchMethod% -quit ]
%defaultPath%%patchPath%%patchMethod% -quit
echo.

:: 패치 세팅 종료 시간 체크
echo    [ 패치 세팅 종료 시간 : %time% ]
set h1=%time:~0,2%
set m1=%time:~3,2%
set s1=%time:~6,2%

:: 패치 세팅 작업 시간 계산
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

:: 결과
echo.
echo    [ 번들 패치 세팅에 소요된 시간은 : %h% 시간 %m% 분 %s% 초 입니다. ]
echo.

:PatchArea

:: [START] 번들 세팅 gBuild파일 경로 세팅 ※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※

if %patchType% == Android set settingsPath=\Settings\Build\aOS\aOS_11_abPatchOutput.gBuild
if %patchType% == iOS set settingsPath=\Settings\Build\iOS\iOS_11_abPatchOutput.gBuild

:: [END] 번들 세팅 gBuild파일 경로 세팅 ※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※

set patchMethod= -executeMethod Gong.Build.AutoBuilder.BuildFromArgs --BuildVersion "%versionCode%" --BuildSettingFile "%patchPath%%settingsPath%"

:: 패치 시작 시간 체크
timeout 2
echo.
echo    번들 패치를 시작합니다.
echo.
echo    [ 패치 시작 시간 : %time% ]
set h0=%time:~0,2%
set m0=%time:~3,2%
set s0=%time:~6,2%
echo.
echo   Command : [ %defaultPath%"%patchPath%"%patchMethod% -quit ]
%defaultPath%%patchPath%%patchMethod% -quit
echo.

:: 패치 종료 시간 체크
echo    [ 패치 종료 시간 : %time% ]
set h1=%time:~0,2%
set m1=%time:~3,2%
set s1=%time:~6,2%

:: 패치 작업 시간 계산
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

:: 결과
echo.
echo    [ 번들 패치 소요 시간은 : %h% 시간 %m% 분 %s% 초 입니다. ]
echo.
echo ================================================================================================================
echo.
echo    작업이 완료되었습니다. 번들 폴더를 열고 프로그램을 종료합니다.
echo.

start %patchPath%/PatchResources/assetbundles/%patchType%

:CMDQUIT
PAUSE