﻿using Microsoft.EntityFrameworkCore;

namespace Husky.TwoFactor.Data
{
	public class TwoFactorDbContext : DbContext
	{
		public TwoFactorDbContext(DbContextOptions<TwoFactorDbContext> options) : base(options) {
		}

		public DbSet<TwoFactorCode> TwoFactorCodes { get; set; }
	}
}
