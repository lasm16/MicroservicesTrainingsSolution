using DataAccess.Models;

namespace AchievementsApi.BLL.Abstractions
{
    public interface IAchievementRewardCalculator
    {
        void CalculateReward(Achievement achievement);
    }
}
