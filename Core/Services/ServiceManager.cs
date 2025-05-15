using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IProductService> _productService;
        private readonly Lazy<IBasketService> _basketService;
        private readonly Lazy<IAuthenticationService> _authenicationService;
        private readonly Lazy<IOrderService> _orderService;
        private readonly Lazy<IPaymentService> _paymentService;
        private readonly Lazy<ICacheService> _cacheService;




        public ServiceManager(IUnitOfWork unitOfWork,IMapper mapper,ICacheRepository cacheRepository ,IBasketRepository basketRepository,  UserManager<User> userManager, IOptions<JwtOptions> options, IConfiguration configuration)
        {
            _productService=new Lazy<IProductService>(()=>new ProductService(unitOfWork,mapper));
            _basketService = new Lazy<IBasketService>(() => new BasketService(basketRepository, mapper));
            _authenicationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(userManager,mapper,options));
            _orderService= new Lazy<IOrderService>(()=> new OrderService(mapper,basketRepository,unitOfWork));
            _paymentService = new Lazy<IPaymentService>(() => new PaymentService(basketRepository, unitOfWork, mapper, configuration));
            _cacheService = new Lazy<ICacheService>(() => new CacheService(cacheRepository));

        }
        public IProductService ProductService => _productService.Value;

        public IBasketService BasketService => _basketService.Value;

        public IAuthenticationService AuthenticationService => _authenicationService.Value;

        public IOrderService OrderService => _orderService.Value;

        public IPaymentService PaymentService => _paymentService.Value;

        public ICacheService CacheService => _cacheService.Value;
    }
}
