var random = new Random();
var decisionId = "volume-setting";

for (var i = 1; i <= 5; i++)
{
    var from = random.Next(0, 3);
    var to = random.Next(0, 3);

    var evt = new DecisionChanged(
        decisionId,
        "user-1",
        "level",
        from,
        to,
        i,
        DateTime.UtcNow
    );

    Console.WriteLine(evt);
    await Task.Delay(500);
}
