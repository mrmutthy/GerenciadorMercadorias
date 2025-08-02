# Gerenciador de Mercadorias

## Como executar o projeto

### 1. Instale o .NET 9.0

Baixe e instale o SDK do .NET 9.0 pelo site oficial:
https://dotnet.microsoft.com/download/dotnet/9.0

### 2. Restaure os pacotes e compile a solução

Abra o terminal na raiz do projeto e execute:

```powershell
dotnet restore
dotnet build
```

### 3. Execute a aplicação

No terminal, navegue até a pasta `GerenciadorMercadorias.UI` e rode:

```powershell
dotnet run
```

Pronto! A interface gráfica será aberta para uso.

## Descrição do Projeto
Sistema de controle de esque de mercadorias com operações CRUD(Create, Read, Update e Delete) para teste tecnico de conhecimento C# para Nootech.

Estrutura Utilizada:

- **Model**: Contém as classes que representam as entidades do domínio, como Mercadoria.
- **DAL**: Responsável pelo acesso e manipulação dos dados.
- **UI**: Interface gráfica da aplicação, desenvolvida em WPF.