using API.Middleware;
using Application.Features.Commands.CreateApplication;
using Application.Mapping;
using Application.Validators;
using Domain.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add FluentValidation and Validators
builder.Services.AddControllers();
builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

// Register all validators from the assembly containing the BaseValidator class
builder.Services.AddValidatorsFromAssemblyContaining<SubmitApplicationCommandValidator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnectionString")));
//Add CORS
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader()
        .AllowAnyMethod()
        .WithOrigins("http://localhost:3001");
    });
});
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IFictitiousSchoolApplicationRepository, FictitiousSchoolApplicationRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseDateRepository, CourseDateRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblies(typeof(SubmitApplicationCommandHandler).Assembly);
});

// Register ValidationBehaviour in the MediatR pipeline
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

var app = builder.Build();

// Apply migrations at startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy"); // enable CORS => This should come before UseHttpsRedirection

// Add middleware to handle exceptions and return proper JSON responses
app.UseMiddleware<ValidationExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();