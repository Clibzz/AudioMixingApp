# AudioMixingApp

[> Startdocument van **Monique Sabong**, **Yannieck Blaauw**, **Victor Peters** en **Chris Klunder**.](./documents/STARTDOCUMENT.md)

## Installatie handleiding
Hieronder staan de stappen die genomen worden om gebruik te kunnen maken van de applicatie.

**Disclaimer: Voor het project word .NET 6.0.417 gebruikt. Dit is inmiddels niet meer de meest recente versie, dit is namelijk 6.0.418. Als deze versie gebruikt wil worden moeten de twee "global.json" bestanden geupdate worden naar deze .NET versie**

1.	Zorg ervoor dat Windows developer mode aan staat;
2.	Installeer VS Studio 2022 inclusief de “.net desktop development” en de “.NET Multi-platform App UI development” module;
3.	Pull de repository;
4.	Installeer .net 6.0.417 van https://dotnet.microsoft.com/en-us/download/dotnet/6.0 (staat onder 6.0.25, afhankelijk van het systeem is waarschijnlijk de Windows x64 versie correct);
5.	Navigeer in een terminal naar het pad van de project: hetzelfde niveau als het ".sln" bestand
6.	Run het commando "dotnet --version". Dit zou 6.0.417 moeten weergeven. Als dit zo is, ga naar stap 8;
7.	Als de dotnet versie niet 6.0.417 is, verwijder de andere .net versies tot dat dit commando .net 6.0.417 zegt (De .net versies staan vaak in de map “C:\Program Files\dotnet\sdk”);
8.	Run het commando "dotnet workload install Maui”;
9.	Open het project in Visual Studio en run het project.
