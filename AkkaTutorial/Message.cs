using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaTutorial.Messages;

public abstract record Message
{
    public Guid RequestId { get; }

    public Message(Guid guid)
    {
        RequestId = guid;
    }
}

public abstract record Message<T>
{
    public Guid RequestId { get; }

    public T Value { get; }

    public Message(T value, Guid guid)
    {
        RequestId = guid;
        Value = value;
    }
}
