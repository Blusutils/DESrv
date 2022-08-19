namespace DESPDK.Random {
    public class DotnetRandom : IRandom {
        static System.Random rand = new System.Random();
        public int GetRandInt() {
            return rand.Next();
        }
        public int GetRandInt(int startRange, int endRange) {
            return rand.Next(startRange, endRange);
        }
        public void Seed(int value) {
            rand = new System.Random(value);
        }
    }
}
