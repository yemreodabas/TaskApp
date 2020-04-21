﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;

namespace TaskApp.Services
{
    public interface IMissionService
    {
        void AddNewMission(Mission mission);
        void Delete(int id);
        MissionModel GetById(int id);
        List<MissionModel> GetAllMission();

    }
}
