using Sales.Shared.Entities;

namespace Sales.API.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            this._context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountries();

        }

        private async Task CheckCountries()
        {
            if (!_context.Countries.Any()) {
                _context.Countries.Add(new Country { Name = "Colombia"});
                _context.Countries.Add(new Country { Name = "Perú" });
                _context.Countries.Add(new Country { Name = "México" });
                await _context.SaveChangesAsync();
            }
        }
    }
}
