#pragma warning disable IDE0073 // The file header is missing or not located at the top of the file
using Jurassic;
using System;
#pragma warning restore IDE0073 // The file header is missing or not located at the top of the file
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0008:Use explicit type", Justification = "Not my Stuff")]
        public void InitializeCompiler()
        {
            //Load the compiler
            FileInfo fi = new FileInfo(Path.Combine(Assembly.GetExecutingAssembly().Location));

            var compilerSourceStream = File.OpenRead(Path.Combine(fi.Directory.FullName, "Resources", "typescriptServices.js"));
            var compilerSource = ReadStream(compilerSourceStream);
            //Compile the compiler
            //TypeScriptCompilerScript = JSEngine.Compile(new StringScriptSource(compilerSource));
            JSEngine.Evaluate(new StringScriptSource(compilerSource));

            //Load bootstrap
            var bootstrapSourceStream = File.OpenRead(Path.Combine(fi.Directory.FullName, "Resources", "nTypescriptBootstrap.js"));
            var bootstrapSource = ReadStream(bootstrapSourceStream);
            JSEngine.Evaluate(bootstrapSource);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0008:Use explicit type", Justification = "Not my Stuff")]
        public string ReadStream(Stream stream)
        {
            var value = string.Empty;
            using (stream)
            {
                using (var sourceReader = new StreamReader(stream))
                {
                    value = sourceReader.ReadToEnd();
                }
            }
            return value;
        }

        /// <summary>
        /// Compiles the TypeScript compiler asynchronously.
        /// </summary>
        /// <returns></returns>
        public async Task InitializeCompilerAsync()
        {
            await Task.Run(() => InitializeCompiler());
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0008:Use explicit type", Justification = "Not my Stuff")]
        public string Compile(string source)
        {
            var transpileResult = JSEngine.CallGlobalFunction("tsTranspile", source);
            var outputCode = JSEngine.CallGlobalFunction<string>("getTranspileResultCode", transpileResult);
            //var outputDiagnostics = JSEngine.CallGlobalFunction<string>("getTranspileResultDiagnostics", transpileResult);
            return outputCode;
        }

        public async Task<string> CompileAsync(string source)
        {
            return await Task.Run(() => Compile(source));
        }
    }
}
