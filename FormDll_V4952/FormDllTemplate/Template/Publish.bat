c:
cd C:\FormDllProjects\FormDll_V4952\Forms

dotnet publish --output C:\FormDllProjects\DLLPublish\PublishV4952 -p:PublishProfile=FolderProfile

cd C:\FormDllProjects\DLLPublish\PublishV4952
dir

tar -a -c -f ..\..\Zip\Form4952.zip Forms.dll


exit