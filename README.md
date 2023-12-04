# RPA.Scraping.Alura

### :pencil: Informações técnicas 
* **Interações:** https://www.alura.com.br/

### :dart: Objetivo
- Acessar o site da Alura
- Pesquisar um curso
- Capturar o Nome, Descrição, Professor e Carga Horária de todos os cursos encontrados.

### :zap: Desenvolvimento
1. Robô irá acessar o site da Alura
1. No campo "**O que você quer aprender**", irá inserir o curso pesquisado
1. Caso encontre algum curso, irá capturar o **Nome**, **Descrição** e Link do mesmo.
1. Após, acessará o link de todos os cursos capturados e em seguida irá capturar o **Nome do Professor** e **Carga Horária** do curso.
1. Caso haja mais de uma página de cursos, o robô irá percorrer todas e efetuará a captura dos dados.
1. Ao finalizar a raspagem dos dados, o robô irá armazenar as informações no banco de dados. 

### :fire: Robô parou! E agora?
Verificar os logs gerados em: **[diretório da aplicação]\Log\log.txt**



### ⚛️ **Arquitetura**
**Linguagem**
- C#11

**Tecnologias utilizadas:**
- [Microsoft.EntityFrameworkCore.Design] - Componentes de tempo de design compartilhados para ferramentas Entity Framework Core.
- [FluentValidation.AspNetCore] - FluentValidation pode ser usado em aplicativos Web ASP.NET Core para validar modelos recebidos.
- [Microsoft.EntityFrameworkCore.Tools] - Ferramentas principais do Entity Framework para o console do gerenciador de pacotes NuGet no Visual Studio.
- [Docker] - O Docker fornece um conjunto de ferramentas de desenvolvimento, serviços, conteúdo confiável e automações.

**Bibliotecas utilizadas:**
- [Selenium.WebDriver] - driver para controlar o navegador de internet.
- [Html Agility Pack] - Manipulador de documentos HTML.
- [Serilog.AspNetCore] - Gerador de logs para a aplicação.

### ✏️ **Banco de Dados**
- MySql

**Tabelas Automação:**
- cursos

### :green_book: Recursos necessários para testar a aplicação
- Necessário ter instalado o [.NET SDK 7](https://dotnet.microsoft.com/pt-br/download/dotnet/thank-you/sdk-7.0.403-windows-x64-installer).

**Docker**

Deve-se conter uma pasta chamada **data** na raiz do projeto, pois a mesma será utilizada para armazenar as informações do **Docker** e do **MySql**.

Iniciar o **Docker** pois o mesmo hospeda o banco de dados **MySql**, para isso, abra o **PowerShell** na pasta raíz do projeto. Após, execute o comando abaixo:
```sh
docker-compose up -d
```
No projeto **RPA.Scraping.Alura.Infra.Data** deve-se conter uma pasta chamada **Migrations** com alguns arquivos dentro, caso não tenha, executar o seguinte comando:

```sh
dotnet ef --startup-project RPA.Scraping.Alura.Application\RPA.Scraping.Alura.Application.csproj --project RPA.Scraping.Alura.Infra.Data\RPA.Scraping.Alura.Infra.Data.csproj migrations add Initial_Migration
```
Após rodar o comando acima, deve-se atualizar/criar o banco de dados conforme o migration acima, para isso, use:
```sh
dotnet ef --startup-project RPA.Scraping.Alura.Application\RPA.Scraping.Alura.Application.csproj --project RPA.Scraping.Alura.Infra.Data\RPA.Scraping.Alura.Infra.Data.csproj database update
```

basta iniciar a aplicaçõa e pesquisar um **curso** desejado.

Após finalizar os testes, deve-se para o **Docker** com o comando abaixo:
```sh
docker-compose down
```
