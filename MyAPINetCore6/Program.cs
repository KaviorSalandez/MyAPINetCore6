using Microsoft.EntityFrameworkCore;
using MyAPINetCore6.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// để api của mình cho mọi người sài => addCors
// lí do: default của trình duyệt mình chỉ cho phép kết nối với các resourse trong cùng cái web đó thôi
// mà nếu API chỉ cho cùng API mình gọi thì đâu còn ý nghĩa của API
// API mình định nghĩa ,pulblic , expose công bố ra các endpoint cho những ứng dụng khác sài
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
{
    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
}));

//đăng kí khai báo kết nối dbcontext
builder.Services.AddDbContext<BookStoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookStoreString"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
