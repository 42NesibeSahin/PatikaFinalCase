using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Domain.Entities;

namespace WebAPI.Application.Helpers.UserHelpers
{
	public static class UserHelper
	{
		public static bool ExistRoleName(string roleName)
		{

			foreach (var value in Enum.GetValues(typeof(UserRoleEnum)))
			{
				var memberInfo = value.GetType().GetMember(value.ToString());
				var attributes = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);

				if (attributes.Length > 0 && ((DisplayAttribute)attributes[0]).Name == roleName)
				{
					return true;
				}
			}
			return false;
		}

	}
	public enum UserRoleEnum
	{
		[Display(Name = "admin")]
		admin = 1,
		[Display(Name = "user")]
		user = 2,
		[Display(Name = "auditor")]
		auditor = 3,
	}


}
