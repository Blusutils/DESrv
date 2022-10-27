using System.Runtime.InteropServices;

namespace DESrv.PDK.Random {
    [ComVisible(true)]
    public class DotnetRandom : RandomBase {
        static System.Random rand = new System.Random();
        public override int GetRandInt() {
            return rand.Next();
        }
        public override int GetRandInt(int startRange, int endRange) {
            return rand.Next(startRange, endRange);
        }
        public override void Seed(int value) {
            rand = new System.Random(value);
        }
    }
}
