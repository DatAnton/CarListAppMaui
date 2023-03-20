using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace CarListApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(o =>
            {
                o.AddPolicy("AllowAll", a => a.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
            });

            builder.Services.AddDbContext<CarListDbContext>(o => o.UseSqlite($"Data Source=C:\\carListdb\\carList.db"));

            builder.Services.AddIdentityCore<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<CarListDbContext>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JwtSettings:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
            });

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapGet("/cars", async (CarListDbContext db) => await db.Cars.ToListAsync());
            app.MapGet("/cars/{id}", async (int id, CarListDbContext db) => await db.Cars.FindAsync(id) is Car car ? Results.Ok(car) : Results.NotFound());
            app.MapPut("/cars/{id}", async (int id, Car car, CarListDbContext db) =>
            {
                var record = await db.Cars.FindAsync(id);
                if(record == null)
                    return Results.NotFound();

                record.Make = car.Make;
                record.Model = car.Model;
                record.Vin = car.Vin;

                await db.SaveChangesAsync();
                return Results.NoContent();
            });
            app.MapDelete("/cars/{id}", async (int id, CarListDbContext db) =>
            {
                var record = await db.Cars.FindAsync(id);
                if (record == null)
                    return Results.NotFound();
                db.Remove(record);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            app.MapPost("/cars", async (Car car, CarListDbContext db) =>
            {
                await db.Cars.AddAsync(car);
                await db.SaveChangesAsync();
                return Results.Created($"/cars/{car.Id}", car);
            });

            app.MapPost("/login", async (LoginDto loginDto, CarListDbContext db, UserManager<IdentityUser> userManager) =>
            {
                var user = await userManager.FindByNameAsync(loginDto.UserName);
                if (user == null)
                    return Results.Unauthorized();

                var isValidPassword = await userManager.CheckPasswordAsync(user, loginDto.Password);
                if(!isValidPassword)
                    return Results.Unauthorized();

                //Generate token

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var roles = await userManager.GetRolesAsync(user);
                var claims = await userManager.GetClaimsAsync(user);
                var tokenClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("email_confirmed", user.EmailConfirmed.ToString())
                }
                .Union(claims)
                .Union(roles.Select(r => new Claim(ClaimTypes.Role, r)));

                var securityToken = new JwtSecurityToken(
                    issuer: builder.Configuration["JwtSettings:Issuer"],
                    audience: builder.Configuration["JwtSettings:Audience"],
                    claims: tokenClaims,
                    expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(builder.Configuration["JwtSettings:DurationInMin"])),
                    signingCredentials: credentials
                );

                var accessToken = new JwtSecurityTokenHandler().WriteToken(securityToken);

                var response = new AuthResponseDto
                {
                    UserId = user.Id,
                    UserName = loginDto.UserName,
                    Token = accessToken
                };

                return Results.Ok(response);
            })
            .AllowAnonymous();

            app.Run();
        }
    }
}