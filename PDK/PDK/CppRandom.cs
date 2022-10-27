//using System.Runtime.InteropServices;

//namespace DESPDK.Random {
//    [ComVisible(true)]
//    public class CppRandom : RandomBase {
//        [DllImport("CppRand.dll")]
//        public static extern int GetRandIntDll();
//        [DllImport("CppRand.dll")]
//        public static extern int GetRandIntDllRanged(int startRange, int endRange);
//        [DllImport("CppRand.dll")]
//        public static extern void SeedDll(int value);

//        public override int GetRandInt() {
//            return GetRandIntDll();
//        }

//        public override int GetRandInt(int startRange, int endRange) {
//            return GetRandIntDllRanged(startRange, endRange);
//        }

//        public override void Seed(int value) {
//            SeedDll(value);
//        }
//    }
//}
