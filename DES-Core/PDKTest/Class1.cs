using DESPDK;

namespace PDKTest {
    public class Extension : PDKAbstractExtension {
        public new static string ID = "Test";
        public new int ExtType = 1;
        public new string Name = "Test PLugin";
        public new string Version = "0.0.0";
        public new string DESVersion = "0.0.1";
        public new string Author = "EgorBron";

        public Extension() {
            ID = "Test1";
            ExtType = 1;
            Name = "Test PLugin";
            Version = "0.0.0";
            DESVersion = "0.0.1";
            Author = "EgorBron";
            Console.WriteLine(ID);
    }

        public override void Entrypoint() {
            Console.WriteLine("Hello World!", ID);
        }
        public override void LoadSubExtension(PDKAbstractExtension extension) {

        }
        public override void OnLoad() {

        }
        public override void OnUnload() {

        }
    }
}