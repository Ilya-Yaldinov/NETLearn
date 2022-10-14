using Newtonsoft.Json;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

class Program
{
    private const string PathTanks = "tanks.json";
    private const string PathUnits = "units.json";
    private const string PathFactories = "factories.json";
    static void Main(string[] args)
    {
        var tanks = GetTanks();
        var units = GetUnits();
        var factories = GetFactories();
        Console.WriteLine($"Количество резервуаров: {tanks.Length}, установок: {units.Length}");

        var foundUnit = FindUnit(units, tanks, "Резервуар 2");
        var factory = FindFactory(factories, foundUnit);

        Console.WriteLine($"Резервуар 2 принадлежит установке {foundUnit.Name} и заводу {factory.Name}");

        var totalVolume = GetTotalVolume(tanks);
        Console.WriteLine($"Общий объем резервуаров: {totalVolume}");
    }

    // реализуйте этот метод, чтобы он возвращал массив резервуаров, согласно приложенным таблицам
    public static Tank[] GetTanks()
    {
        string tanks = File.ReadAllText(PathTanks);
        return JsonConvert.DeserializeObject<Tank[]>(tanks);
    }
    // реализуйте этот метод, чтобы он возвращал массив установок, согласно приложенным таблицам
    public static Unit[] GetUnits()
    {
        string units = File.ReadAllText(PathUnits);
        return JsonConvert.DeserializeObject<Unit[]>(units);
    }
    // реализуйте этот метод, чтобы он возвращал массив заводов, согласно приложенным таблицам
    public static Factory[] GetFactories()
    {
        string factories = File.ReadAllText(PathFactories);
        return JsonConvert.DeserializeObject<Factory[]>(factories);
    }

    // реализуйте этот метод, чтобы он возвращал установку (Unit), которой
    // принадлежит резервуар (Tank), найденный в массиве резервуаров по имени
    // учтите, что по заданному имени может быть не найден резервуар
    public static Unit FindUnit(Unit[] units, Tank[] tanks, string tankName)
    {
        int? unitId = tanks
            .ToList()
            .Where(x => x.Name == tankName)
            .Select(x => x.UnitId)
            .First();
        if (unitId != null)
        {
            return units
                .ToList()
                .Where(x => x.Id == unitId)
                .First();
        }
        return new Unit();
    }
    
    // реализуйте этот метод, чтобы он возвращал объект завода, соответствующий установке
    public static Factory FindFactory(Factory[] factories, Unit unit)
    {
        return factories
            .ToList()
            .Where(x => x.Id == unit.FactoryId)
            .First();
    }

    // реализуйте этот метод, чтобы он возвращал суммарный объем резервуаров в массиве
    public static int GetTotalVolume(Tank[] units)
    {
        return units
            .ToList()
            .Select(x => x.MaxVolume)
            .Sum(); ;
    }
}
