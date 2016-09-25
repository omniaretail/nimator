. ${env:ProgramFiles(x86)}/MSBuild/14.0/Bin/msbuild.exe `
  "./src/Nimator.sln" `
  /verbosity:minimal `
  /p:Configuration=Release `
  /p:RestorePackages=true `
  /t:Rebuild

Remove-Item nuget-package -Recurse -Confirm:$false -ErrorAction Ignore

New-Item -ItemType Directory nuget-package/input/lib/net451
New-Item -ItemType Directory nuget-package/output

Copy-Item src/Nimator/bin/Release/Nimator.dll ./nuget-package/input/lib/net451/
Copy-Item src/Nimator/bin/Release/Nimator.pdb ./nuget-package/input/lib/net451/
Copy-Item src/Nimator/bin/Release/Nimator.xml ./nuget-package/input/lib/net451/

nuget pack ./Nimator.nuspec -basepath ./nuget-package/input/ -outputdirectory ./nuget-package/output/
