﻿using Husky.Authentication.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Husky.PrincipalData
{
	public static class PrincipalExtensions
    {
		public static Baggage Baggage(IPrincipal principal) {

			if ( principal.IsAnonymous ) {
				return null;
			}
			var cache = principal.ServiceProvider.GetService<IMemoryCache>();
			return new BaggagePool<Baggage>(cache).PickOrPrepare(principal.IdString, new Baggage(principal));
		}
	}
}