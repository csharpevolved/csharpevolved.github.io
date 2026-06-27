using System;
using System.Collections.Generic;

public class LocalFunctionsAsyncAndIterators
{
    // Local async function
    public async System.Threading.Tasks.Task<string> FetchDataAsync(string url)
    {
        var client = new System.Net.Http.HttpClient();
        var data = await FetchInternal();
        return data;

        async System.Threading.Tasks.Task<string> FetchInternal()
        {
            var response = await client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }
    }

    // Local iterator function (generator)
    public IEnumerable<int> GenerateSequence(int count)
    {
        return GetNumbers();

        IEnumerable<int> GetNumbers()
        {
            for (int i = 0; i < count; i++)
            {
                yield return i * 2;
            }
        }
    }

    // Local async iterator (C# 8.0+)
    public async System.Collections.Generic.IAsyncEnumerable<int> FetchNumbersAsync(int count)
    {
        await foreach (var num in GetNumbersAsync())
        {
            yield return num;
        }

        async IAsyncEnumerable<int> GetNumbersAsync()
        {
            for (int i = 0; i < count; i++)
            {
                await System.Threading.Tasks.Task.Delay(100); // Simulate async work
                yield return i;
            }
        }
    }

    // Local function with lazy evaluation
    public IEnumerable<T> FilterAndTransform<T>(IEnumerable<T> source, Func<T, bool> predicate, Func<T, T> transform)
    {
        return ExecuteTransform();

        IEnumerable<T> ExecuteTransform()
        {
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    yield return transform(item);
                }
            }
        }
    }

    // Combining multiple local functions with async
    public async System.Threading.Tasks.Task<(int success, int failed)> ProcessItemsAsync(string[] items)
    {
        int successCount = 0;
        int failCount = 0;

        foreach (var item in items)
        {
            if (await TryProcess(item))
                successCount++;
            else
                failCount++;
        }

        return (successCount, failCount);

        async System.Threading.Tasks.Task<bool> TryProcess(string item)
        {
            try
            {
                await ProcessItemAsync(item);
                return true;
            }
            catch
            {
                return false;
            }
        }

        async System.Threading.Tasks.Task ProcessItemAsync(string item)
        {
            await System.Threading.Tasks.Task.Delay(50); // Simulate work
            Console.WriteLine($"Processed: {item}");
        }
    }

    // Local async function with resource management
    public async System.Threading.Tasks.Task<int> CountLinesAsync(string filePath)
    {
        return await CountLines();

        async System.Threading.Tasks.Task<int> CountLines()
        {
            int count = 0;
            using (var reader = new System.IO.StreamReader(filePath))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                        count++;
                }
            }
            return count;
        }
    }
}
