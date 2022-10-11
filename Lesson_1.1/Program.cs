using Newtonsoft.Json;

class Program
{
    private const string PathJson = "JSON_sample_1.json";
    static void Main(string[] args)
    {
        DealWorker dealWorker = new DealWorker();
        var deals = dealWorker.GetDeals(PathJson);
        var numers = dealWorker.GetSumsByMonth(deals);
    }
}

public class DealWorker
{
    public IEnumerable<Deal> GetDeals(string path)
    {
        if(path == null) throw new ArgumentNullException("path");
        string deals = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<IEnumerable<Deal>>(deals);

    }

    public IList<string> GetNumbersOfDeals(IEnumerable<Deal> deals)
    {
        return deals
            .Where(x => x.Sum >= 100)
            .OrderBy(x => x.Date)
            .Take(5)
            .OrderByDescending(x => x.Sum)
            .Select(x => x.Id)
            .ToList();
    }

    public record SumByMonth(DateTime Month, int Sum);

    public IList<SumByMonth> GetSumsByMonth(IEnumerable<Deal> deals)
    {
        return deals
            .GroupBy(x => x.Date.Month)
            .Select(x => new SumByMonth( new DateTime(01, x.Key, 0001), x.Select(x => x.Sum).Sum()))
            .ToList();
    }
}
