using CarRental.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Data;

namespace CarRental.DAL.Configurations
{
    public class CarConfigurations : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.Property(my => my.ModelYear).IsRequired()
                .HasColumnType(SqlDbType.Int.ToString());
            builder.Property(ds => ds.Description)
                .HasColumnType(SqlDbType.NChar.ToString())
                .HasMaxLength(50);
        }
    }
}
