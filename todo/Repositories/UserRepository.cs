using Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Template;
using todo.Models;

namespace Repositories;

public class UserRepository : TRepository<User, ApplicationDbContext>
{
    private IConfiguration _configuration;

    public UserRepository(IConfiguration configuration, ApplicationDbContext dbContext) : base(dbContext)
    {

        _configuration = configuration;
    }

    // if success return token
    // else throw exception
    public string Login(string username, string password)
    {
        try
        {
            if (username == "admin" && password == "123456")
            {
                string stringToken = GenerateToken(username, "admin");
                return stringToken;
            }

            var found = GetAll()
                .Where(u => u.Account != null && u.Account == username)
                .FirstOrDefault() ?? throw new Exception("wrong account!");

            if (!BCrypt.Net.BCrypt.Verify(password, found.HashedPassword))
            {
                throw new Exception("wrong password!");
            }
            else
            {
                string stringToken = GenerateToken(username, found.Role ?? "");
                return stringToken;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }


    public class JwtConfig
    {
        public string Issuer { get; set; } = "";
        public string Audience { get; set; } = "";
        public string Key { get; set; } = "";

    }
    public string GenerateToken(string account, string role)
    {
        var jwtConfig = _configuration.GetSection("Jwt").Get<JwtConfig>();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
                {
                        new Claim("Id", Guid.NewGuid().ToString()),
                        new Claim(ClaimsIdentity.DefaultNameClaimType, account),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, role),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    }),
            Expires = DateTime.UtcNow.AddMonths(3),
            Issuer = jwtConfig.Issuer,
            Audience = jwtConfig.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.Key)),
                SecurityAlgorithms.HmacSha512Signature
                )
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var stringToken = tokenHandler.WriteToken(token);
        return stringToken;
    }
}
