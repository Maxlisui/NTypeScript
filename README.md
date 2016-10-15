
# NTypeScript / TypeScript.NET

A C#/.NET wrapper for TypeScript

License: MIT

## Features

- Compile TypeScript from C#
- Execute compiled JavaScript with built-in JurassicJS Engine
- Asynchronous API
- Can run latest TypeScript version

## Usage

```csharp
        private static async Task MainAsync()
        {
            //Create a new Compiler instance (it has yet to be initialized)
            var compiler = new TypeScriptCompiler();
            Console.WriteLine("Compiling compiler...");

            //InitializeCompilerAsync() loads the compiler from a resource
            //and compiles it. This is relatively resource expensive; it can
            //take a few seconds to compile the TypeScript compiler and
            //quite a few MB of RAM.
            //Right now, NTypeScript uses JurassicJS as its engine, because
            //Jurassic compiles to IL code.
            //Once the compiler is compiled, the compiler can be used to compile
            //TypeScript.
            await compiler.InitializeCompilerAsync();
            string helloWorldScript = @"
class RandomProgram {
    sayHello() {
        console.log(""Hello, World!"");
    }
}
let myProgram = new RandomProgram();
myProgram.sayHello();
";
            Console.WriteLine("Compiling Hello World script...");
            
            //This will compile the script with the compiler that was
            //compiled earlier. This isn't too resource intensive, but
            //takes about the same amount of time as the normal TypeScript
            //compiler running in Node.JS
            var transpiledHelloWorldScript = await compiler.CompileAsync(helloWorldScript);
            Console.WriteLine(transpiledHelloWorldScript);

            //Run the script
            var jsExecutor = new JavaScriptExecutor();
            jsExecutor.EnableConsoleApi();
            await jsExecutor.ExecuteAsync(transpiledHelloWorldScript);
        }
```
