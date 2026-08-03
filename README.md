# Sistema de Gerenciamento de Tarefas
Trabalho em grupo — Módulo 09 (ASP.NET Core MVC) — Senac Entra21.
## Como rodar o projeto
1. Clone o repositório
2. Ajuste a connection string em `appsettings.json` com sua senha do MySQL
3. Rode as migrations:
dotnet ef database update
4. Rode o projeto (`dotnet run` ou F5 no Visual Studio)
## Tecnologias
- ASP.NET Core MVC (.NET 8)
- Entity Framework Core (Code First)
- MySQL
- Autenticação própria via cookie + Claims (sem Identity pronto)
## Divisão do trabalho
- **Gabriel Henrique:** esqueleto do projeto, Models, ApplicationDbContext
- **José Lucas:** Repository Pattern, CRUD de Tarefas
- **Adrian Gazzani:** Autenticação (cadastro, login, hash de senha, cookie)
- **Eduardo Adão:** Layout e Views
- **Everson José:** Integração final ([Authorize], vínculo usuário-tarefa, filtro por status)