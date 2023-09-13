using System.Text;

namespace StringJoins
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> input = new List<string>() 
            {
                "Name=vasya;gotcha=123;gotcha0=123",                
            };
            Console.WriteLine(MergeFiles(input));
            Console.ReadLine();
        }

        public static string MergeFiles(List<string> fileContents)
        {
            var dataByNameDict = new Dictionary<string, List<string>>();
            foreach (var file in fileContents)
            {
                var personsData = file.Split("\r\n");
                foreach (var person in personsData)
                {
                    var data = person.Split(';').ToList();
                    var nameIndex = -1;
                    for (int i = 0; i < data.Count(); i++)
                    {
                        if (data[i].Contains("Name"))
                        {
                            nameIndex = i;
                            break;
                        }
                    }

                    var name = data[nameIndex];
                    data.RemoveAt(nameIndex);
                    if (dataByNameDict.ContainsKey(name))
                    {
                        dataByNameDict[name].AddRange(data);
                    }
                    else
                    {
                        dataByNameDict.Add(name, data);
                    }
                }
            }

            var result = new StringBuilder();
            dataByNameDict = dataByNameDict.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            foreach (var key in dataByNameDict.Keys)
            {
                dataByNameDict[key] = dataByNameDict[key]
                    .OrderBy(x => x, new CustomStringComparer())
                    .ToList();
                result.Append($"{key}");
                if (dataByNameDict[key].Count > 0)
                {
                    result.Append(';');
                }

                result.Append(string.Join(';', dataByNameDict[key]));
                result.Append(Environment.NewLine);
            }

            result.Remove(result.Length - 1, 1);
            return result.ToString();

        }

        public class CustomStringComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                x = x.Split('=')[0];
                y = y.Split('=')[0];
                var minLength = Math.Min(x.Length, y.Length);
                for (int i = 0; i < minLength; i++)
                {
                    if (x[i] > y[i])
                        return 1;
                    if (x[i] < y[i])
                        return -1;
                }

                return x.Length - y.Length > 0 ? 1 : -1;
            }
        }
    }
    
}