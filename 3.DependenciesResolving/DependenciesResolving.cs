namespace DependenciesMain
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    internal class DependenciesResolving
    {
        private static Dictionary<string, List<string>> allPackages;

        private static List<string> dependenciesToBeInstalled;

        private static void Main(string[] args)
        {
            allPackages = new Dictionary<string, List<string>>();
            StreamReader reader = new StreamReader("..\\..\\res\\all_packages.json");
            StringBuilder bld = new StringBuilder();
            string line = string.Empty;
            while ((line = reader.ReadLine()) != null)
            {
                bld.Append(line.Trim());
            }

            reader.Close();
            string pattern = "([\"\\w]+)([: [\\S ]*?)([a-zA-Z\", ]{0,})(])";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(bld.ToString());
            while (match.Success)
            {
                string packageName = match.Groups[1].Value.Replace("\"", string.Empty);
                if (!string.IsNullOrEmpty(match.Groups[3].Value))
                {
                    string[] dependencies = match.Groups[3].Value.Replace("\"", string.Empty).Split(',');
                    List<string> list = new List<string>();
                    foreach (string str in dependencies)
                    {
                        list.Add(str.Trim());
                    }

                    allPackages.Add(packageName, list);
                }
                else
                {
                    allPackages.Add(packageName, new List<string>());
                }

                match = match.NextMatch();
            }

            bld.Length = 0;
            reader = new StreamReader("..\\..\\res\\dependencies.json");
            line = string.Empty;
            while ((line = reader.ReadLine()) != null)
            {
                bld.Append(line.Trim());
            }

            reader.Close();
            pattern = "(\"dependencies\": )(.*)";
            regex = new Regex(pattern);
            match = regex.Match(bld.ToString());
            dependenciesToBeInstalled = new List<string>();
            while (match.Success)
            {
                dependenciesToBeInstalled =
                    match.Groups[2].Value.Replace("[", string.Empty)
                        .Replace("]", string.Empty)
                        .Replace("\"", string.Empty)
                        .Replace("}", string.Empty)
                        .Split(',')
                        .ToList();
                match = match.NextMatch();
            }

            foreach (string dependency in dependenciesToBeInstalled)
            {
                Install(dependency);
            }

            Console.WriteLine("All done.");
        }

        public static void Install(string dependency)
        {
            Console.WriteLine("Installing dependency {0}", dependency);
            if (isAlreadyInstalled(dependency))
            {
                Console.WriteLine("{0} is already installed.", dependency);
                return;
            }
            
            List<string> furtherDependencies;
            if (allPackages.ContainsKey(dependency))
            {
                
                furtherDependencies = allPackages[dependency];
                if (furtherDependencies.Any())
                {
                    Console.WriteLine(
                    "In order to install {0} we need {1}",
                    dependency,
                    string.Join(" and ", furtherDependencies));    
                } 
                
                foreach (string dep in furtherDependencies)
                {
                    Install(dep);
                }

                Directory.CreateDirectory(string.Format(
                        "{0}\\..\\..\\res\\installed_modules\\{1}",
                        Directory.GetCurrentDirectory(),
                        dependency));
            }
        }

        private static bool isAlreadyInstalled(string dependency)
        {
            if (Directory.Exists(string.Format("..\\..\\res\\installed_modules\\{0}", dependency)))
            {
                return true;
            }

            return false;
        }
    }
}