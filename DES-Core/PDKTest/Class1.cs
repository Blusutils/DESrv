using DESCore.DESPDK;

namespace PDKTest {
    public class Class1 : PDKAbstractExtension {
        public string ID = "Test";
        public int ExtType = 1;
        public string Name = "Test PLugin";
        public string Version = "0.0.0";
        public string DESVersion = "0.0.1";
        public string Author = "EgorBron";

        public override void Entrypoint() {
            Console.WriteLine("Hello World!");
        }
        public override void LoadSubExtension(PDKAbstractExtension extension) {

        }
        public override void OnLoad() {

        }
        public override void OnUnload() {

        }
    }
}