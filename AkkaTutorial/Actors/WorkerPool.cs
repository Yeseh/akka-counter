using Akka.Actor;
using Akka.Event;
using AkkaTutorial.Messages;

namespace AkkaTutorial.Actors;

internal class WorkerPool : UntypedActor
{
    private int WorkerCount = 0;
    private int WorkersDone = 0;
    private bool Done = false;

    private IActorRef State, Output;
    private List<IActorRef> Workers = new();
    protected ILoggingAdapter _log { get; } = Context.GetLogger();

    public WorkerPool(IActorRef state, IActorRef output, int workers)
    {
        State = state;
        WorkerCount = workers;
        Output = output;
    }

    protected override void PreStart()
    {
        _log.Info($"Starting pool with {WorkerCount} workers");
        for (int i = 0; i < WorkerCount; i++)
        {
            var worker = Context.ActorOf(Worker.Props(State, Output));
            Workers.Add(worker);
        }
    }
    
    protected override void OnReceive(object message)
    {
        switch (message)
        {
            case "start":
                if (WorkersDone == 0)
                {
                    foreach (var w in Workers)
                    {
                        w.Tell(new TryCount(Guid.NewGuid()));
                    }
                }
                break;

            case "worker-done":
                WorkersDone++;
                if (WorkersDone == WorkerCount)
                {
                    State.Tell(new ReadCount(Guid.NewGuid()));
                }
                break;

            case RespondCountState msg:
                Done = msg.Value.target <= msg.Value.current;
                if (Done) 
                { 
                    _log.Info("Done counting!");
                    Context.Stop(Self);
                }
                else
                { 
                    WorkersDone = 0;
                    Self.Tell("start");
                }
                break;
        }
    }

    public static Props Props(IActorRef state, IActorRef output, int workers)
        => Akka.Actor.Props.Create(() => new WorkerPool(state, output, workers));
}
