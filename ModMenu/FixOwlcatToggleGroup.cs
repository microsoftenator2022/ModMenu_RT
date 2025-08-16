using HarmonyLib;
using Owlcat.Runtime.UI.Controls.Toggles;
using System;
using System.Collections.Generic;
using System.Reflection;


namespace ModMenu
{
  [HarmonyPatch(typeof(OwlcatToggleGroup), nameof(OwlcatToggleGroup.HandleToggleOn))]
  internal static class FixOwlcatToggleGroup
  {
    static Dictionary<OwlcatToggle, IDisposable> MakeCopy(Dictionary<OwlcatToggle, IDisposable> original) =>
      new(original);

    static readonly MethodInfo get_Keys =
      typeof(Dictionary<OwlcatToggle, IDisposable>)
        .GetProperty(nameof(Dictionary<,>.Keys))
        .GetMethod;

    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
      foreach (var instruction in instructions)
      {
        if (instruction.Calls(get_Keys))
          yield return CodeInstruction.Call((Dictionary<OwlcatToggle, IDisposable> original) => MakeCopy(original));

        yield return instruction;
      }
    }
  }
}
