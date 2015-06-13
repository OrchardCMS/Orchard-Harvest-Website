if "%~1"=="" build Precompiled
msbuild /t:%~1 Orchard.proj

