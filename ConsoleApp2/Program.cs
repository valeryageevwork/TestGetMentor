using System;
using System.Text.Json;

int counter = 0;

while (true)
{
    Console.WriteLine("Write next - next planet, write back - back planet\n" +
                      "Write a - exit\n");
    Console.Write("Write: ");

    var str = Console.ReadLine();
    Console.WriteLine();

    if (str.ToLower() == "next")
    {
        counter++;
        await TaskStarWars(counter);
    }
    else if (str.ToLower() == "back")
    {
        if (counter == 1)
        {
            Console.WriteLine("There is not planet\n");
        }
        else
        {
            counter--;
            await TaskStarWars(counter);
            Console.WriteLine();
        }
        
    }
    else if (str.ToLower() == "a")
    {
        break;
    }
    else
    {
        Console.WriteLine("Write the correct string!\n");
    }
}

async static Task TaskStarWars(int counter)
{
    using (var client = new HttpClient())
    {
        var result = await client.GetAsync($"https://swapi.dev/api/planets/{counter}");
        string jsonResult = await result.Content.ReadAsStringAsync();

        JsonDocument doc = JsonDocument.Parse(jsonResult);
        JsonElement root = doc.RootElement;
        var el = root;

        Console.WriteLine(el.GetProperty("name").ToString() + '\n');
        Console.WriteLine(el.GetProperty("gravity").ToString() + '\n');

        var residents = el.GetProperty("residents");
        string[]? array = JsonSerializer.Deserialize<string[]>(residents);
        foreach (var ident in array)
        {
            var resultPeople = await client.GetAsync(ident);
            string jsonResultPeople = await resultPeople.Content.ReadAsStringAsync();
            JsonDocument docPeople = JsonDocument.Parse(jsonResultPeople);
            JsonElement rootPeople = docPeople.RootElement;
            var elPeople = rootPeople;

            Console.WriteLine(elPeople.GetProperty("name").ToString());
        }
        Console.WriteLine();
    }
}


