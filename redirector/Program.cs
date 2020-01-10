using System;
using System.IO;
using System.Linq;

namespace redirector
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine(
                    "Invalid arguments. Usuage redirector {templateFile} {mappingsFile}");
            }
            else
            {
                string templateFile = args[0];
                string mappingsFile = args[1];

                string template = File.ReadAllText(templateFile);

                var mappings =
                    from line in File.ReadAllLines(mappingsFile)
                    where !string.IsNullOrWhiteSpace(line)
                    where !line.TrimStart().StartsWith("#")
                    let pair = line.Split('\t')
                    select new { path = pair[0], target = pair[1] };

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
                    File.WriteAllText(
                        path: mapping.path + ".html",
                        contents: template.Replace("{url}", mapping.target));
                }
            }
        }
    }
}