using HarmonyLib;
using System;

namespace HarmonyAccessViolationPoC
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var harmony = new Harmony("_");
            harmony.PatchAll();

            new TestClass().Run();
        }
    }

    [HarmonyPatch(typeof(TestClass), "Handler")]
    class HandlerPatch
    {
        static void Prefix() { }
    }

    class TestClass : MarshalByRefObject    // crashes
    //class TestClass    // works
    {
        delegate void TestDelegate();

        internal void Run()
        {
            Delegate del = Delegate.CreateDelegate(typeof(TestDelegate), this, "Handler");
            var _ = del.Method;  // <=== "The runtime has encountered a fatal error. [...] 0xc0000005"
        }

        // patched with an empty prefix
        private void Handler() { }
    }
}
