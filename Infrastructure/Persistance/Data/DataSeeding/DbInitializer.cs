using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities.OrderEntities;
using Microsoft.AspNetCore.Identity;

namespace Persistance.Data.DataSeeding
{

    public class DbInitializer : IDbInitializer
    {
        private readonly AppDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public DbInitializer(AppDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task InitializeAync()
        {
            //1. Check if we have a bulding migration or not
            try
            {
               // if (_dbContext.Database.GetPendingMigrations().Any()) {
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


                    if (!_dbContext.DeliveryMethods.Any())
                    // if(_dbContext.Products.Count() == 0)
                    {
                        //D:\ROUTE\Route\Backend\API Project\E-Commerce\Infrastructure\Persistance\Data\DataSeeding\types.json
                        var methodsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistance\Data\DataSeeding\delivery.json");
                    // convert fron json to c# object (deserlilzation)
                        var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(methodsData);
                        if (methods is not null && methods.Any())
                        {
                            await _dbContext.AddRangeAsync(methods);
                            await _dbContext.SaveChangesAsync();
                        }
                    }
               // }
                

            
            
            }
            catch (Exception ex) {

            }

        }

        public async Task InitializeIdentityAsync()
        {
            // seed roles
            if (!_roleManager.Roles.Any())
            {
                // Admin, SuperAdmin
               await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));

            }
            //seed users
            
            if (!_userManager.Users.Any())
            {
                var adminUser = new User()
                {
                    DisplayName="Admin",
                    UserName="Admin",
                    Email="Admin@gmail.com",
                    PhoneNumber="01013145678"
                };

                var superAdminUser = new User()
                {
                    DisplayName = "SuperAdmin",
                    UserName = "SuperAdmin",
                    Email = "SuperAdmin@gmail.com",
                    PhoneNumber = "01013145678"
                };

                await _userManager.CreateAsync(adminUser, "P@ssw0rd");
                await _userManager.CreateAsync(superAdminUser, "P@ssw0rd@");


                // assign to each user a role
                await _userManager.AddToRoleAsync(adminUser, "Admin");
                await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
            }



        }
    }
}
