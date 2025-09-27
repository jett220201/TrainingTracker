using TrainingTracker.Application.Interfaces.Helpers;

namespace TrainingTracker.Infrastructure.Helpers
{
    public class FitnessCalculator : IFitnessCalculator
    {
        public decimal CalculateBFP(decimal weight, decimal height, int age, int genderFactor)
        {
            return (1.2m * CalculateBMI(weight, height)) + (0.23m * age) - (10.8m * genderFactor) - 5.4m;
        }

        public decimal CalculateBMI(decimal weight, decimal height)
        {
            return weight / (height * height);
        }

        public decimal CalculateProgressPercent(decimal currentValue, decimal goalValue)
        {
            if (goalValue == 0) return 0;
            if(currentValue > goalValue)
            {
                return (goalValue / currentValue) *100;
            }
            else
            {
                return (currentValue / goalValue) * 100;
            }
        }
    }
}
