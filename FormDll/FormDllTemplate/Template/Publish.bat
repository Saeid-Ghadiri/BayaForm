c:
cd C:\FormDllProjects\FormDll_V5050\Forms

dotnet publish --output C:\FormDllProjects\DLLPublish\PublishV5050 -p:PublishProfile=FolderProfile

cd C:\FormDllProjects\DLLPublish\PublishV5050
dir

tar -a -c -f ..\..\Zip\Form5050.zip Forms.dll


exit