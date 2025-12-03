c:
cd C:\FormDllProjects\FormDll_V4966\Forms

dotnet publish --output C:\FormDllProjects\DLLPublish\PublishV4966 -p:PublishProfile=FolderProfile

cd C:\FormDllProjects\DLLPublish\PublishV4966
dir

tar -a -c -f ..\..\Zip\Form4966.zip Forms.dll


exit