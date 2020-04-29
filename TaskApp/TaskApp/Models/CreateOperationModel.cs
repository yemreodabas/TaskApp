using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskApp.Models
{
    public class CreateOperationModel
    {
        public string Name { get; set; }
        public int MissionId { get; set; }
        public int OperationStatus { get; set; }
    }
}
