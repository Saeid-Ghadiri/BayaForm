c:
cd C:\FormDllProjects\FormDll_V4956\Forms

dotnet publish --output C:\FormDllProjects\DLLPublish\PublishV4956 -p:PublishProfile=FolderProfile

cd C:\FormDllProjects\DLLPublish\PublishV4956
dir

tar -a -c -f ..\..\Zip\Form4956.zip Forms.dll


exit