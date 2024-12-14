using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Payments.Application.ClientServices;
using Payments.Application.CompanyServices;
using Payments.Application.PaymentServices;
using Payments.Domain.Repositories;
using Payments.Infrastructure.Repositories;
using Payments.WebApi.Controllers.DataContracts;
using Payments.WebApi.Controllers.Validators;
using Payments.WebApi.Extensions;

namespace Payments.WebApi
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Payments API",
                    Version = "v1",
                    Description = "API para gerenciar pagamentos"
                });
            });
            services.AddDatabase(Configuration);
            
            services.AddScoped<IClientsService, ClientService>();
            services.AddScoped<IClientsRepository, ClientsRepository>();
            services.AddSingleton<IValidator<UpsertClientRequest>, UpsertClientRequestValidator>();
            
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ICompaniesRepository, CompaniesRepository>();
            services.AddSingleton<IValidator<UpsertCompanyRequest>, UpsertCompanyRequestValidator>();
            
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IPaymentsRepository, PaymentsRepository>();
            services.AddSingleton<IValidator<InsertPaymentRequest>, InsertPaymentRequestValidator>();
            
            Dapper.SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
        }
        
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseSwagger();
            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payments API v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}