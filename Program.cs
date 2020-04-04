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
        public delegate void TestEvent();
        public event TestEvent OnTestEvent;

        internal void Run()
        {
            OnTestEvent += Handler;
            var _ = OnTestEvent.Method;  // <=== "The runtime has encountered a fatal error. [...] 0xc0000005"
        }

        // patched with an empty prefix
        private void Handler() { }
    }
}
