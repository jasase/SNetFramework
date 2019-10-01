using System;

namespace Framework.Contracts.Extension.EventService
{
    public class EventMessage
  {
    private readonly Guid _id;
    private readonly DateTime _publishDate;

    public EventMessage()
    {
      _id = Guid.NewGuid();
      _publishDate = DateTime.Now;
    }

    public DateTime PublishDate
    {
      get { return _publishDate; }      
    }

    public Guid MessageId
    {
      get { return _id; }
    }    
  }
}
