using AchievementsApi.BLL.DTO;

namespace AchievementsApi.Abstractions
{
    public interface IAchievementRewardCalculator
    {
        void CalculateReward(AchievementDto achievement);
    }
}
