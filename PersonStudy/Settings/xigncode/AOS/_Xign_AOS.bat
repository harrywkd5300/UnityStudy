@echo off

@set YEAR=%date:~0,4%
@set MONTH=%date:~5,2%
@set DAY=%date:~8,2%
@set HOUR=%time:~0,2%
@set MINUTE=%time:~3,2%
@set SECOND=%time:~6,2%

set PATH_KEY_STORE=%cd%\..\hc.keystore
set PATH_JARSIGNER="C:\Program Files\openjdk\jdk-1.8.0\bin\jarsigner"
set PATH_ZIPALIGN="C:\Users\admin\AppData\Local\Android\Sdk\build-tools\30.0.3\zipalign"
set PATH_APKSIGNER="C:\Users\admin\AppData\Local\Android\Sdk\build-tools\30.0.3\apksigner"

set KEY_ALIAS=homerun_challenge
set KEY_PW=gong1234

set PATH_APK=%cd%\unitybuild.apk
set PATH_FINAL_APK=hc_t_f_v_Dev.apk

@echo.
@echo ------------------------------------------------------
@echo                  Gonggames XignCode3
@echo           ***********************************
@echo                      APK Clean Begin...
@echo ------------------------------------------------------

xaos_sign.exe clean %PATH_APK%
@echo %ERRORLEVEL%
if %ERRORLEVEL% == 0 goto CleanSuccess
@echo.
@echo @@@@@@@@@@@@@@@@@@@@@@@@
@echo               APK Clean ERROR
@echo            --Terminate Process--
@echo @@@@@@@@@@@@@@@@@@@@@@@@
pause
exit

cls
:CleanSuccess
@echo.
@echo ------------------------------------------------------
@echo        APK Clean Complete!!!
@echo ------------------------------------------------------


cls
@echo.
@echo ------------------------------------------------------
@echo                ****************************
@echo                       APK Xign Begin...
@echo ------------------------------------------------------

xaos_sign.exe %PATH_APK%
if %ERRORLEVEL% == 0 goto XignSuccess
@echo.
@echo @@@@@@@@@@@@@@@@@@@@@@@@
@echo              APK Xign ERROR
@echo           --Terminate Process--
@echo @@@@@@@@@@@@@@@@@@@@@@@@
pause
exit

cls
:XignSuccess
@echo.
@echo ------------------------------------------------------
@echo                    APK Xign Complete!!!
@echo ------------------------------------------------------

cls
@echo.
@echo ------------------------------------------------------
@echo                  **************************
@echo                         Jarsign Begin...
@echo ------------------------------------------------------

%PATH_JARSIGNER% -verbose -keystore %PATH_KEY_STORE% %PATH_APK% %KEY_ALIAS% -storepass "gong1234"
if %ERRORLEVEL% == 0 goto JarsignSuccess
@echo %ERRORLEVEL%
@echo.
@echo @@@@@@@@@@@@@@@@@@@@@@@@
@echo        Jarsign ERROR
@echo       --Terminate Process--
@echo @@@@@@@@@@@@@@@@@@@@@@@@
pause
exit


:JarsignSuccess
@echo.
@echo ------------------------------------------------------
@echo                      APK Xign Complete!!!
@echo ------------------------------------------------------

cls
@echo.
@echo -------------------------------------------------
@echo 	Zip Align
@echo -------------------------------------------------
if exist %PATH_FINAL_APK% del %PATH_FINAL_APK%
%PATH_ZIPALIGN% -p -f -v 4 %PATH_APK% %PATH_FINAL_APK%
if %ERRORLEVEL% == 0 goto ZipAlignSuccess

@echo.
@echo -------------------------------------------------
@echo 	Zip Align : ERROR
@echo 	Close Process
@echo -------------------------------------------------
pause
exit

:ZipAlignSuccess
del %PATH_APK%
@echo.
@echo -------------------------------------------------
@echo 	Zip Align Success!!
@echo.
@echo 	Next step is Android Sign
@echo 	Please Wait...		
@echo -------------------------------------------------
TIMEOUT /t 3

cls
@echo.
@echo -------------------------------------------------
@echo 	Android Sign	
@echo -------------------------------------------------
call %PATH_APKSIGNER% sign --ks %PATH_KEY_STORE% --ks-pass pass:%KEY_PW% --key-pass pass:%KEY_PW% --ks-key-alias homerun_challenge --out %PATH_FINAL_APK% %PATH_FINAL_APK%
if %ERRORLEVEL% == 0 goto ApkSignSuccess

@echo.
@echo -------------------------------------------------
@echo 	Android Sign : ERROR
@echo 	Close Process
@echo -------------------------------------------------
pause
exit

:ApkSignSuccess
@echo.
@echo -------------------------------------------------
@echo 	Android Sign Success!!
@echo.
@echo 	Please Wait...		
@echo -------------------------------------------------
@echo.

cls
@echo.
@echo.
@echo -------------------------------------------------
@echo 	Build Success
@echo 	Thank you.
@echo -------------------------------------------------
pause
