using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaTutorial.Messages;

public record ReadCount : Message
{
    public ReadCount(Guid requestId)
        : base(requestId) { }
}

public record TryCount : Message
{
    public TryCount(Guid requestId)
        : base(requestId) { }
}

public record RespondCountState : Message<(int current, int target)>
{
    public RespondCountState((int current, int target) value, Guid requestId)
        : base(value, requestId) { }
}

public record OutputStatus : Message<bool>
{
    public OutputStatus(bool value, Guid requestId)
        : base(value, requestId) { }
}


public record PermitCount: Message<int>
{
    public PermitCount(int value, Guid requestId)
        : base(value, requestId) { }
}

public record RecordCount : Message<int>
{
    public RecordCount(int value, Guid requestId)
        : base(value, requestId) { }
}

public record CountRecorded : Message
{
    public CountRecorded(Guid requestId)
        : base(requestId) { }
}

public record CountDenied
{
    public static CountDenied Instance { get; } = new();

    private CountDenied() { }
}



