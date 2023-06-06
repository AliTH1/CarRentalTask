namespace CarRental.Entities.Dtos.Cars
{
    public class GetCarDto
    {
        public int ModelYear { get; set; }
        public double DailyPrice { get; set; }
        public string? Description { get; set; }
    }
}
