using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Contracts;

namespace Persistance.Data.DataSeeding
{

    public class DbInitializer : IDbInitializer
    {
        private readonly AppDbContext _dbContext;

        public DbInitializer(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task InitializeAync()
        {
            //1. Check if we have a bulding migration or not
            try
            {
                if (_dbContext.Database.GetPendingMigrations().Any()) {
                    // Migrate any building migrations
                    await _dbContext.Database.MigrateAsync();
                    // check if product,brand or type has no data in databse, then start seeding
                    if (!_dbContext.ProductTypes.Any()){
                        //D:\ROUTE\Route\Backend\API Project\E-Commerce\Infrastructure\Persistance\Data\DataSeeding\types.json
                        var typeData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistance\Data\DataSeeding\types.json");
                        // convert fron json to c# object (deserlilzation)
                        var types = JsonSerializer.Deserialize<List<ProductType>>(typeData);
                        if (types is not null && types.Any()) {
                             await _dbContext.AddRangeAsync(types);
                            await _dbContext.SaveChangesAsync();
                        }
                    }

                    if (!_dbContext.ProductBrands.Any())
                    {
                        //D:\ROUTE\Route\Backend\API Project\E-Commerce\Infrastructure\Persistance\Data\DataSeeding\types.json
                        var brandsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistance\Data\DataSeeding\brands.json");
                        // convert fron json to c# object (deserlilzation)
                        var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                        if (brands is not null && brands.Any())
                        {
                            await _dbContext.AddRangeAsync(brands);
                            await _dbContext.SaveChangesAsync();
                        }
                    }

                   if(!_dbContext.Products.Any())
                   // if(_dbContext.Products.Count() == 0)
                    {
                        //D:\ROUTE\Route\Backend\API Project\E-Commerce\Infrastructure\Persistance\Data\DataSeeding\types.json
                        var productsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistance\Data\DataSeeding\products.json");
                        // convert fron json to c# object (deserlilzation)
                        var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                        if (products is not null && products.Any())
                        {
                            await _dbContext.AddRangeAsync(products);
                            await _dbContext.SaveChangesAsync();
                        }
                    }
                }
                

            
            
            }
            catch (Exception ex) {

            }

        }
    }
}
