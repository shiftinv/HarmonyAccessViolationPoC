using HarmonyLib;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace HarmonyAccessViolationPoC
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var harmony = new Harmony("_");
            harmony.PatchAll();

            var form = new Form1();

            var propEvents = typeof(Control).GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance);
            var eventClick = typeof(Control).GetField("EventClick", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

            var button = AccessTools.Field(typeof(Form1), "button1").GetValue(form);
            var events = (EventHandlerList)propEvents.GetValue(button);
            var handler = events[eventClick];

            var handlerMethod = handler.Method;  // <=== "The runtime has encountered a fatal error. [...] 0xc0000005"

            // handlerMethod  should be equal to  Form1.Button1_Click
            Console.WriteLine($"handlerMethod: {handlerMethod}");
        }
    }


    [HarmonyPatch(typeof(Form1), "Button1_Click")]
    class HandlerPatch
    {
        static void Prefix()
        {
        }
    }
}
