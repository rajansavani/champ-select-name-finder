# champ-select-name-finder
Allows you to bypass the hidden names feature added to ranked queue champ select and return an opgg link with the names of your teammates.

## usage info
Make sure you have [dotnet](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-7.0.101-windows-x64-installer?journey=vs-code) installed.
To start launch command prompt as an administrator and go to address of the champSelectNameFinder project folder.

	cd ~/Desktop/champSelectNameFinder
  
then build the project using 

		dotnet build
    
each time you get into a new champ select lobby, type the following

		dotnet run
 
 this will output an opgg link you can place into your browser with the summoner names of your teammates in that lobby
