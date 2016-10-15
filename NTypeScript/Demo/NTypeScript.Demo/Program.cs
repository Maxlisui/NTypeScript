using IridiumIon.NTypeScript;
using System;
using System.Threading.Tasks;

namespace NTypeScript.Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            var compiler = new TypeScriptCompiler();
            Console.WriteLine("Compiling compiler...");
            await compiler.InitializeCompilerAsync();
            string helloWorldScript = @"
class RandomProgram {
    public void sayHello() {
        console.log(""Hello, World!"");
    }
}
let myProgram = new RandomProgram();
myProgram.sayHello();
";
            Console.WriteLine("Compiling Hello World script...");
            var compiledScript = compiler.Compile(helloWorldScript);
            Console.WriteLine(compiledScript);
        }
    }
}