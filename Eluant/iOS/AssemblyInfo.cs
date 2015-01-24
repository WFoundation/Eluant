using System;
using ObjCRuntime;
using System.Reflection;

[assembly: CLSCompliantAttribute (true)]
[assembly: LinkWith("liblua5.1.a", LinkTarget.Simulator | LinkTarget.ArmV6 | LinkTarget.ArmV7 | LinkTarget.ArmV7s, Frameworks = "Foundation", ForceLoad = true, IsCxx = true, LinkerFlags = "-lstdc++")]
