using AchievementsApi.BLL.Abstractions;

namespace AchievementsApi.BLL.RewardStrategies
{
    public class RewardStrategyFactory
    {
        public static IAchievementRewardCalculator? CreateStrategy(DataAccess.Enums.AchievementType type)
        {
            return type switch
            {
                DataAccess.Enums.AchievementType.Running => new RunningRewardCalculator(),
                DataAccess.Enums.AchievementType.StrengthTraining => new StrengthTrainingRewardCalculator(),
                _ => throw new ArgumentException(message: $"Unexpected enum value: {type}", paramName: nameof(type)),
            };
        }
    }
}
