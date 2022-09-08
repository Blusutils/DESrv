using System.Runtime.InteropServices;

namespace DESPDK.Random {
    public class CppRandom : RandomBase {
        [DllImport("CppRand.dll", EntryPoint = "?GetRandIntDll@@YAHXZ")]
        public static extern int GetRandIntDll();
        [DllImport("CppRand.dll", EntryPoint = "?GetRandIntDllRanged@@YAHHH@Z")]
        public static extern int GetRandIntDllRanged(int startRange, int endRange);
        [DllImport("CppRand.dll", EntryPoint = "?SeedDll@@YAXH@Z")]
        public static extern void SeedDll(int value);

        public override int GetRandInt() {
            return GetRandIntDll();
        }

        public override int GetRandInt(int startRange, int endRange) {
            return GetRandIntDllRanged(startRange, endRange);
        }

        public override void Seed(int value) {
            SeedDll(value);
        }
    }
}
