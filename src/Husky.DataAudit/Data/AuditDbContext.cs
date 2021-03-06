﻿using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace Husky.DataAudit.Data
{
	public sealed class AuditDbContext : DbContext
	{
		public AuditDbContext(DbContextOptions<AuditDbContext> options) : base(options) {
		}

		public DbSet<AuditEntry> AuditEntries { get; set; }
		public DbSet<AuditEntryProperty> AuditEntryProperties { get; set; }
	}
}
