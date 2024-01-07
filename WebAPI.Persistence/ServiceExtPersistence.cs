using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Application.Interfaces.Repositories;
using WebAPI.Application.Interfaces.Services;
using WebAPI.Domain.Common;
using WebAPI.Persistence.Repositories;
using WebAPI.Persistence.Services;

namespace WebAPI.Persistence
{
    public static class ServiceExtPersistence
    {
        public static void AddPersistenceSubService(this IServiceCollection service, IConfiguration configuration)
        {
            // Repositories
            service.AddScoped<IAccountRepository, AccountRepository>();
            service.AddScoped<IAutoPaymentRepository, AutoPaymentRepository>();
            service.AddScoped<ILoanRepository, LoanRepository>();
            service.AddScoped<ITicketRepository, TicketRepository>();
            service.AddScoped<ITransactionRepository, TransactionRepository>();
            service.AddScoped<ITransferRepository, TransferRepository>();
            service.AddScoped<IUserRepository, UserRepository>();

            // Services
            service.AddScoped<IAccountService, AccountService>();
            service.AddScoped<IAutoPaymentService, AutoPaymentService>();
            service.AddScoped<ILoanService, LoanService>();
            service.AddScoped<ITicketService, TicketService>();
            service.AddScoped<ITransactionService, TransactionService>();
            service.AddScoped<ITransferService, TransferService>();
            service.AddScoped<IUserService, UserService>();

            // Entity
            service.AddScoped<IEntity, Entity>();
        }

    }
}
