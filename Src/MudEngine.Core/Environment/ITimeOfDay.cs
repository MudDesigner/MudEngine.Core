namespace MudDesigner.MudEngine.Environment
{
    public interface ITimeOfDay : ICloneableComponent<ITimeOfDay>
    {
        int Hour { get; }
        int HoursPerDay { get; }
        int Minute { get; }

        void DecrementByHour(int hours);
        void DecrementByMinute(int minutes);
        void IncrementByHour(int hours);
        void IncrementByMinute(int minutes);
    }
}