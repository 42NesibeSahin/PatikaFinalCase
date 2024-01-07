using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Application.Interfaces.Repositories;
using WebAPI.Domain.Entities;
using WebAPI.Persistence.Context;

namespace WebAPI.Persistence.Repositories
{
	public class AutoPaymentRepository : GenericRepository<AutoPayment>, IAutoPaymentRepository
	{
		public AutoPaymentRepository(DataContext context) : base(context)
		{

		}
	}
}
