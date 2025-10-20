using AchievementsApi.Abstractions;
using AchievementsApi.BLL.DTO;

namespace AchievementsApi.BLL.RewardStrategies
{
    public class StrengthTrainingRewardCalculator : IAchievementRewardCalculator
    {
        public void CalculateReward(AchievementDto achievement)
        {
            achievement.Reward = Math.Round(achievement.Value / 10m);
        }
    }
}
