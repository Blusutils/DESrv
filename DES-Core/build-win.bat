dotnet build -c Debug -o outs-win --sc -v n --nologo
tar -c -a -f DESrv_x.x.x_Windows.zip outs-win
pause