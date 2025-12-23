var random = new Random();
var decisionId = "volume-setting";
var events = new List<DecisionChanged>();

for (var i = 1; i <= 5; i++)
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

foreach (var evt in events.OrderBy(_ => random.Next()))
{
    Console.WriteLine(evt);
    await Task.Delay(random.Next(200, 800));
}
