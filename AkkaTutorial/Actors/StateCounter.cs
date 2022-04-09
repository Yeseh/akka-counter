using Akka.Actor;
using Akka.Event;
using AkkaTutorial.Messages;

namespace AkkaTutorial.Actors;

internal class StateCounter : UntypedActor
{
    private int Target = -1;
    private int Value = 0;
    protected ILoggingAdapter _log { get; } = Context.GetLogger();

    public StateCounter(int target)
    {
        Target = target;
    }

    protected override void PreStart()
    {
        Console.WriteLine($"Initialized state Actor, counting to {Target}");
    }

    protected override void OnReceive(object message)
    {
        switch (message)
        {
            case ReadCount r:
                Sender.Tell(new RespondCountState((Value, Target), r.RequestId));
                break;

            case TryCount r:
                if (Value +1 <= Target)
                {
                    Value++;
                    Sender.Tell(new PermitCount(Value, r.RequestId));
                }
                else { Sender.Tell(CountDenied.Instance); }
                break;
        }
    }

    public static Props Props(int target) 
        => Akka.Actor.Props.Create(() => new StateCounter(target));
}
