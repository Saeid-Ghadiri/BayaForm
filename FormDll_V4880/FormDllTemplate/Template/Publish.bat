c:
cd C:\FormDllProjects\FormDll_V4880\Forms

dotnet publish --output C:\FormDllProjects\DLLPublish\PublishV4880 -p:PublishProfile=FolderProfile

cd C:\FormDllProjects\DLLPublish\PublishV4880
dir

tar -a -c -f ..\..\Zip\Form4880.zip Forms.dll


exit