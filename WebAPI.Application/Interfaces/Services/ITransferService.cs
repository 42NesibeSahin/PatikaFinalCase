using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Application.DTOs;

namespace WebAPI.Application.Interfaces.Services
{
    public interface ITransferService
    {
		Task<TransferDto> GetByIDAsync(int id);
		Task<TransferDto> AddTransferAsync(TransferEkleDto ekleDto);

	}
}
