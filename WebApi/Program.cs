// ����������� ����������� ����������� ���� ��� ������ � Entity Framework � ���-�����������
using Microsoft.EntityFrameworkCore; // ��� ������ � Entity Framework Core
using WebApi; // ��� ������� � ������ ApplicationDbContext (�������� ���� ������)

var builder = WebApplication.CreateBuilder(args); // �������� ������ ������� ��� ���-����������

// ���������� �������� � ��������� ������������

// ���������������� ��������� ���� ������, ��������� Npgsql ��� ����������� � PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
// ������������ ������ ����������� � ������ "DefaultConnection" �� ������������ ����������

// ���������������� CORS ��� ����������� ����������� ������� ����� �������� � �������� (��������, � React-�����������)
builder.Services.AddCors(options =>
{
    // ���������� �������� CORS � ������ "AllowReactApp"
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            // ���������� �������� � ���������� ��������� (��������, React-����������, ���������� �� localhost:3000)
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyHeader() // ���������� ����� ����������
                   .AllowAnyMethod(); // ���������� ����� ������� (GET, POST, PUT, DELETE � �.�.)
        });
});

// ���������� ������ ������������ ��� ��������� HTTP-��������
builder.Services.AddControllers();

// ��������� Swagger ��� ���������������� API
// �������������� ���������� � Swagger ����� ����� �� ������: https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); // ���������� ������ ��� ������ � �������� API �������� �����
builder.Services.AddSwaggerGen(); // ���������� Swagger ��� ��������� ������������ API

// �������� ����������
var app = builder.Build();

// ��������� ��������� ��������� HTTP-��������

// ��������� Swagger � Swagger UI ������ � ����� ����������
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // ��������� Swagger ��� ����������� ������������ API
    app.UseSwaggerUI(); // ��������� UI ��� ������������ Swagger-������������
}

// ���������� CORS ��� �������� "AllowReactApp", ����� ���������� ���������� ����� ���������� ������� �� ������
app.UseCors("AllowReactApp");

// ��������� ��������� �����������
app.UseAuthorization();

// ������������� �������� � ������������ API
app.MapControllers();

// ������ ����������
app.Run(); // ����� ���-����������, ��������� �������� ���������� � ����� �������
