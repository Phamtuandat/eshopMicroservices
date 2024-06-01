﻿namespace Identity.Api.Areas.Identity.Models.Account.Login
{
  
        public class LoginViewModel
        {
            public bool AllowRememberLogin { get; set; } = true;
            public bool EnableLocalLogin { get; set; } = true;

            public IEnumerable<ExternalProvider> ExternalProviders { get; set; } = Enumerable.Empty<ExternalProvider>();
            public IEnumerable<ExternalProvider> VisibleExternalProviders => ExternalProviders.Where(x => !string.IsNullOrWhiteSpace(x.DisplayName));

            public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders?.Count() == 1;
            public string? ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;

            public class ExternalProvider
            {
                public ExternalProvider(string authenticationScheme, string? displayName = null)
                {
                    AuthenticationScheme = authenticationScheme;
                    DisplayName = displayName;
                }

                public string? DisplayName { get; set; }
                public string AuthenticationScheme { get; set; }
            }
        }
}