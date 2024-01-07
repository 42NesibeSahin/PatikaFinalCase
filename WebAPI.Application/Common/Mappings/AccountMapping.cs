using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Application.DTOs;
using WebAPI.Domain.Entities;

namespace WebAPI.Application.Common.Mappings
{
	public class AccountMapping : Profile
	{
		public AccountMapping()
		{
			CreateMap<Account, AccountDto>().ReverseMap();
			CreateMap<Account, AccountEkleDto>().ReverseMap();
			CreateMap<AccountDto, AccountEkleDto>().ReverseMap();
		}
	}
}
