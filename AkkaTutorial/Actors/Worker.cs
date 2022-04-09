using Akka.Actor;
using AkkaTutorial.Messages;
using Akka.Event;

namespace AkkaTutorial.Actors;

internal class Worker : UntypedActor
{
    private int Number = -1;
    private IActorRef State, Output;
    protected ILoggingAdapter _log { get; } = Context.GetLogger();

    public Worker(IActorRef state, IActorRef output)
    {
        State = state;
        Output = output;
    }

    protected override void OnReceive(object message)
    {
        switch (message)
        {
            case TryCount msg:
                if (Number == -1) { State.Tell(msg); }
                else { Output.Tell(new RecordCount(Number, msg.RequestId)); }
                break;

            case PermitCount msg:
                Number = msg.Value;
                Output.Tell(new RecordCount(Number, msg.RequestId));
                break;

            case CountRecorded msg:
                // Reset worker after number is recorded
                Number = -1;
                Context.Parent.Tell("worker-done");
                break;

            case CountDenied msg:
                Context.Parent.Tell("worker-done");
                break;
        }
    }

    public static Props Props(IActorRef state, IActorRef output)
        => Akka.Actor.Props.Create(() => new Worker(state, output));
}
