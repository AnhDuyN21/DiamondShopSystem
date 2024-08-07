﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons
{
    public class JWTSection
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }

    public class PayOSConfig
    {
        public string PAYOS_CHECKSUM_KEY { get; set; } = string.Empty;
        public string PAYOS_API_KEY { get; set; } = string.Empty;
        public string PAYOS_CLIENT_ID { get; set; } = string.Empty;
    }
    public class GoogleImage
    {
        public string type { get; set; }
        public string project_id { get; set; }
        public string private_key_id { get; set; }
        public string private_key { get; set; }
        public string client_email { get; set; }
        public string client_id { get; set; }
        public string auth_uri { get; set; }
        public string token_uri { get; set; }
        public string auth_provider_x509_cert_url { get; set; }
        public string client_x509_cert_url { get; set; }
        public string universe_domain { get; set; }
    }
    public class AppConfiguration
    {
        public string DatabaseConnection { get; set; }
        public JWTSection JWTSection { get; set; }
        public PayOSConfig PayOSConfig { get; set; }
        public GoogleImage GoogleImage { get; set; }

    }

}
