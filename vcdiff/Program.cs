using System;
using System.IO;
using VCDiff.Encoders;
using VCDiff.Decoders;
using VCDiff.Includes;

namespace vcdiff_cli
{
    internal class Program
    {
        static int Main(string[] args)
        {
            if (args.Length < 4)
            {
                PrintUsage();
                return 1;
            }

            string mode     = args[0].ToLowerInvariant();
            string argOne   = args[1];
            string argTwo   = args[2];
            string argThree = args[3];

            bool quiet = Array.Exists(args, a => a.Equals("-quiet", StringComparison.OrdinalIgnoreCase));

            try
            {
                switch (mode)
                {
                    case "-create":
                        Create(oldFile: argOne, newFile: argTwo, patchFile: argThree);
                        if (!quiet) Console.WriteLine($"Patch created: {argThree}");
                        break;

                    case "-apply":
                        Apply(oldFile: argOne, patchFile: argTwo, newFile: argThree);
                        if (!quiet) Console.WriteLine($"Patch applied: {argThree}");
                        break;

                    default:
                        Console.Error.WriteLine($"Unknown mode: {args[0]}");
                        PrintUsage();
                        return 1;
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.Error.WriteLine($"File not found: {ex.FileName}");
                return 1;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return 1;
            }

            return 0;
        }

        private static void Create(string oldFile, string newFile, string patchFile)
        {
            using var source = File.OpenRead(oldFile);
            using var target = File.OpenRead(newFile);
            using var patch  = File.Create(patchFile);
            var encoder = new VcEncoder(source, target, patch);
            VCDiffResult result = encoder.Encode(interleaved: true);
            if (result != VCDiffResult.SUCCESS)
                throw new Exception($"VCDiff encode failed: {result}");
        }

        private static void Apply(string oldFile, string patchFile, string newFile)
        {
            using var source = File.OpenRead(oldFile);
            using var patch  = File.OpenRead(patchFile);
            using var output = File.Create(newFile);
            var decoder = new VcDecoder(source, patch, output);
            VCDiffResult result = decoder.Decode(out long _);
            if (result != VCDiffResult.SUCCESS)
                throw new Exception($"VCDiff decode failed: {result}");
        }

        private static void PrintUsage()
        {
            Console.Error.WriteLine("Arguments:");
            Console.Error.WriteLine("  -create : Creates a vcdiff patch from two files.");
            Console.Error.WriteLine("  -apply  : Applies a vcdiff patch to a file.");
            Console.Error.WriteLine("  -quiet  : Silences output when patching a file.");

            Console.Error.WriteLine("Usage:");
            Console.Error.WriteLine("  vcdiff.exe -create <oldfile> <newfile> <patchfile>");
            Console.Error.WriteLine("  vcdiff.exe -apply  <oldfile> <patchfile> <newfile>");
            Console.Error.WriteLine("  vcdiff.exe -create <oldfile> <newfile> <patchfile> -quiet");
            Console.Error.WriteLine("  vcdiff.exe -apply  <oldfile> <patchfile> <newfile> -quiet");
        }
    }
}