using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IProductService> _productService;
        private readonly Lazy<IBasketService> _basketService;
        private readonly Lazy<IAuthenticationService> _authenicationService;



        public ServiceManager(IUnitOfWork unitOfWork,IMapper mapper, IBasketRepository basketRepository, IAuthenticationService authenticationService, UserManager<User> userManager) {
            _productService=new Lazy<IProductService>(()=>new ProductService(unitOfWork,mapper));
            _basketService = new Lazy<IBasketService>(() => new BasketService(basketRepository, mapper));
            _authenicationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(userManager));

        }
        public IProductService ProductService => _productService.Value;

        public IBasketService BasketService => _basketService.Value;

        public IAuthenticationService AuthenticationService => _authenicationService.Value;
    }
}
