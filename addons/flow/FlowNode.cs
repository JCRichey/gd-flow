using Godot;
using System;
using System.Collections.Generic;

/*
    Any feedback or contribution is appreciated as this is very much a WIP. 
    I'm more used to C than C# and am new to using Godot, please be gentle

    I made this speficially for modeling the behavior of electrical systems for a
    an immersive sim game I've been working on. It's in a functional state but I feel
    it could be improved alot

    Guide
    ____________________________________________________________
    Any Scene you want to use: Implement IFlow Interface, then add these:

    [Export]
	FlowNode FlowNode_;
	public FlowNode GetFlowNode() => FlowNode_; 
    public void FlowUpdate(bool state)
    {

    }

    Make sure to call the Init() method in your Scene's _Ready!

    The FlowNode Resource for some reason will not show up in the menu if you try
    to set it directly in the Export parameter. I recommend making a dummy instance of 
    it, then dragging that onto the Export parammeter and making it unique & local to scene,
    then set the connections.
*/


public interface IFlow
{
    public FlowNode GetFlowNode();
    public void FlowUpdate(bool state);
}

[Tool]
public partial class FlowNode : Resource
{
    Node Owner;

    [Export]
    string InputConnectionName = "";
    public FlowNode InputConnection;
    public bool  HasInputConnection; 

    [Export]
    string[] OutputConnectionNames;
    public List<FlowNode> OutputConnections = new List<FlowNode>();
    public bool HasOutputConnections;

    
    public bool InputPower
    {
        get
        {
            if(!HasInputConnection){ // Root Node == Power Source
                return true;
            }
            else
            {
                return InputConnection.OutputPower;
            }
        }
    }

    [Export] public bool On = true;

    public bool OutputPower
    {
        get
        {
            return InputPower & On; 
        }
    }
   

    bool Initialized = false;
    public void Init(Node owner)
    {
        Initialized = true; // Make sure owner calls Init()
        Owner = owner;

        bool GetInputConnection()
        {
            // If no Input Node Name given in Export
            if(InputConnectionName.Length < 1){ // Can't use null check for this. I think because it's an Export? Not sure
                return false;
            }

            // Get node from Scene Tree
            Node inputNode = NodeFromSceneRoot(InputConnectionName, owner);

            #if DEBUG
            if(inputNode == null){
                GD.PrintErr($"{owner.Name}->FlowNode->{InputConnectionName}: Invalid Input Node! Reason: Could not find node in Scene Tree");
                return false;
            }
            // Check if Input Node implements IFlow
            if(!(inputNode is IFlow)){
                GD.PrintErr($"{owner.Name}->FlowNode->{InputConnectionName}: Invalid Input Node! Reason: Given Input node does not implement IFlow");
                return false;
            }
            #endif

            InputConnection = ((IFlow)inputNode).GetFlowNode();
            return true;      

        } HasInputConnection = GetInputConnection();
        
        bool GetOutputConnections()
        {  
            // Any Child Connections set?
            if(OutputConnectionNames.Length < 1){
                return false;
            }

            for(int i = 0; i < OutputConnectionNames.Length; i++)
            {
                string outputName = OutputConnectionNames[i];
                Node   outputNode = NodeFromSceneRoot(outputName, owner);

                #if DEBUG
                if(outputNode == null){
                    GD.PrintErr($"{owner.Name}->FlowNode->GetChildren(): Invalid Output Connection '{outputName}'! Reason: Could not find node in Scene Tree");
                    return false;
                }
                if(!(outputNode is IFlow)){
                    GD.PrintErr($"{owner.Name}->FlowNode->GetChildren(): Invalid Output Connection '{outputName}'! Reason: Does not implement IFlow");
                    return false;
                }
                #endif

                OutputConnections.Add(((IFlow)outputNode).GetFlowNode());
            }
            return true;

        } HasOutputConnections = GetOutputConnections();
        
        #if DEBUG
        //DebugPrint();
        #endif
        //CascadeUpdate(this);
    }

    // This FEELS like a bad way to do things, but it works so far. Dodges all the issues with signals & broken typed array exports and all that so yea
    static Node NodeFromSceneRoot(string name, Node owner)
    {
        var root = owner.GetNode("/root");

        #if DEBUG
        // Is name valid?
        if(name.Length < 1){
            GD.PrintErr($"{owner.Name}->FlowNode->NodeFromSceneRoot: Invalid Name '{name}'");
            return null;
        }
        // Can we find scene root?
        if(root == null){
            GD.PrintErr($"{owner.Name}->FlowNode->NodeFromSceneRoot: Could not find scene root. Will this ever trigger?");
            return null;
        }
        #endif

        // Try to find child from Scene Root
        var node = root.FindChild(name, true, false); 

        #if DEBUG
        if(node == null){
            GD.PrintErr($"{owner.Name}->ElectricData->NodeFromSceneRoot: Node not found: '{name}', Did you get the spelling right?");
            return null;
        }
        #endif

        return node;
    }

    
    public void Flip(bool on)
    {
        On = on;
        UpdateCascade();
    }

    void UpdateCascade()
    {   
        // Tell Owner about update
        ((IFlow)Owner).FlowUpdate(OutputPower);

        #if DEBUG
        //DebugPrint();
        #endif

        // Are we at the end?
        if(!HasOutputConnections){
            return;
        }

        // Travel Down Node Tree Updating all Owners
        for(int i = 0; i < OutputConnections.Count; i++){
            OutputConnections[i].UpdateCascade();
        }
    }

    public void DebugPrint()
    {
        GD.Print("____________________________________");
        GD.Print($"{Owner.Name} Flow Connection Data:");

        GD.Print($"Has Input Connection: {HasInputConnection.ToString()} - " +     (HasInputConnection? InputConnectionName : ""));
        GD.Print($"Has Output Connections: {HasOutputConnections.ToString()} - " + (HasOutputConnections? OutputConnections.Count.ToString() : ""));

        GD.Print($"Input Power: {InputPower}");
        GD.Print($"On: {On}");
        GD.Print($"Output Power: {OutputPower}");

        GD.Print("____________________________________");
    }
}
