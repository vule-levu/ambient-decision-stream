public record DecisionChanged(
    string DecisionId,
    string ActorId,
    string Dimension,
    int? From,
    int? To,
    long Sequence,
    DateTime OccurredAt
);
