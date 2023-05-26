# Stealth Fuhrer - C# .NET MAUI

## Introduction

Stealth Fuhrer is a social deduction game for 5-10 people about finding and stopping Hitler and
a fascist before they take over the government. The game is set in 1930s Germany and revolves around
the political struggle between the liberal and fascist teams.

In "Stealth Fuhrer", players are randomly assigned roles as either liberals or fascists, with one player being
secretly designated as "Secret Hitler." The liberals' goal is to pass liberal policies and identify and
eliminate Hitler, while the fascists aim to pass fascist policies and help Hitler rise to power.

Throughout the game, players participate in rounds where they propose and vote on policies, while trying to
gather information about each other's allegiances. The game involves deception, negotiation, and deduction as
players attempt to gain control and achieve their objectives.

"Stealth Fuhrer" is typically played with a large group of people, making it a popular choice for parties and
social gatherings. It offers an engaging and immersive experience as players navigate the delicate balance
between trust and suspicion.

### *Based on "Secret Hitler" by Max Temkin, Mike Boxleiter, Tommy Maranges, and Mac Schubert*

We would like to acknowledge and give full credit to the original authors of "Secret Hitler,"
Max Temkin, Mike Boxleiter, Tommy Maranges, and Mac Schubert, for their innovative game design and concept.
Stealth Fuhrer is a clone of "Secret Hitler" and has been created with their game as a source of inspiration.

Please note that Stealth Fuhrer is not affiliated with or endorsed by the original creators of "Secret Hitler."

To learn more about the original game and its creators, please visit their official website:
https://www.secrethitler.com/.

### About the developers

This game is being developed part of the course "C#2" at NHL Stenden University of Applied Sciences.
Below you can find the team members:

|        Name        |
|:------------------:|
| Krystian Wiazowski |
| Alexandru Gumaniuc |
|    Rob Veldman     |
|    Nathan Mills    |
|  Andrei Khudiakov  |

## Description

This game will be an abbreviation of the original idea. The preview of the rules can be found
here: https://www.secrethitler.com/assets/Secret_Hitler_Rules.pdf.

*General specifications:*

* Card game
* 2D view point
* Multiplayer mode only (connected via the same LAN Network)
* To start, it will require a minimum of **5 people** and a maximum of **10 people**

### Class Diagram

![ClassDiagram](./Assets/ClassDiagramStealthFurher.png "Class Diagram")

### Design Mockups

#### Main Menu

![Main Menu](./Assets/MainMenu.png "Main Menu")

#### Lobby

*When joining/creating a new game*
![Lobby](./Assets/Lobby.jpg "Lobby")
*After joining/creating a new game*
![Lobby](./Assets/Lobby_2.jpg "Lobby")

#### Game

![Game](./Assets/Game.png "Game")

#### Game - Voting

![Game - Voting](./Assets/GameVoting.png "Game - Voting")

## Input & Output

*The section below is yet to be updated*

### Input

|        Case         |   Type   | Conditions |
|:-------------------:|:--------:|:----------:|
| Example exampleName | `String` | not empty  |

### Output

|  Case   |   Type   |
|:-------:|:--------:|
| Example | `String` |

### Calculations

|  Case   |    Calculation    |
|:-------:|:-----------------:|
| Example | `Example*Example` |

### Remarks

* Input will be validated

## Class Diagram

(The class diagram will be added here later)

![UML Diagram](InsertDiagramHere.png "insert diagram here")

## Test Plan

In this section the testcases will be described to test the application.

### **Test Data**

In the following table you'll find all the data that is needed for testing.

#### Example

|    ID    |        Input        |            Code             |
|:--------:|:-------------------:|:---------------------------:|
| example1 | `name1`, 1, `role`  | Example("name1", 1, HITLER) |
| example2 | empty, empty, empty |   Example(null, 0, null)    |

### Test Cases

In this section the test cases will be described. Every test case should be executed with the test data as starting
point.

#### Test Case 1

Description: Testing input validation.

| Step | Input |        Action         |              Expected Output              |
|:----:|:-----:|:---------------------:|:-----------------------------------------:|
|  1   |       | setExample("Example") |                 `Example`                 |
|  2   |       |   setExample(null)    |                   null                    |
|  3   |       |    setExample(" ")    | `Example must be at least 2 letters long` |



