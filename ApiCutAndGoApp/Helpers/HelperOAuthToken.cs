using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiCutAndGoApp.Helpers {
    public class HelperOAuthToken {

        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }

        public HelperOAuthToken(IConfiguration configuration) {
            this.Issuer = configuration.GetValue<string>("ApiOAuth:Issuer");
            this.Audience = configuration.GetValue<string>("ApiOAuth:Audience");
            this.SecretKey = configuration.GetValue<string>("ApiOAuth:SecretKey");
        }

        // Necesitamos un método para generar el Token a partir del secret key
        public SymmetricSecurityKey GetKeyToken() {
            byte[] data = Encoding.UTF8.GetBytes(this.SecretKey);
            return new SymmetricSecurityKey(data);
        }

        // Necesitamos un método para poder habilitar los servicios de seguridad
        // dentro del program. Dicho método devuelve ACTION
        public Action<JwtBearerOptions> GetJwtOptions() {
            Action<JwtBearerOptions> options = new Action<JwtBearerOptions>(options => {
                // Validación para nuestro Token
                options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = this.Issuer,
                        ValidAudience = this.Audience,
                        IssuerSigningKey = this.GetKeyToken()
                };
            });
            return options;
        }

        // Método para indicar el esquema de la autentificación
        public Action<AuthenticationOptions> GetAuthenticationOptions() {
            Action<AuthenticationOptions> options =
                new Action<AuthenticationOptions>(options => {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                });
            return options;
        }

    }
}