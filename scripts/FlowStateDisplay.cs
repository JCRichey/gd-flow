using Godot;
using System;

public partial class FlowStateDisplay : Control
{
    [Export]
    Label Input, On, Output;

    [Export]
    Color ColorOn = new Color(0.0f, 1.0f, 0.0f), ColorOff = new Color(1.0f, 0.0f, 0.0f);
    
    public void Update(FlowNode node)
    {
        //Input.Color  = node.InputPower?  ColorOn:ColorOff;
        //On.Color     = node.On?          ColorOn:ColorOff;
        //Output.Color = node.OutputPower? ColorOn:ColorOff;

        Input.Text  = node.InputPower?  "1":"0";
        On.Text     = node.On?          "1":"0";
        Output.Text = node.OutputPower? "1":"0";
    } 
}
