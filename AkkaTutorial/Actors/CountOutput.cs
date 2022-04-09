using Akka.Actor;
using Akka.Event;
using AkkaTutorial.Messages;

namespace AkkaTutorial.Actors;

internal class CountOutput : UntypedActor
{
    public int Value { get; private set; } = 0;

    protected ILoggingAdapter _log { get; } = Context.GetLogger();

    protected override void OnReceive(object message)
    {
        switch (message)
        {
            case RecordCount r:
                var bValid = r.Value - 1 == Value;
                if (bValid)
                {
                    Value = r.Value;
                    _log.Info($"Count: {Value}");
                    Sender.Tell(new CountRecorded(r.RequestId));
                }
                else
                {
                    Sender.Tell(CountDenied.Instance);
                }
                break;
        }
    }

    public static Props Props()
        => Akka.Actor.Props.Create(() => new CountOutput());
}
