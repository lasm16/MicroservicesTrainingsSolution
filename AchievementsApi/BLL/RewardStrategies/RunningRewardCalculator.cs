using AchievementsApi.BLL.Abstractions;
using DataAccess.Models;

namespace AchievementsApi.BLL.RewardStrategies
{
    public class RunningRewardCalculator : IAchievementRewardCalculator
    {
        public void CalculateReward(Achievement achievement)
        {
            achievement.Reward = Math.Round(achievement.Value * 0.5m);
        }
    }
}
