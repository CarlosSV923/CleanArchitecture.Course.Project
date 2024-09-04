namespace CleanArchitecture.Course.Project.Domain.Entities.Alquileres
{
    public sealed record DateRange
    {
        public DateOnly Start { get; init; }
        public DateOnly End { get; init; }
        private DateRange(DateOnly start, DateOnly end)
        {
            Start = start;
            End = end;
        }

        public int CantidadDias => End.DayNumber - Start.DayNumber;

        public static DateRange Create(DateOnly start, DateOnly end)
        {
            if (start > end)
            {
                throw new InvalidOperationException("La fecha de inicio no puede ser mayor a la fecha de fin");
            }

            return new DateRange(start, end);
        }
    }
}