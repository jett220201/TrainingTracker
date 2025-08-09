namespace TrainingTracker.Application.Interfaces.Helpers
{
    public interface IFitnessCalculator
    {
        public decimal CalculateBMI(decimal weight, decimal height); // Body Mass Index
        public decimal CalculateBFP(decimal weight, decimal height, int age, int genderFactor); // Body Fat Percentage
    }
}
