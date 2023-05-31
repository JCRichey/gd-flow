# gd-flow
## Godot 4 Plugin for Modeling the Behavior of Electrical Systems

Any feedback or contribution is appreciated as this is very much a WIP. 

This is part of the code base for a game I’m working on. Since I decided to divide the code base into plugins I thought I may as well host them here and maybe get some feedback. This is in a working state, but I’m not quite happy with the implementation. 

I decided to use Resources as the base since using Child Nodes of the Scene you want to connect comes with some major annoyances. With this approach you can slot the Iflow interface into any class & include the data & functions needed and be able to access the Flow Connection data directly from your level’s hierarchy without having to toggle editable children on everything in your level.

I thought about redoing this making a custom editor window that would pop up on objects in the level showing their child components & allowing you to edit them freely. But I want to finish this game before my hair turns gray

Being able to click on a node in the level and hook all electric logic up with just the node’s names not having to fiddle with it’s children has been a huge improvement. That being said, I still feel like this a clumsy way to do it and am open to feedback & contributions.

I'm more used to C than C# and am new to using Godot, please be gentle
