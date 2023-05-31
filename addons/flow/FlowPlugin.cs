#if TOOLS
using Godot;
using System;

[Tool]
public partial class FlowPlugin : EditorPlugin
{
	static string NodeName = "FlowNode";
	Script _Script = ResourceLoader.Load<Script>($"res://addons/flow/{NodeName}.cs");
	Texture2D Icon = ResourceLoader.Load<Texture2D>("res://addons/flow/icon.png");

	public override void _EnterTree()
	{
		AddCustomType(NodeName, "Resource", _Script, Icon);
	}

	public override void _ExitTree()
	{
		RemoveCustomType(NodeName);
	}
}
#endif
