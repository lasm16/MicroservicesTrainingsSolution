using AchievementsApi.BLL.Abstractions;
using DataAccess.Models;

namespace AchievementsApi.BLL.RewardStrategies
{
    public class StrengthTrainingRewardCalculator : IAchievementRewardCalculator
    {
        public void CalculateReward(Achievement achievement)
        {
            achievement.Reward = Math.Round(achievement.Value / 10m);
        }
    }
}
