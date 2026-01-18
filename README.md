#  Biblioteca API

Sistema de controle de biblioteca desenvolvido em **C# .NET 8**, permitindo o cadastro de usuários, livros e empréstimos, além do controle de devoluções e cálculo de multas.

---

##  Como Executar o Projeto

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Execução Local

1. Clone o repositório:
```bash
git clone https://github.com/Everton-Buenno/BibliotecaApi.git
cd BibliotecaApi
```

2. Restaure as dependências:
```bash
dotnet restore
```

3. Execute a aplicação:
```bash
dotnet run
```

4. Acesse o Swagger para testar a API:
```
https://localhost:7208/swagger
```
ou
```
http://localhost:5023/swagger
```


##  Desafios Realizados

| # | Tipo | Desafio | Status |
|---|------|---------|--------|
| 1 | FEATURE | Validar CPF do usuário (impedir duplicados) |  Implementado |
| 2 | FEATURE | Validar ISBN do livro (13 dígitos numéricos) |  Implementado |
| 3 | BUG | Multa gerada apenas quando devolução após data prevista |  Corrigido |
| 4 | BUG | Impedir empréstimo de livro já emprestado |  Corrigido |
| 5 | FEATURE | Endpoint GET /livro/listar |  Implementado |
| 6 | FEATURE | Impedir empréstimo se usuário tiver atraso |  Implementado |
| 7 | FEATURE | Nova regra de multa (escalonada com limite R$50) |  Implementado |
| 8 | FEATURE | Autenticação Bearer Token (JWT) |  Implementado |

---

##  Autenticação JWT

A API utiliza autenticação **JWT Bearer Token** para proteger endpoints sensíveis.

### Como autenticar:

1. **Faça login** no endpoint `/Auth/login`:
```json
POST /Auth/login
{
    "username": "admin",
    "password": "biblioteca123"
}
```

2. **Resposta** com o token:
```json
{
    "sucesso": true,
    "conteudo": {
        "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
        "expiresAt": "2025-01-17T22:00:00Z"
    }
}
```

3. **Use o token** no header das requisições protegidas:
```
Authorization: Bearer {seu_token}
```

4. **No Swagger**: Clique em "Authorize"  e insira:
```
Bearer {seu_token}
```

---

##  Endpoints da API

###  Autenticação
| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| POST | `/Auth/login` | Login e geração de token JWT | Não |

###  Usuários
| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| POST | `/Usuario/cadastrar` | Cadastra novo usuário | Não |

###  Livros
| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| POST | `/Livro/cadastrar` | Cadastra novo livro | Sim |
| GET | `/Livro/listar` | Lista todos os livros | Não |

###  Empréstimos
| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| POST | `/Emprestimo/cadastrar` | Registra novo empréstimo | Não |
| POST | `/Emprestimo/devolver` | Registra devolução | Sim |

---

##  Regras de Multa (Desafio 7)

A multa é calculada **apenas** quando a devolução ocorre **após** a data prevista:

| Dias de Atraso | Valor por Dia |
|----------------|---------------|
| 1 a 3 dias | R$ 2,00 |
| 4+ dias | R$ 3,50 |
| **Limite máximo** | **R$ 50,00** |

### Exemplos:
| Dias | Cálculo | Multa |
|------|---------|-------|
| 2 dias | 2 × R$ 2,00 | R$ 4,00 |
| 5 dias | (3 × R$ 2,00) + (2 × R$ 3,50) | R$ 13,00 |
| 20 dias | Calculado, mas limitado | R$ 50,00 |

---

##  Estrutura do Projeto

```
BibliotecaApi/
├── Application/
│   └── Api/
│       ├── Auth/               # JWT Authentication
│       │   ├── JwtSettings.cs
│       │   ├── JwtTokenService.cs
│       │   └── DTO/
│       ├── Controllers/        # API Controllers
│       │   ├── AuthController.cs
│       │   ├── EmprestimoController.cs
│       │   ├── LivroController.cs
│       │   └── UsuarioController.cs
│       └── Responses/          # Response patterns
├── Domain/
│   └── Entities/               # Domain entities
│       ├── EmprestimoEntity.cs
│       ├── LivroEntity.cs
│       └── UsuarioEntity.cs
├── Infrastructure/
│   ├── Data/                   # Database config
│   ├── IOC/                    # Dependency Injection
│   └── Repositories/           # Data access
├── UseCases/
│   ├── Emprestimo/             # Loan use cases
│   ├── Livro/                  # Book use cases
│   └── Usuario/                # User use cases
├── appsettings.json
├── Program.cs
└── README.md
```

---

##  Tecnologias Utilizadas

- **.NET 8** - Framework principal
- **SQLite** - Banco de dados
- **Dapper** - Micro ORM
- **JWT Bearer** - Autenticação
- **Swagger/OpenAPI** - Documentação da API

---

##  Exemplos de Uso

### Cadastrar Usuário
```bash
curl -X POST "https://localhost:5001/Usuario/cadastrar" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "João Silva",
    "cpf": "12345678901",
    "email": "joao@email.com"
  }'
```

**Resposta (sucesso):**
```json
{
  "sucesso": true,
  "conteudo": 1,
  "mensagemErro": null
}
```

**Resposta (CPF duplicado - Desafio 1):**
```json
{
  "sucesso": false,
  "conteudo": null,
  "mensagemErro": "Usuário com este CPF já está cadastrado."
}
```

### Cadastrar Livro (requer autenticação)
```bash
curl -X POST "https://localhost:5001/Livro/cadastrar" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {token}" \
  -d '{
    "titulo": "Clean Code",
    "autor": "Robert C. Martin",
    "isbn": "9780132350884"
  }'
```

**Resposta (ISBN inválido - Desafio 2):**
```json
{
  "sucesso": false,
  "conteudo": null,
  "mensagemErro": "ISBN deve conter exatamente 13 dígitos numéricos."
}
```

### Registrar Empréstimo (requer autenticação)
```bash
curl -X POST "https://localhost:5001/Emprestimo/cadastrar" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {token}" \
  -d '{
    "idUsuario": 1,
    "idLivro": 1,
    "dataPrevistaDevolucao": "2025-02-01"
  }'
```

**Resposta (livro já emprestado - Desafio 4):**
```json
{
  "sucesso": false,
  "conteudo": null,
  "mensagemErro": "Este livro já está emprestado e ainda não foi devolvido."
}
```

**Resposta (usuário com atraso - Desafio 6):**
```json
{
  "sucesso": false,
  "conteudo": null,
  "mensagemErro": "Usuário com empréstimo em atraso não pode realizar novo empréstimo."
}
```

### Devolver Empréstimo (requer autenticação)
```bash
curl -X POST "https://localhost:5001/Emprestimo/devolver" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {token}" \
  -d '{
    "idEmprestimo": 1
  }'
```

**Resposta (com multa - Desafios 3 e 7):**
```json
{
  "sucesso": true,
  "conteudo": "Empréstimo devolvido com sucesso. Multa: R$13,00, Total a pagar: R$18,00",
  "mensagemErro": null
}
```

---

##  Autor

**Everton Bueno**

---

##  Licença

Este projeto foi desenvolvido como parte de um desafio técnico.
