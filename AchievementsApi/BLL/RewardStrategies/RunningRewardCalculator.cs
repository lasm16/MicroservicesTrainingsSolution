using AchievementsApi.Abstractions;
using AchievementsApi.BLL.DTO;

namespace AchievementsApi.BLL.RewardStrategies
{
    public class RunningRewardCalculator : IAchievementRewardCalculator
    {
        public void CalculateReward(AchievementDto achievement)
        {
            achievement.Reward = Math.Round(achievement.Value * 0.5m);
        }
    }
}
