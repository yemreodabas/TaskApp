using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;

namespace TaskApp.Models
{
    public class MissionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public string MissionUserName { get; set; }

        public MissionModel() { }

        public MissionModel(Mission mission)
        {
            this.Id = mission.Id;
            this.Name = mission.Name;
            this.UserId = mission.UserId;
            this.MissionUserName = mission.MissionUsername;
        }
    }
}
