# *Secret Hitler game*
Team members:

|        Name        |
|:------------------:|
| Krystian Wiazowski |
| Alexandru Gumaniuc |
|    Rob Veldman     |
|    Nathan Mills    |
|  Andrei Khudiakov  |

## **Description**
This game will be an abbreviation of the original idea. The preview of the rules can be found here: https://www.secrethitler.com/assets/Secret_Hitler_Rules.pdf.

General specifications:
* Card game
* Multiplayer mode only (connected via the same LAN Network)
* 2D view point
* To start, it will require minimum of 5 people

### **Input & Output**

#### **Input**
|        Case         |     Type     |      Conditions      |
|:-------------------:|:------------:|:--------------------:|
| Example exampleName |   `String`   |      not empty       |



#### **Output**
|  Case   |   Type   |
|:-------:|:--------:|
| Example | `String` |


### **Calculations**
|  Case   |    Calculation     |
|:-------:|:------------------:|
| Example | `Example*Example`  |


### **Remarks**
* Input will be validated

## **Class Diagram**
![UML Diagram](InsertDiagramHere.png "insert diagram here")

## **Test Plan**
In this section the testcases will be described to test the application.

### **Test Data**
In the following table you'll find all the data that is needed for testing.

#### **Example**
|    ID    |        Input        |            Code             |
|:--------:|:-------------------:|:---------------------------:|
| example1 | `name1`, 1, `role`  | Example("name1", 1, HITLER) |
| example2 | empty, empty, empty |   Example(null, 0, null)    |


### **Test Cases**
In this section the test cases will be described. Every test case should be executed with the test data as starting
point.

#### **Test Case 1**
**Description:** Testing input validation.

| Step | Input |        Action         |              Expected Output              |
|:----:|:-----:|:---------------------:|:-----------------------------------------:|
|  1   |       | setExample("Example") |                 `Example`                 |
|  2   |       |   setExample(null)    |                   null                    |
|  3   |       |    setExample(" ")    | `Example must be at least 2 letters long` |



