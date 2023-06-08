using CarRental.DAL.Repositories.Abstracts;

namespace CarRental.UnitOfWork.Abstracts
{
    public interface IUnitOfWork
    {
        public ICarRepository CarRepository { get; }
        Task SaveAsync();
    }
}
