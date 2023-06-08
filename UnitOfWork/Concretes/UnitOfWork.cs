using CarRental.DAL;
using CarRental.DAL.Repositories.Abstracts;
using CarRental.DAL.Repositories.Concretes.EFCore;
using CarRental.UnitOfWork.Abstracts;

namespace CarRental.UnitOfWork.Concretes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;
        private ICarRepository _carRepository;

        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public ICarRepository CarRepository => _carRepository = _carRepository ?? new EFCarRepository(_appDbContext);

        public async Task SaveAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}
