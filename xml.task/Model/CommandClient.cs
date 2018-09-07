using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using xml.task.Model.Commands;
using xml.task.Model.Commands.SimpleCommands;

namespace xml.task
{
    internal static class CommandClient
    {
        public static List<Command> GetCommands(XDocument xmlDocument)
        {
            var commands = new List<Command>();
            var id = 0;
            if (xmlDocument.Root == null) return commands;
            foreach (var xmlElement in xmlDocument.Root.Elements())
            {
                var filesInformation = GetFilesInformation(xmlElement);
                switch (xmlElement.Name.LocalName)
                {
                    case @"stab_d":
                        foreach (var filesList in filesInformation)
                        {
                            var command = new DynamicCommand(xmlElement)
                            {
                                ID = id,
                                Files = filesList                             
                            };
                            commands.Add(command);
                            id++;
                        }
                        break;
                    case @"correction":
                        commands.Add(new WriteCommand(xmlElement)); break;
                    default:
                        break;
                }
            }
            foreach (var comm in commands)
            {
                Console.WriteLine(comm.Name);
                Console.WriteLine(comm.ID);
                foreach (var list in comm.Files)
                {
                    Console.WriteLine(list);
                }
            }
            return commands;
        }

        public static List<List<string>> GetFilesInformation(XElement xmlElement)
        {
            var finalList = new List<List<string>>();

            foreach (var subElement in xmlElement.Elements())
            {
                
                var type = subElement.Name.LocalName;
                if (type == @"file")
                {
                    var combination = new List<string>();
                    var path = subElement?.Attribute(@"path")?.Value;
                    if (path != null)
                    {
                        combination.Add(path);
                    }
                    if (combination.Count > 0)
                        finalList.Add(combination);
                }
                else if (type == @"files")
                {
                    var combination = new List<string>();
                    foreach (var fileElement in subElement.Elements())
                    {
                        var path = fileElement?.Attribute(@"path")?.Value;
                        if (path != null)
                        {
                            combination.Add(path);
                        }
                    }
                    if (combination.Count > 0)
                        finalList.Add(combination);
                }
                else if (type == @"folder")
                {
                    var folderPath = subElement?.Attribute(@"path")?.Value;
                    var varString = subElement?.Attribute(@"var")?.Value;
                    if (varString == null)
                        continue;
                    var varExtensionsList = varString.Split(',');
                    if (varExtensionsList.Length == 0)
                        continue;
                    var filesList = new List<List<string>>();
                    foreach (var extension in varExtensionsList)
                    {
                        filesList.Add(Directory.GetFiles(folderPath, $@"*.{extension}").ToList<string>());
                    }
                    var combinationsList = GetCombinations(filesList);

                    var staticCombination = new List<string>();
                    foreach (var fileElement in subElement.Elements())
                    {
                        var path = fileElement?.Attribute(@"path")?.Value;
                        if (path != null)
                        {
                            staticCombination.Add(path);
                        }
                    }

                    for (int i = 0; i < combinationsList.Count; i++)
                    {
                        combinationsList[i].AddRange(staticCombination);
                    }
                    finalList.AddRange(combinationsList);
                }
            }

            return finalList;
        }

        public static string GetHeader(XDocument xmlDocument)
        {
            return xmlDocument.Root?.Attribute(@"name")?.Value ?? @"noname";
        }

        private static List<List<string>> GetCombinations(List<List<string>> sourceList)
        {
            return GetCombinationsWithRecursion(sourceList, new List<List<string>>(), new int[sourceList.Count]);
        }

        private static List<List<string>> GetCombinationsWithRecursion(List<List<string>> sourceList, List<List<string>> resultList, int[] counter)
        {
            var temporaryList = resultList;
            var combinationList = new List<string>();
            for (int i = 0; i < sourceList.Count; i++)
            {
                combinationList.Add(sourceList[i][counter[i]]);
            }
            temporaryList.Add(combinationList);
            counter[counter.Length - 1]++;
            for (int i = counter.Length - 1; i >= 0; i--)
            {
                if (counter[i] == sourceList[i].Count)
                {
                    if (i == 0)
                    {
                        return temporaryList;
                    }
                    else
                    {
                        counter[i - 1]++;
                        counter[i] = 0;
                    }
                }
            }
            return GetCombinationsWithRecursion(sourceList, temporaryList, counter);
        }
    }
}
