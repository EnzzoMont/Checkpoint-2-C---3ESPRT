<style>
    section {
      font-size: 26px; /* Substitua '24px' pelo tamanho desejado */
    }
</style>

# Aula 10 - Conexão com SQL Server
**Introdução a Bancos de Dados com ADO.NET**

---

## 📋 Agenda de Hoje

1. Introdução ao ADO.NET (10 min)
2. SQL Server em Docker (5 min)
3. Connection Strings (5 min)
4. Executando Comandos SQL (10 min)
5. Boas Práticas e Segurança (10 min)
6. **Laboratório Prático** (80 min)

---

## 🎯 O que vamos aprender?

- Configurar SQL Server localmente
- Conectar C# com banco de dados
- Executar operações CRUD
- Proteger contra SQL Injection
- Gerenciar recursos de conexão

---

## 📚 O que é ADO.NET?

**Active Data Objects .NET**

Conjunto de classes do .NET Framework para:
- Conectar a bancos de dados
- Executar comandos SQL
- Manipular resultados
- Gerenciar transações

```
C# Application → ADO.NET → SQL Server
```

---

## 🔧 Principais Classes

| Classe | Propósito |
|--------|-----------|
| `SqlConnection` | Gerencia conexão com BD |
| `SqlCommand` | Executa comandos SQL |
| `SqlDataReader` | Lê dados (forward-only) |
| `SqlParameter` | Parâmetros seguros |

---
<style scoped>
    section { font-size: 20px; } /* Applies to all elements on this slide */
</style>


## 🐳 O que é Docker?

**Definição:**
Plataforma para criar, distribuir e executar aplicações em **containers**

**Container:**
- Pacote leve e portátil
- Contém aplicação + dependências
- Isolado do sistema hospedeiro
- Compartilha o kernel do SO

```
┌─────────────────────────────────┐
│        Aplicação A              │
│  ┌──────────────────────────┐   │
│  │  SQL Server + Libs       │   │
│  └──────────────────────────┘   │
│         Container               │
└─────────────────────────────────┘
         Docker Engine
─────────────────────────────────
      Sistema Operacional
```

---

## 🔄 Como o Docker Funciona?

### Arquitetura Básica

```
┌──────────────────────────────────────┐
│         Docker Client                │
│    (docker run, docker build)        │
└────────────┬─────────────────────────┘
             │ API REST
             ▼
┌──────────────────────────────────────┐
│        Docker Daemon                 │
│  • Gerencia containers               │
│  • Gerencia imagens                  │
│  • Gerencia volumes                  │
└────────────┬─────────────────────────┘
             │
             ▼
┌──────────────────────────────────────┐
│         Containers                   │
│  [SQL] [Redis] [App] [Nginx]         │
└──────────────────────────────────────┘
```

---
<style scoped>
    section { font-size: 20px; } /* Applies to all elements on this slide */
</style>


## 📦 Imagens vs Containers

### Imagem Docker
- Template **somente leitura**
- Blueprint para criar containers
- Contém código + dependências
- Armazenada no Docker Hub

### Container
- **Instância em execução** de uma imagem
- Ambiente isolado e efêmero
- Pode ser iniciado/parado/deletado
- Múltiplos containers da mesma imagem

```
Imagem SQL Server
      │
      ├─→ Container 1 (Dev)
      ├─→ Container 2 (Test)
      └─→ Container 3 (QA)
```

---

## ✅ Vantagens do Docker

### 1. **Portabilidade**
- "Funciona na minha máquina" → Funciona em qualquer lugar
- Mesmo ambiente: Dev, Test, Prod

### 2. **Isolamento**
- Cada container é independente
- Sem conflitos de dependências
- Segurança adicional

### 3. **Leveza**
- Mais leve que VMs
- Inicia em segundos
- Menos recursos consumidos

---

## ✅ Vantagens do Docker (cont.)

### 4. **Versionamento**
- Imagens versionadas
- Rollback fácil
- Histórico completo

### 5. **Escalabilidade**
- Fácil criar múltiplas instâncias
- Load balancing simples
- Auto-scaling possível

### 6. **DevOps**
- CI/CD integrado
- Deploy automatizado
- Ambiente consistente

---

## ❌ Desvantagens do Docker

### 1. **Curva de Aprendizado**
- Conceitos novos (imagens, volumes, networks)
- Comandos específicos
- Debugging diferente

### 2. **Performance**
- Pequeno overhead comparado a bare metal
- I/O de disco pode ser mais lento
- Rede virtualizada

### 3. **Persistência de Dados**
- Containers são efêmeros
- Precisa configurar volumes
- Backup requer atenção especial

---

## ❌ Desvantagens do Docker (cont.)

### 4. **Segurança**
- Containers compartilham kernel
- Imagens podem ter vulnerabilidades
- Requer configuração adequada

### 5. **Windows**
- Performance inferior no Windows
- Requer WSL2 ou Hyper-V
- Nem todas imagens funcionam

### 6. **Complexidade em Produção**
- Orquestração necessária
- Monitoramento desafiador
- Gestão de segredos

---
<style scoped>
    section { font-size: 20px; } /* Applies to all elements on this slide */
</style>

## 🆚 Docker vs Máquinas Virtuais

```
┌─────────────────────┐  ┌─────────────────────┐
│   Virtual Machine   │  │      Docker         │
├─────────────────────┤  ├─────────────────────┤
│   App A │ App B     │  │  App A │ App B      │
│   Bins  │ Bins      │  │  Bins  │ Bins       │
│   Libs  │ Libs      │  │  Libs  │ Libs       │
├─────────┴───────────┤  ├────────┴────────────┤
│    Guest OS         │  │   Docker Engine     │
│    Guest OS         │  ├─────────────────────┤
├─────────────────────┤  │    Host OS          │
│    Hypervisor       │  └─────────────────────┘
├─────────────────────┤  
│    Host OS          │  ⚡ Mais leve
└─────────────────────┘  ⚡ Mais rápido
                         ⚡ Menos recursos
🐢 Pesado
🐢 Lento
🐢 Muito recurso
```

---
<style scoped>
    section { font-size: 20px; } /* Applies to all elements on this slide */
</style>

## 🎯 Docker para Desenvolvedores

**Casos de Uso Comuns:**

### Bancos de Dados
```bash
docker run -d postgres
docker run -d mysql
docker run -d mongodb
```

### Ferramentas de Dev
```bash
docker run -d redis
docker run -d rabbitmq
docker run -d elasticsearch
```

### Ambientes Completos
```bash
docker-compose up
# Sobe app + db + cache + queue
```

---
<style scoped>
    section { font-size: 20px; } /* Applies to all elements on this slide */
</style>


## 🐳 SQL Server em Docker

**Por que usar para este lab?**
- ✅ Ambiente isolado
- ✅ Setup rápido (1 comando)
- ✅ Fácil de limpar
- ✅ Sem conflitos com outras instalações
- ✅ Mesma versão para todos

```bash
docker run -e "ACCEPT_EULA=Y" \
  -e "MSSQL_SA_PASSWORD=SqlServer2024!" \
  -p 1433:1433 --name sqlserver2022 \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

**Comandos úteis:**
```bash
docker ps              # Ver containers rodando
docker stop sqlserver2022  # Parar
docker start sqlserver2022 # Iniciar
docker rm sqlserver2022    # Remover
```

---

## 🔌 Connection String

```csharp
string connectionString = 
    "Server=localhost,1433;" +
    "Database=LojaDB;" +
    "User Id=sa;" +
    "Password=SqlServer2024!;" +
    "TrustServerCertificate=True;";
```

**Componentes:**
- `Server`: Endereço + porta
- `Database`: Nome do BD
- `User Id` / `Password`: Credenciais
- `TrustServerCertificate`: Dev local

---

## 📝 Pattern Básico

```csharp
using (SqlConnection conn = new SqlConnection(connectionString))
{
    conn.Open();
    
    using (SqlCommand cmd = new SqlCommand(sql, conn))
    {
        // Executar comando
    }
} // Conexão fecha automaticamente
```

**Por que `using`?**
- Fecha conexão automaticamente
- Libera recursos
- Previne vazamentos

---

## 🎯 Tipos de Execução

### ExecuteNonQuery
Para `INSERT`, `UPDATE`, `DELETE`
```csharp
int linhasAfetadas = cmd.ExecuteNonQuery();
```

### ExecuteScalar
Para `SELECT` com valor único
```csharp
object resultado = cmd.ExecuteScalar();
```

### ExecuteReader
Para `SELECT` com múltiplas linhas
```csharp
SqlDataReader reader = cmd.ExecuteReader();
```

---

## 🔒 SQL Injection

### ❌ NUNCA faça isso:
```csharp
string sql = $"SELECT * FROM Clientes " +
             $"WHERE Nome = '{nome}'";
```

**Problema:** Se `nome = "'; DROP TABLE Clientes; --"`

---

## ✅ SEMPRE use parâmetros:

```csharp
string sql = "SELECT * FROM Clientes " +
             "WHERE Nome = @Nome";

cmd.Parameters.AddWithValue("@Nome", nome);
```

**Benefícios:**
- 🛡️ Previne SQL Injection
- ⚡ Melhor performance
- 📊 Tipos corretos

---

## 📋 Boas Práticas

1. ✅ **Sempre** use `using` para conexões
2. ✅ **Sempre** use parâmetros
3. ✅ **Sempre** trate exceções
4. ✅ Reutilize connection strings
5. ✅ Valide dados antes de inserir
6. ✅ Use transações para múltiplas operações
7. ✅ Não exponha erros ao usuário final

---

## 🏗️ Nosso Projeto

**Sistema de Gerenciamento de Loja**

Entidades:
- 👤 Cliente
- 📁 Categoria
- 📦 Produto
- 🛒 Pedido
- 📝 PedidoItem

---

## 💻 Laboratório - Parte 1

**Setup do Ambiente (15 min)**

1. Iniciar SQL Server no Docker
2. Conectar com Azure Data Studio
3. Criar database e tabelas
4. Inserir dados de teste

---

## 💻 Laboratório - Parte 2

**CRUD de Produtos (30 min)**

1. Listar todos os produtos
2. Inserir novo produto
3. Atualizar produto existente
4. Deletar produto

---

## 💻 Laboratório - Parte 3

**Operações Avançadas (35 min)**

1. Buscar produtos por categoria
2. Criar pedido com itens
3. Listar pedidos de um cliente
4. Calcular total do pedido

---

## 🎯 Desafios Extras

1. Implementar busca por nome (LIKE)
2. Adicionar paginação
3. Criar stored procedure
4. Implementar transação completa

---

## 📚 Recapitulando

✅ ADO.NET é a base para acesso a dados  
✅ Connection String configura a conexão  
✅ `using` garante limpeza de recursos  
✅ Parâmetros previnem SQL Injection  
✅ ExecuteReader para múltiplos registros  
✅ ExecuteNonQuery para comandos de escrita  

---

## ❓ Dúvidas?

**Recursos:**
- Documentação Microsoft: docs.microsoft.com
- SQL Server Express: Alternativa ao Docker
- Azure Data Studio: Cliente SQL moderno
- Connection String Reference: connectionstrings.com

---

## 🎉 Vamos ao Lab!

**Mãos à obra!**

1. Clone o projeto base
2. Configure seu ambiente
3. Complete os exercícios
4. Teste suas implementações

**Lembre-se:** Os comentários guiam vocês!