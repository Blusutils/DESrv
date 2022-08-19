namespace DESPDK.Random {
    public interface IRandom {
        public void Seed(int value);
        public int GetRandInt();
        public int GetRandInt(int startRange, int endRange);
    }
}