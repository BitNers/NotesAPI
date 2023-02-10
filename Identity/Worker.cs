using Identity.Data;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static System.Net.WebRequestMethods;

namespace Identity
{
    public class Worker : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public Worker(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync();
                        
            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
            
            if (await manager.FindByClientIdAsync("notesapi") is null)
            {


                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "notesapi",
                    DisplayName = "Notes API",
                    RedirectUris = { new Uri("https://localhost:8000/Identity/Account/Login") },
                    Permissions =
                {

                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.Password,
                    Permissions.ResponseTypes.Code,
                    Permissions.Endpoints.Introspection

                }
                }) ; 
            }

        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
