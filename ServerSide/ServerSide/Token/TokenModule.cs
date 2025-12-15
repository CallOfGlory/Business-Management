using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class TokenModule
{
    private readonly string _issuer = "Server_issuer";
    private readonly string _secretKey = "your_very_long_secret_key_that_is_at_least_16_chars";
    private readonly string _audience = "Grand_audience";

    public string GenerateToken(int id, int expires = 2400)
    {
        List<Claim> claims = new List<Claim>();


        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

        SigningCredentials signatureCreditails =
            new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        claims.Add(new Claim(ClaimTypes.NameIdentifier, id.ToString()));

        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expires),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = signatureCreditails,
        };

        JwtSecurityTokenHandler jwtToken = new JwtSecurityTokenHandler();
        SecurityToken securityToken = jwtToken.CreateToken(tokenDescriptor);
        string token = jwtToken.WriteToken(securityToken);

        return token;

    }

    public bool ValidateToken(string token)
    {
        TokenValidationParameters validationTokenParams = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _issuer,

            ValidateAudience = true,
            ValidAudience = _audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_secretKey)),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,

            RequireExpirationTime = true
        };

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        SecurityToken validatedToken;
        var principal = tokenHandler.ValidateToken(
            token,
            validationTokenParams,
            out validatedToken
        );
        return true;
    }

}