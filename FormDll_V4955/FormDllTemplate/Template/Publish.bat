c:
cd C:\FormDllProjects\FormDll_V4955\Forms

dotnet publish --output C:\FormDllProjects\DLLPublish\PublishV4955 -p:PublishProfile=FolderProfile

cd C:\FormDllProjects\DLLPublish\PublishV4955
dir

tar -a -c -f ..\..\Zip\Form4955.zip Forms.dll


exit