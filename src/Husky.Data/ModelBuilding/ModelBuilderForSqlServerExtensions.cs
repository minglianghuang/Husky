﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Husky.Data.ModelBuilding.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Husky.Data.ModelBuilding
{
	public static class ModelBuilderForSqlServerExtensions
	{
		public static ModelBuilder ForSqlServer<TDbContext>(this ModelBuilder modelBuilder, CustomModelBuilderOptions options = null)
			where TDbContext : DbContext {
			return modelBuilder.ForSqlServer(typeof(TDbContext), options);
		}

		internal static ModelBuilder ForSqlServer(this ModelBuilder modelBuilder, Type dbContextType, CustomModelBuilderOptions options = null) {
			options = options ?? new CustomModelBuilderOptions();

			// Try to read all entities types from DbContext
			var entityTypeSets = dbContextType.GetProperties();
			foreach ( var potential in entityTypeSets ) {
				var entityType = potential.PropertyType.GetGenericArguments().FirstOrDefault();
				if ( entityType == null || potential.PropertyType.Name != "DbSet`1" ) {
					continue;
				}

				// Read all properties from current entity
				var entityTypeBuilder = modelBuilder.Entity(entityType);
				var properties = entityType.GetProperties();
				foreach ( var prop in properties ) {

					if ( options.AnnotatedDefaultValueSql || options.AutoDetectedDefaultValueSql ) {
						var valueSql = prop.GetCustomAttribute<DefaultValueSqlAttribute>();
						var propertyBuilder = entityTypeBuilder.Property(prop.PropertyType, prop.Name);

						// Enable sql default values for convenience
						if ( options.AnnotatedDefaultValueSql && valueSql?.ValueSql != null ) {
							propertyBuilder.ForSqlServerHasDefaultValueSql(valueSql?.ValueSql);
						}

						// Enable default datetime value for nummerics and datetimes
						if ( options.AutoDetectedDefaultValueSql && valueSql?.ValueSql == null && !prop.IsDefined(typeof(KeyAttribute)) ) {
							if ( typeof(DateTime) == prop.PropertyType ) {
								propertyBuilder.ForSqlServerHasDefaultValueSql("getdate()");
							}
							else if ( prop.PropertyType.GetTypeInfo().IsEnum || prop.PropertyType.GetTypeInfo().IsPrimitive || prop.PropertyType == typeof(decimal) ) {
								propertyBuilder.ForSqlServerHasDefaultValueSql("0");
							}
						}
					}

					// Enable Index annotations, if an Index is set as Clustered, then set Non-Clustered from the PrimaryKey
					if ( options.AnnotatedIndices ) {
						var indice = prop.GetCustomAttribute<IndexAttribute>();
						if ( indice != null ) {

							// Remove clustered from Primary Key
							if ( indice.IsClustered ) {
								var primaryKeyFieldNames = properties
									.Where(x => x.IsDefined(typeof(KeyAttribute)))
									.OrderBy(x => x.GetCustomAttribute<ColumnAttribute>()?.Order)
									.Select(x => x.Name)
									.ToArray();
								entityTypeBuilder.HasKey(primaryKeyFieldNames).ForSqlServerIsClustered(false);
							}

							// Add clustered to annotated properties
							// todo: support a property combination.
							entityTypeBuilder.HasIndex(prop.Name).IsUnique(indice.IsUnique).ForSqlServerIsClustered(indice.IsClustered);
						}
					}
				}
			}
			return modelBuilder;
		}
	}
}