﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicebusApp.Common;

public static class Constants
{
    public const string ConnectionString = "";
    public const string OrderCreatedQueueName = "OrderCreatedQueue";
    public const string OrderDeletedQueueName = "OrderDeletedQueue";

    public const string OrderTopic = "OrderTopic";
    public const string OrderCreatedSubName = "OrderCreatedSub";
    public const string OrderDeletedSubName = "OrderDeletedSub";
}
