

using HotChocolate.Types;

namespace TournamentGraphpQlDemo.Domain;

[UnionType("MatchEvent")]
public interface IMatchEvent
{
     Guid Id { get; set; }
     Match Match { get; set; }
     DateTime Time { get; set; }
}

[InterfaceType("MatchEventBase")]
public abstract class MatchEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [IsProjected(false)]
    public Guid MatchId { get; set; }
    public Match Match { get; set; } = null!;
    public DateTime Time { get; set; }
}

public class MatchPlayerEvent :MatchEvent, IMatchEvent
{
    [IsProjected(false)]
    public Guid PlayerId { get; set; }
    [IsProjected(false)] public Player Player { get; set; } = null!;
    public MatchPlayerEventType EventType { get; set; }
    public bool IsHomeTeam { get; set; }
}

public class MatchGenericEvent : MatchEvent, IMatchEvent
{
    public MatchGenericEventType EventType { get; set; }
}

public enum MatchGenericEventType
{
    MatchStarted,
    FirstPeriodFinished,
    SecondPeriodeStared,
    MatchFinished,
    TimeStopped,
    TimeStarted
}

public enum MatchPlayerEventType
{
    Goal,
    GoalPenalty,
    MissedPenaltyGoal,
    Penalty,
    CausedPenalty,
    YellowCard,
    RedCard,
    Assist,   
}