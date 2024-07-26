using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicebusApp.Common.Events;

public class OrderCreatedEvent
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public string ProductName { get; set; }
}
