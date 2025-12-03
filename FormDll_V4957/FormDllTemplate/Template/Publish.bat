c:
cd C:\FormDllProjects\FormDll_V4957\Forms

dotnet publish --output C:\FormDllProjects\DLLPublish\PublishV4957 -p:PublishProfile=FolderProfile

cd C:\FormDllProjects\DLLPublish\PublishV4957
dir

tar -a -c -f ..\..\Zip\Form4957.zip Forms.dll


exit