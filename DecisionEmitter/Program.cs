var random = new Random();
var decisionId = "volume-setting";
var events = new List<DecisionChanged>();
var seen = new HashSet<string>();
var recent = new Dictionary<string, List<DateTime>>();
var window = TimeSpan.FromSeconds(15);


for (var i = 1; i <= 20; i++)
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
    Observe(evt);

    Console.WriteLine($"ACCEPTED {id[..8]}  {evt}");
    Thread.Sleep(random.Next(200, 800));
}

void Observe(DecisionChanged evt)
{
    var key = $"{evt.DecisionId}:{evt.Dimension}";

    if (!recent.TryGetValue(key, out var timestamps))
    {
        timestamps = new List<DateTime>();
        recent[key] = timestamps;
    }

    timestamps.Add(evt.OccurredAt);

    // forget old moments
    timestamps.RemoveAll(t => t < DateTime.UtcNow - window);

    // emit only when repetition becomes noticeable
    if (timestamps.Count == 4)
    {
        var signal = new AmbientSignal(
            evt.DecisionId,
            evt.Dimension,
            "oscillating",
            DateTime.UtcNow
        );

        Console.WriteLine($"AMBIENT {signal}");
    }
}
