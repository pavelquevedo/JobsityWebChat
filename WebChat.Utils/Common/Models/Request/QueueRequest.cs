﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebChat.Utils.Common.Models.Request
{
    public class QueueRequest
    {
        public int RoomId { get; set; }
        public string Message { get; set; }
    }
}