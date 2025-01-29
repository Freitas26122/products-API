# Products API

Esta API faz parte do sistema **Products**, uma plataforma desenvolvida para ajudar estudantes no preparo para o ENEM. A API foi construída utilizando **C#** e **.NET 6**, com **Entity Framework Core** para o gerenciamento de dados. Ela é responsável por lidar com a lógica de negócios, operações de CRUD e integração com o banco de dados **SQL Server**.

## Tecnologias Utilizadas

- **C#** com **.NET 6**
- **Entity Framework Core** para ORM
- **SQL Server** para armazenamento de dados
- **RESTful API** para comunicação com o frontend (Angular)
- **Swagger** para documentação automática da API

## Funcionalidades Principais

- Correção automatizada de redações com base em critérios do ENEM.
- Gerenciamento de usuários, aulas, conteúdos de estudo e progresso.
- Suporte para cadastro, edição, consulta e exclusão (CRUD) de aulas, redações e materiais.
- Integração com o front-end em **Angular**.
- Autenticação e autorização de usuários (em desenvolvimento).

## Requisitos de Instalação

Certifique-se de que o ambiente de desenvolvimento tenha os seguintes pré-requisitos:

- **.NET 6 SDK**
- **SQL Server** (ou outra versão compatível com o **Entity Framework Core**)
- **Visual Studio 2022** ou **Visual Studio Code**
- **Postman** (opcional, para testar endpoints)

## Configuração do Banco de Dados

1. Crie uma base de dados no **SQL Server**.
2. Atualize as **connection strings** no arquivo `appsettings.json` com os detalhes do seu banco de dados:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=SEU_SERVIDOR;Database=ProductsDb;User Id=SEU_USUARIO;Password=SUA_SENHA;"
}
```

3. Execute as migrações do Entity Framework para criar as tabelas:

```bash
dotnet ef database update
```

## Executando o Projeto

1. Clone o repositório e navegue até o diretório da API:

```bash
git clone https://github.com/seu-repositorio/Products-api.git
cd Products-api
```

2. Restaure os pacotes NuGet:

```bash
dotnet restore
```

3. Execute a aplicação:

```bash
dotnet run
```

A API estará disponível em `http://localhost:5000`. Você pode acessar a documentação do **Swagger** em `http://localhost:5000/swagger` para explorar e testar os endpoints.

## Testes

Para rodar os testes de unidade, use o seguinte comando:

```bash
dotnet test
```

## Contribuindo

Se você deseja contribuir para o projeto, siga estas etapas:

1. Faça um fork do repositório.
2. Crie uma nova branch para sua feature (`git checkout -b minha-feature`).
3. Faça commit das suas alterações (`git commit -m 'Adiciona minha feature'`).
4. Faça push para a branch (`git push origin minha-feature`).
5. Abra um Pull Request.