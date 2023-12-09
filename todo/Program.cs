using System.Text;
using Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using Template;
using todo.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// CONFIG HERE:
/////////////////////////////--------------------------------------------------
// Add services to the container. vẫn chưa hiểu
//???
//builder.Services.AddControllersWithViews();
//#if !DEBUG
//builder.Host.ConfigureLogging((hostContext, logBuilder) =>
//        logBuilder.ClearProviders()
//            .AddFileLogger(configuration =>
//            {
//                hostContext.Configuration.GetSection("Logging").GetSection("RoundTheCodeFile").GetSection("Options").Bind(configuration);
//            }));
//#endif

//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//    serverOptions.Limits.MaxRequestBodySize = long.MaxValue;
//});
//??????
// Add services to the container. vẫn chưa hiểu
////??????
//builder.Services.AddDbContext<ApplicationDbContext>(option =>
//{
//#if DEBUG
//    var usedCnn = builder.Configuration["UsedConnectionString_Debug"];
//#else
//    var usedCnn = builder.Configuration["UsedConnectionString"];
//#endif
//    if (usedCnn == "PSQLDB")
//    {
//        option.UseNpgsql(builder.Configuration.GetConnectionString(usedCnn));
//    }
//    else
//    {
//        option.UseSqlServer(builder.Configuration.GetConnectionString(usedCnn));
//    }
//});
////??????
// thêm databse
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// thêm cors (chấp nhận tất cả các request gọi đến server này)
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("*")
                            .WithHeaders("*")
                            .WithMethods("*");
                      });
});
// thêm dịch vụ xác thực JWT
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
    });

// thêm repository (thêm bảng thực thề vào)
builder.Services.AddScoped<TRepository<User, ApplicationDbContext>, UserRepository>();
builder.Services.AddScoped<TRepository<Misson, ApplicationDbContext>, MissonRepository>();

/////////////////////////////--------------------------------------------------
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

