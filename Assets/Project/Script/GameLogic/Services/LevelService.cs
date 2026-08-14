using System.Collections.Generic;
using Models;

namespace GameLogic.Services
{
    public class LevelService
    {
        private List<LevelConfig> _levelConfigs;

        public LevelService(List<LevelConfig> levelConfigs)
        {
            _levelConfigs = levelConfigs;
        }

        public LevelConfig GetCurrentLevel()
        {
            return _levelConfigs[0];
        }
    }
}