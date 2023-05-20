# Input System 

## General Concepts:

1. User
2. Bindings
    a. Device
    b. Controls
    c. Interactions
    d. Actions
3. Action Method (Scripting Behaviours)

## Workflow for input system

* Direct
* Embedded Action
* Action Asset
* Action Asset and Player Input Component

The one used in this project is the use of **action asset** and its generated script, along with a **wrapper input class** to utilize the script of action asset.

## Concepts for Action Asset:

### InputAction Class

* *Many controls are binded to an input action*
* *The action needs to be **enabled** before it actively listens for the its bindings*
* *Methods to handle controls can be assigned to specific callback defined in input action*
    * *started*
    * *performed*
    * *canceled*


// yet to cover
1. **Control Path Expressions** to store multiple controls binded to an inputaction and the use of *bindings* to access this list.
2. Two type of bindings
    * *normal bindings*
    * *composite bindings*
3. The use of **Binding Mask** to filter out devices and their respective controls
4.  