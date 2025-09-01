# Minimal API - Gerenciamento de Veículos

Este projeto implementa uma API REST minimalista utilizando .NET 8 para cadastro e gerenciamento de veículos, com autenticação JWT, integração com MySQL e testes.

## Funcionalidades
- Cadastro, consulta, atualização e remoção de veículos
- Cadastro e autenticação de administradores
- Autenticação JWT para rotas protegidas
- Validação de dados
- Documentação via Swagger
- Testes dos endpoints principais 

## Tecnologias Utilizadas
- .NET 8
- Entity Framework Core
- MySQL
- JWT Authentication
- Swagger

## Como executar
1. Configure o MySQL e as strings de conexão em `appsettings.json`
2. Execute as migrações para criar as tabelas
3. Inicie a API com `dotnet run` na pasta `Api`
4. Acesse a documentação em `/swagger`
5. Execute os testes com `dotnet test` na pasta `Test`