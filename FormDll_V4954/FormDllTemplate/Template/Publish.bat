c:
cd C:\FormDllProjects\FormDll_V4954\Forms

dotnet publish --output C:\FormDllProjects\DLLPublish\PublishV4954 -p:PublishProfile=FolderProfile

cd C:\FormDllProjects\DLLPublish\PublishV4954
dir

tar -a -c -f ..\..\Zip\Form4954.zip Forms.dll


exit