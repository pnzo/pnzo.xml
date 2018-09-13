using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using xml.task.Model.Commands;
using xml.task.Model.Commands.SimpleCommands;

namespace xml.task.Model
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

                foreach (var List in filesInformation)
                {
                    Command command;
                    switch (xmlElement.Name.LocalName)
                    {
                        case @"stability":
                            command = new DynamicCommand(xmlElement);
                            break;
                        case @"graph":
                            command = new PlotCommand(xmlElement);
                            break;
                        default:
                            command = new ErrorCommand(xmlElement);
                            break;
                    }
                    command.Id = id;
                    command.Files = List;
                    commands.Add(command);
                    id++;

                }
            }
            foreach (var comm in commands)
            {
                Console.WriteLine(comm.Name);
                Console.WriteLine(comm.Id);
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
                switch (type)
                {
                    case @"file":
                        {
                            var combination = new List<string>();
                            var path = subElement.Attribute(@"path")?.Value;
                            if (path != null)
                            {
                                combination.Add(path);
                            }

                            if (combination.Count > 0)
                                finalList.Add(combination);
                            break;
                        }
                    case @"files":
                        {
                            var combination = subElement.Elements()
                                .Select(fileElement => fileElement?.Attribute(@"path")?.Value)
                                .Where(path => path != null).ToList();
                            if (combination.Count > 0)
                                finalList.Add(combination);
                            break;
                        }
                    case @"folder":
                        var folderPath = subElement.Attribute(@"path")?.Value;
                        var varString = subElement.Attribute(@"var")?.Value;
                        if (varString == null || folderPath == null)
                            continue;
                        var varExtensionsList = varString.Split(',');
                        if (varExtensionsList.Length == 0)
                            continue;
                        var List = varExtensionsList
                            .Select(extension => Directory.GetFiles(folderPath, $@"*.{extension}").ToList())
                            .Where(list => list.Count > 0).ToList();
                        var combinationsList = GetCombinations(List);

                        var staticCombination = subElement.Elements()
                            .Select(fileElement => fileElement?.Attribute(@"path")?.Value)
                            .Where(path => path != null).ToList();

                        foreach (var combination in combinationsList)
                        {
                            combination.AddRange(staticCombination);
                        }

                        finalList.AddRange(combinationsList);
                        break;
                }
            }

            return finalList;
        }

        public static string GetHeader(XDocument xmlDocument)
        {
            return xmlDocument.Root?.Attribute(@"name")?.Value ?? @"noname";
        }

        private static List<List<string>> GetCombinations(IReadOnlyList<List<string>> sourceList)
        {
            return GetCombinationsWithRecursion(sourceList, new List<List<string>>(), new int[sourceList.Count]);
        }

        private static List<List<string>> GetCombinationsWithRecursion(IReadOnlyList<List<string>> sourceList, List<List<string>> resultList, IList<int> counter)
        {
            if (sourceList.Count == 0)
                return resultList;
            var temporaryList = resultList;
            var combinationList = sourceList.Select((t, i) => t[counter[i]]).ToList();
            temporaryList.Add(combinationList);
            counter[counter.Count - 1]++;
            for (var i = counter.Count - 1; i >= 0; i--)
            {
                if (counter[i] != sourceList[i].Count) continue;
                if (i == 0)
                    return temporaryList;
                counter[i - 1]++;
                counter[i] = 0;
            }
            return GetCombinationsWithRecursion(sourceList, temporaryList, counter);
        }
    }
}
