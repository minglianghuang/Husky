﻿using System;
using System.ComponentModel.DataAnnotations;
using Husky.Sugar;

namespace Insider.Portal.Models.AccountModels
{
	public class RegistryVerifyModel
    {
		public Guid Id { get; set; }

		[Display(Name = "帐号")]
		public string AccountName { get; set; }

		[Required(ErrorMessage = "请输入您接收到的验证码。")]
		[StringLength(6, MinimumLength = 6, ErrorMessage = "请输入正确的验证码，长度6位。")]
		[Display(Name = "验证码")]
		public string DynamicPassCode { get; set; }


		public bool IsEmail => AccountName.IsEmail();
		public bool IsMobile => AccountName.IsMainlandMobile();
	}
}