﻿using System.ComponentModel.DataAnnotations;
using Husky.Users.Data;
using Insider.Portal.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Insider.Portal.Models.AccountModels
{
	public class RegistryModel
	{
		const string _mobilePattern = @"^1[3578]\d{9}$";
		const string _emailPattern = @"^[-0-9a-zA-Z.+_]+@[-0-9a-zA-Z.+_]+\.[a-zA-Z]{2,4}$";

		const string _typeName = "邮箱";
		const string _typePattern = _emailPattern;
		public AccountNameType AccountNameType => AccountNameType.Email;

		[Required(ErrorMessage = "必须填写，请用您的" + _typeName + "作为帐号名。")]
		[RegularExpression(_typePattern, ErrorMessage = "格式无效，请用您的" + _typeName + "作为帐号名。")]
		[Remote(nameof(ApiController.IsAccountApplicable), "Api", AdditionalFields = nameof(AccountNameType), HttpMethod = "POST", ErrorMessage = "{0}已经被注册了。")]
		[Display(Name = _typeName)]
		public string AccountName { get; set; }

		[Required(ErrorMessage = "密码必须填写。")]
		[StringLength(18, MinimumLength = 8, ErrorMessage = "密码长度须在{2}-{1}位之间。")]
		[DataType(DataType.Password)]
		[Display(Name = "密码")]
		public  string Password { get; set; }

		[Required(ErrorMessage = "重复输入一遍密码。")]
		[MaxLength(15), Compare(nameof(Password), ErrorMessage = "两次密码输入不一致。")]
		[DataType(DataType.Password)]
		[Display(Name = "密码确认")]
		public  string PasswordConfirm { get; set; }
	}
}