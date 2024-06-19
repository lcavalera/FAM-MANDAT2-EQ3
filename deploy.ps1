
# Connection Info
$remoteUser = "administrator"
$remoteHost = "172.20.45.99"
# Project files
$ApiProjectDir = ".\Acef.Reasons.API\Acef.Reasons.API.csproj"
$MvcProjectDir = ".\Acef.MVC\Acef.MVC.csproj"
# Local Dirs
$ApiLocalDir = ".\Acef.Reasons.API\bin\Release\net8.0\publish"
$MvcLocalDir = ".\Acef.MVC\bin\Release\net8.0\publish"
# Remote Dirs
$ApiRemoteDir = "C:\inetpub\wwwroot\acef\api"
$MvcRemoteDir = "C:\inetpub\wwwroot\acef\mvc"

#-- 1. Compile & publish the app and its dependencies
dotnet publish $ApiProjectDir -c Release -o $ApiLocalDir
dotnet publish $MvcProjectDir -c Release -o $MvcLocalDir

#-- 2 Stop the webapp pool
ssh $remoteUser@$remoteHost "powershell -command Stop-WebAppPool -Name ACEF_API"
ssh $remoteUser@$remoteHost "powershell -command Stop-WebAppPool -Name ACEF_MVC"

#-- 3. Transfer files
start-sleep 10
scp -r "$($ApiLocalDir)\*" $remoteUser@$($remoteHost):$ApiRemoteDir
start-sleep 10
scp -r "$($MvcLocalDir)\*" $remoteUser@$($remoteHost):$MvcRemoteDir

#-- 4. Start again the webapp pool
ssh $remoteUser@$remoteHost "powershell -command Start-WebAppPool -Name ACEF_API"
ssh $remoteUser@$remoteHost "powershell -command Start-WebAppPool -Name ACEF_MVC"

echo 'Done'
