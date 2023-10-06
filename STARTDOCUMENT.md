# Startdocument C#2 project - AudioMixingApplication

Startdocument van **Monique Sabong**, **Yannieck Blaauw**, **Victor Peters** en **Chris Klunder**.

## Applicatie Beschrijving
De applicatie die ontwikkeld wordt is een virtueel dj station. Hierbij is de bedoeling dat er een of twee nummers afgespeeld kunnen worden en dat je effecten kunt toevoegen aan de nummers (reverb, mid boosting, bass boosting, flanger etc.). De applicatie dient als audio player en als audio mixer.

De applicatie zal ontwikkeld worden met **.NET MAUI**.

## Lay-out


![Scherm1](img/Scherm%201.png "Scherm 1")
![Scherm2](img/Scherm%202.png "Scherm 2")
![Scherm3](img/Scherm%203.png "Scherm 3")
![Scherm4](img/Scherm%204.png "Scherm 4")
![Scherm5](img/Scherm%205.png "Scherm 5")


## Klassendiagram

![Class Diagram](img/classdiagram.png "First Version of the class diagram")

## Testplan

In this section the testcases will be described to test the application.

### Testdata

In de tabellen hieronder worden de data weergegeven die nodig zijn om de applicatie te testen


#### Car

| ID           | Input          | Code                |
| ------------ | -------------- | ------------------- |
| `test`       |  | `new test(image[0])` |
| `test`       | test.png       | `new test(image[1])` |

## Test cases:

Player interactive cases:

| Input                                         | Expected result                                           | Actual result |
| --------------------------------------------- | --------------------------------------------------------- | ------------- |
| User imports song                             | Song is displayed in the DJ menu                          | ...           |
| User imports second song                      | Second song is also displayed in the DJ menu              | ...           |
| User plays one or both song(s)                | Song(s) are played                                        | ...           |
| User pauses one or both song(s)               | Song(s) are paused                                        | ...           |
| User skips a song                             | The next song in the queue plays                          | ...           |
| Gebruiker gaat naar het nummer overzicht      | Alle nummers in de library worden weergegeven.            | ...           |
| Gebruiker past het volume aan                 | Het volume wordt aangepast                                | ...           |
| Gebruiker voegt een effect toe aan het nummer | Het nummer krijgt het effect dat de gebruiker toegevoegd. | ...           |
| Gebruiker maakt een afspeellijst aan          | Er wordt een afspeellijst aangemaakt.                     | ...           |
| Gebruiker voegt een effect toe aan het nummer | Het nummer krijgt het effect dat de gebruiker toegevoegd. | ...           |
| Gebruiker voegt een effect toe aan het nummer | Het nummer krijgt het effect dat de gebruiker toegevoegd. | ...           |
| Gebruiker voegt een effect toe aan het nummer | Het nummer krijgt het effect dat de gebruiker toegevoegd. | ...           |
| Gebruiker voegt een effect toe aan het nummer | Het nummer krijgt het effect dat de gebruiker toegevoegd. | ...           |

Level cases:

| Input                        | Expected result              | Actual result |
| ---------------------------- | ---------------------------- | ------------- |
| Level is opened              | User can see levels on a map | ...           |
| User switches between levels | Level selected is updated    | ...           |
| User chooses level           | Loading screen is shown      | ...           |
| User is past loading screen  | Level is loaded in           | ...           |

Enemy cases:

| Input | Expected result | Actual result |
| ----- | --------------- | ------------- |

## Planning

Om de voortgang van dit project te bewaken, is er een planning gemaakt. Deze planning bevat in grote lijnen hetgeen waarmee wij ons bezig zullen houden per week tot de deadline. Er kan natuurlijk afgeweken worden van deze planning, maar het is het doel om deze in grote lijnen te volgen.

Dit project is begonnen op maandag 2 oktober 2023 en zal eindigen op vrijdag 1 december 2023.

![Planning](img/Planning.png "Project planning")

## Literatuurlijst

-   Britch, D. Gechev I. jconrey (2023, 30 januari) What is .NET MAUI? Geraadpleegd op 30 april 2023, van <https://learn.microsoft.com/en-us/dotnet/maui/what-is-maui?view=net-maui-7.0>
