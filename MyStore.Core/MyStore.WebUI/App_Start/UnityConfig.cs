using MyStore.Core.Contracts;
using MyStore.Core.Models;
using MyStore.DataAccess.InMemory;
using MyStore.DataAccess.SQL;
using MyStore.Services;
using System;

using Unity;

namespace MyStore.WebUI
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>

        //RegisterTypes method that gets called from our activator and tells it to register all our types.
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();

            //// Registered our two repositories
            //container.RegisterType<IRepository<Product>, InMemoryRepository<Product>>();
            //container.RegisterType<IRepository<ProductCategory>, InMemoryRepository<ProductCategory>>();

            //Implementation changed with SQLRepository
            container.RegisterType<IRepository<Product>, SQLRepository<Product>>();
            container.RegisterType<IRepository<ProductCategory>, SQLRepository<ProductCategory>>();

            //Connecting new two (Cart, CartItems) repositories

            container.RegisterType<IRepository<Cart>, SQLRepository<Cart>>();
            container.RegisterType<IRepository<CartItem>, SQLRepository<CartItem>>();

            //Registering CartService
            container.RegisterType<ICartService, CartService>();

            //Injecting customer repository
            container.RegisterType<IRepository<Customer>, SQLRepository<Customer>>();
        }
    }
}