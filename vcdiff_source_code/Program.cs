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
            if (args.Length < 2)
            {
                PrintUsage();
                return 1;
            }
            string mode = args[0].ToLowerInvariant();
            bool quiet = Array.Exists(args, a => a.Equals("-quiet", StringComparison.OrdinalIgnoreCase));

            try
            {
                switch (mode)
                {
                    case "-manifest":
                        RunManifest(manifestFile: args[1], quiet);
                        break;

                    case "-create":
                        if (args.Length < 4) { PrintUsage(); return 1; }
                        Create(oldFile: args[1], newFile: args[2], patchFile: args[3]);
                        if (!quiet) Console.WriteLine($"Patch created: {args[3]}");
                        break;

                    case "-apply":
                        if (args.Length < 4) { PrintUsage(); return 1; }
                        Apply(oldFile: args[1], patchFile: args[2], newFile: args[3]);
                        if (!quiet) Console.WriteLine($"Patch applied: {args[3]}");
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

        private static void RunManifest(string manifestFile, bool quiet)
        {
            if (!File.Exists(manifestFile))
                throw new FileNotFoundException("Manifest file not found.", manifestFile);

            string[] lines = File.ReadAllLines(manifestFile);
            int success = 0, skipped = 0;

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split('|');
                if (parts.Length != 3)
                {
                    if (!quiet) Console.Error.WriteLine($"Skipped (bad line): {line}");
                    skipped++;
                    continue;
                }

                string oldFile   = parts[0].Trim();
                string newFile   = parts[1].Trim();
                string patchFile = parts[2].Trim();

                if (!File.Exists(oldFile) || !File.Exists(newFile))
                {
                    if (!quiet) Console.Error.WriteLine($"Skipped (missing file): {Path.GetFileName(newFile)}");
                    skipped++;
                    continue;
                }

                Directory.CreateDirectory(Path.GetDirectoryName(patchFile)!);
                Create(oldFile, newFile, patchFile);
                if (!quiet) Console.WriteLine($"Patch created: {Path.GetFileName(newFile)}");
                success++;
            }

            if (!quiet) Console.WriteLine($"Done. {success} patch(es) created, {skipped} skipped.");
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
            Console.Error.WriteLine("  -create   : Creates a vcdiff patch from two files.");
            Console.Error.WriteLine("  -apply    : Applies a vcdiff patch to a file.");
            Console.Error.WriteLine("  -manifest : Uses a manifest (.txt) file to batch process multiple files.");
            Console.Error.WriteLine("  -quiet    : Fully silences all console output.");
            Console.Error.WriteLine("");
            Console.Error.WriteLine("Usage:");
            Console.Error.WriteLine("  vcdiff.exe -create <oldfile> <newfile> <patchfile>");
            Console.Error.WriteLine("  vcdiff.exe -apply  <oldfile> <patchfile> <newfile>");
            Console.Error.WriteLine("  vcdiff.exe -manifest <manifestfile>");
            Console.Error.WriteLine("  vcdiff.exe -create <oldfile> <newfile> <patchfile> -quiet");
            Console.Error.WriteLine("  vcdiff.exe -apply  <oldfile> <patchfile> <newfile> -quiet");
            Console.Error.WriteLine("  vcdiff.exe -manifest <manifestfile> -quiet");
            Console.Error.WriteLine("");
            Console.Error.WriteLine("Manifest Notes:");
            Console.Error.WriteLine("  - A \"manifest\" file is just a standard text document with entries.");
            Console.Error.WriteLine("  - Only the \"create\" patches operation works with a manifest file.");
            Console.Error.WriteLine("  - Each line in a manifest is the arugments separated by pipe \"|\" character.");
            Console.Error.WriteLine("  - For example: C:\\Old.file|C:\\New.File|C:\\Patch.vcdiff");

        }
    }
}