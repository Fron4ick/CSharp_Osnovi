using System;
using System.Collections.Generic;
using System.Linq;

namespace PocketGoogle;

public class Indexer : IIndexer
{
    private readonly Dictionary<string, Dictionary<int, List<int>>> data = 
        new Dictionary<string, Dictionary<int, List<int>>>();
    
    private static readonly char[] separators = { ' ', '.', ',', '!', '?', ':', '-', '\r', '\n' };

    public void Add(int id, string documentText)
    {
        int currentPos = 0;
        string[] words = documentText.Split(separators);

        foreach (var word in words)
        {
            if (!string.IsNullOrEmpty(word))
            {
                if (!data.ContainsKey(word))
                    data[word] = new Dictionary<int, List<int>>();

                if (!data[word].ContainsKey(id))
                    data[word][id] = new List<int>();

                data[word][id].Add(currentPos);
            }
            currentPos += word.Length + 1;
        }
    }

    public List<int> GetIds(string word)
    {
        if (data.ContainsKey(word))
        {
            return data[word].Keys.ToList();
        }
        return new List<int>();
    }

    public List<int> GetPositions(int id, string word)
    {
        if (data.ContainsKey(word) && data[word].ContainsKey(id))
        {
            return data[word][id];
        }
        return new List<int>();
    }

    public void Remove(int id)
    {
        foreach (var wordEntry in data.Values)
        {
            wordEntry.Remove(id);
        }
    }
}