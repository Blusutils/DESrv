namespace DESrv.PDK.Random {
    public class RandomBase {
        public readonly string ExtID = "DESrvInternal";
        public bool IsPreffered { get; set; } = false;
        public virtual void Seed(int value) { throw new NotImplementedException(); }
        public virtual int GetRandInt() { throw new NotImplementedException(); }
        public virtual int GetRandInt(int startRange, int endRange) { throw new NotImplementedException(); }

        public static List<RandomBase> Randoms { get; set; } = new();
        public static RandomBase? GetPrefferedRandom(int seed = 1) {
            foreach (var rnd in Randoms)
                if (rnd.IsPreffered) {
                    rnd.Seed(seed);
                    return rnd;
                } else return null;
            return null; }
    }
}