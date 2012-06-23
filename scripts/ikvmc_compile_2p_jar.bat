@echo off

set TUPROLOG_VERSION=2.5.0
set JAR_FILE=2p.jar
set ASSEMBLY_NAME=alice.tuprolog
set DLL_FILE=%ASSEMBLY_NAME%.dll

set IKVM_HOME=C:\RRutt\UTIL\ikvm-7.0.4335.0
set IKVMC="%IKVM_HOME%\bin\ikvmc.exe"

set IKVMC_OPTIONS=-assembly:%ASSEMBLY_NAME% -version:%TUPROLOG_VERSION%.0

pushd ..\lib
cd

echo Compiling %JAR_FILE% to %DLL_FILE% ...

%IKVMC% %IKVMC_OPTIONS% %JAR_FILE% -out:%DLL_FILE%

popd
cd

echo Done..

pause