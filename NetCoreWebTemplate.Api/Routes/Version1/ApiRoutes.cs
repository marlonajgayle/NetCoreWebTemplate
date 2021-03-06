﻿namespace NetCoreWebTemplate.Api.Routes.Version1
{
    public static class ApiRoutes
    {
        public const string Domain = "api";
        public const string Version = "v1";
        public const string Base = Domain + "/" + Version;

        public static class Client
        {
            public const string Create = Base + "/clients";
            public const string Get = Base + "/clients/{clientId}";
            public const string GetAll = Base + "/clients";
            public const string Update = Base + "/clients/{clientId}";
            public const string Delete = Base + "/clients/{clientId}";
        }

        public static class Identity
        {
            public const string Create = Base + "identity/account";
            public const string Login = Base + "identity/login";
        }
    }
}
