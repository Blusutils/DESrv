namespace DESPDK.Random {
    public class RandomBase {
        public virtual void Seed(int value) { throw new NotImplementedException(); }
        public virtual int GetRandInt() { throw new NotImplementedException(); }
        public virtual int GetRandInt(int startRange, int endRange) { throw new NotImplementedException(); }
    }
}