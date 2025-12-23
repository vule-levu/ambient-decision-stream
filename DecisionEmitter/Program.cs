var random = new Random();
var decisionId = "volume-setting";
var events = new List<DecisionChanged>();
var seen = new HashSet<string>();


for (var i = 1; i <= 10; i++)
{
    var from = random.Next(0, 3);
    var to = random.Next(0, 3);

    events.Add(new DecisionChanged(
        decisionId,
        "user-1",
        "level",
        from,
        to,
        i,
        DateTime.UtcNow.AddSeconds(random.Next(-20, 5))
    ));
}

var shuffled = events.OrderBy(_ => random.Next()).ToList();

foreach (var evt in shuffled)
{
    Emit(evt);

    // force a duplicate ~30% of the time
    if (random.NextDouble() < 0.3)
    {
        Emit(evt);
    }
}


void Emit(DecisionChanged evt)
{
    var id = EventIdentity.From(evt);

    if (!seen.Add(id))
    {
        Console.WriteLine($"DUPLICATE IGNORED  {id[..8]}");
        return;
    }

    Console.WriteLine($"ACCEPTED {id[..8]}  {evt}");
    Thread.Sleep(random.Next(200, 800));
}
