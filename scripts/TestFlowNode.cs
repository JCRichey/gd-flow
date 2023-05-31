using Godot;
using System;

public partial class TestFlowNode : Control, IFlow
{
    [Export]
    FlowStateDisplay display;

    [Export]
	FlowNode FlowNode_;
	public FlowNode GetFlowNode() => FlowNode_; 
    public void FlowUpdate(bool state)
    {
        display.Update(FlowNode_); // Joewarida?
    }

    public override void _Ready()
    {
        FlowNode_.Init(this);

        base._Ready();
    }

    public void _on_button_pressed()
    {
        FlowNode_.Flip(!FlowNode_.On);
    }
}
