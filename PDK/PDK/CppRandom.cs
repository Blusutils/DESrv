using System.Runtime.InteropServices;

namespace DESPDK.Random {
    public class CppRandom : IRandom {
        [DllImport("CppRand.dll")]
        public static extern int GetRandIntDll();
        [DllImport("CppRand.dll")]
        public static extern int GetRandIntDll(int startRange, int endRange);
        [DllImport("CppRand.dll")]
        public static extern void SeedDll(int value);

        public int GetRandInt() {
            return GetRandIntDll();
        }

        public int GetRandInt(int startRange, int endRange) {
            return GetRandIntDll(startRange, endRange);
        }

        public void Seed(int value) {
            SeedDll(value);
        }
    }
}
