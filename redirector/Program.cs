using System;
using System.IO;
using System.Linq;

namespace redirector
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2 || args.Length > 3)
            {
                Console.WriteLine(
                    "Invalid arguments. " +
                    "Usuage redirector: " +
                    "{mappingsFile} " +
                    "{templateFile with hash redirect} " +
                    "[{templateFile without hash redirect}]");
            }
            else
            {
                string mappingsFile = args[0];
                string templateFileWithHashRedirect = args[1];
                string templateFileWithoutHashRedirect = args.Last();

                string templateWithHashRedirect = File.ReadAllText(templateFileWithHashRedirect);
                string templateWithoutHashRedirect = File.ReadAllText(templateFileWithoutHashRedirect);

                var mappings =
                    from lineIndex in File.ReadAllLines(mappingsFile).Select((line, index) => (line, index))
                    let line = lineIndex.line
                    where !string.IsNullOrWhiteSpace(line)
                    where !line.TrimStart().StartsWith("#")
                    select Extract(line, lineIndex.index);

                var duplicates =
                    from mapping in mappings
                    let path = mapping.path.ToLowerInvariant()
                    group mapping by path into g
                    where g.Count() > 1
                    select g.Key;

                if (duplicates.Any())
                {
                    Console.WriteLine($"{mappingsFile} contains keys: {duplicates.First()}.");
                }

                foreach (var mapping in mappings)
                {
                    string template = mapping.target.Contains("#")
                        ? templateWithoutHashRedirect
                        : templateWithHashRedirect;

                    File.WriteAllText(
                        path: mapping.path + ".html",
                        contents: template.Replace("{url}", mapping.target));
                }
            }
        }

        private static (string path, string target) Extract(string line, int index)
        {
            var pair = line.Split('\t');

            if (pair.Length < 2) throw new FormatException(
                $"Target missing from line {index + 1}. No tab character found.");

            if (string.IsNullOrWhiteSpace(pair[0])) throw new FormatException(
                 $"Path missing from line {index + 1}. Only white space found before the tab character.");

            if (string.IsNullOrWhiteSpace(pair[1])) throw new FormatException(
                 $"Target missing from line {index + 1}. Only white space found after the tab character.");

            return (pair[0], pair[1]);
        }
    }
}