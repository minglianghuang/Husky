﻿using System;

namespace Husky.Principal
{
	public class Principal<T> : Identity, IIdentity, IPrincipalUser where T : struct, IFormattable, IEquatable<T>
	{
		public Principal(IIdentityManager identityManager, IServiceProvider serviceProvider) {

			var identity = identityManager.ReadIdentity();
			if ( identity != null && identity.IsAuthenticated ) {
				IdString = identity.IdString;
				DisplayName = identity.DisplayName;
				identityManager.SaveIdentity(this);
			}

			IdentityManager = identityManager;
			ServiceProvider = serviceProvider;
		}

		public T? Id {
			get => Id<T>();
			set => IdString = value?.ToString();
		}

		public IIdentityManager IdentityManager { get; private set; }
		public IServiceProvider ServiceProvider { get; private set; }
	}
}