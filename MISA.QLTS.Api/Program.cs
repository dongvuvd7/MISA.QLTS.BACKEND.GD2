using MISA.QLTS.Core.Exceptions;
using MISA.QLTS.Core.Interfaces.Repositories;
using MISA.QLTS.Core.Interfaces.Services;
using MISA.QLTS.Core.Services;
using MISA.QLTS.Infrastructure.Repositories;

///
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(
    options =>
    {
        options.Filters.Add<HttpResponseExceptionFilter>();
    }
);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

builder.Services.AddScoped<IAssetService, AssetService>();
builder.Services.AddScoped<IAssetRepository, AssetRepository>();

builder.Services.AddScoped<IAssetCategoryService, AssetCategoryService>();
builder.Services.AddScoped<IAssetCategoryRepository, AssetCategoryRepository>();

builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

builder.Services.AddScoped<ICostSourceService, CostSourceService>();
builder.Services.AddScoped<ICostSourceRepository, CostSourceRepository>();

builder.Services.AddScoped<ILicenseService, LicenseService>();
builder.Services.AddScoped<ILicenseRepository, LicenseRepository>();

builder.Services.AddScoped<ILicenseDetailRepository, LicenseDetailRepository>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
            .WithOrigins(
                    "http://localhost:8080") //Note:  The URL must be specified without a trailing slash (/).
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(_ => true)
            .AllowCredentials();
        });


});

///
///Tao service Authentication Middleware trong method ConfigureServices trong Program.cs
//AuthenticationCheme duoc truyen toi method AddAuthentication thiet lap gia tri default authentication scheme cho ung dung
//CookieAuthenticationDefaults.AuthenticationScheme cung cap 1 gia tri cua Cookies cho scheme
//(Co the cung cap bat ki mot gia tri string nao cho scheme
//Co nhieu option khac ma co the su dung trong cac truong hop cu the CookieAuthenticationOptions Class)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie = new CookieBuilder
        {
            Name = ".aspNetCoreDemo.Security.Cookie",
            Path = "/",
            HttpOnly = true,
            SecurePolicy = CookieSecurePolicy.SameAsRequest,
            SameSite = SameSiteMode.Lax,
        };

        options.Cookie.Name = "MISA.QLTS.UserLoginSecurityCookie"; //Ten cookie tra ve browser
        options.ExpireTimeSpan = TimeSpan.FromDays(1); //Thoi gian cookie se het han ke tu thoi diem no duoc tao (do trinh duyet van con luu no)
        options.Cookie.MaxAge = options.ExpireTimeSpan; //Thoi gian cookie ton tai o trinh duyet, neu khong co options nay thi cookie se het hieu luc sau ExpireTimeSpan nhung van ton tai o trinh duyet, chi mat khi dong trinh duyet
        options.SlidingExpiration = true; //Trinh xu ly se cap lai cookie moi voi thoi gian het han duoc reset lai bat cu khi nao request gui lai (khi thoi gian hieu luc cua cookie cu da qua mot nua)
        options.AccessDeniedPath = "/Forbidden/"; //Chuyen huong khi xu ly ForbidAsync
        options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None; //Domain khac nhau thi can dung dong nay de tra ve cookie cho cac domain khac nhau

        options.EventsType = typeof(CustomCookieAuthenticationEvents);
    });

builder.Services.AddHttpContextAccessor();
///
builder.Services.AddSingleton<CustomCookieAuthenticationEvents>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
//app.UseHttpsRedirection();

///
app.UseAuthentication();
///



app.UseAuthorization();
///
app.MapDefaultControllerRoute();
///

app.MapControllers();

//app.UseCors(opt => opt.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:8080").SetIsOriginAllowed(_ => true).AllowCredentials());

app.Run();
