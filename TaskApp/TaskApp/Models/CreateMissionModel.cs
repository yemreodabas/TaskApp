﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskApp.Models
{
    public class CreateMissionModel
    {
        public string Name { get; set; }
        public int UserId { get; set; }
        public string MissionUsername { get; set; }
    }
}
