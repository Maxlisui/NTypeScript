using Jurassic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace IridiumIon.NTypeScript
{
    /// <summary>
    /// Provides a compiler service. To reduce overhead of loading the compiler, use one instance throughout your program.
    /// </summary>
    public class TypeScriptCompiler
    {
        public ScriptEngine JSEngine { get; set; }
        public CompiledScript TypeScriptCompilerScript { get; set; }

        /// <summary>
        /// Creates a new TypeScript compiler. Be sure to call InitializeCompiler() or InitializeCompilerAsync() before using the compiler.
        /// </summary>
        public TypeScriptCompiler()
        {
            JSEngine = new ScriptEngine();
        }

        /// <summary>
        /// Compiles the TypeScript compiler
        /// </summary>
        public void InitializeCompiler()
        {
            //Load the compiler
            var compilerSourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("IridiumIon.NTypeScript.Resources.typescriptServices.js");
            var compilerSource = string.Empty;
            using (compilerSourceStream)
            {
                using (var sourceReader = new StreamReader(compilerSourceStream))
                {
                    compilerSource = sourceReader.ReadToEnd();
                }
            }
            //Compile the compiler
            //TypeScriptCompilerScript = JSEngine.Compile(new StringScriptSource(compilerSource));
            JSEngine.Evaluate(new StringScriptSource(compilerSource));
            //Add a utility function to allow transpilation
            string transpilerBootstrap = "function tsTranspile(source) { return ts.transpile(source); }";
            JSEngine.Evaluate(transpilerBootstrap);
        }

        /// <summary>
        /// Compiles the TypeScript compiler asynchronously.
        /// </summary>
        /// <returns></returns>
        public async Task InitializeCompilerAsync()
        {
            await Task.Run(() => InitializeCompiler());
        }

        public string Compile(string source)
        {
            return JSEngine.CallGlobalFunction<string>("tsTranspile", source);
        }

        /// <summary>
        /// Injects a Console implementation into the script engine. This allows the use of console.log() and more...
        /// </summary>
        public void EnableConsoleApi()
        {
            JSEngine.SetGlobalValue("console", new Jurassic.Library.FirebugConsole(JSEngine));
        }
    }
}