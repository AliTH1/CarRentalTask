using CarRental.Core.DAL.Repositories.Concretes;
using CarRental.DAL.Repositories.Abstracts;
using CarRental.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CarRental.DAL.Repositories.Concretes.EFCore
{
    public class EFCarRepository : EFBaseRepository<Car, AppDbContext>, ICarRepository
    {
        public EFCarRepository(AppDbContext context) : base(context)
        {
        }
    }
}
