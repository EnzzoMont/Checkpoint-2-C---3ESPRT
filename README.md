# 🏆 Checkpoint 2 - Conexão com SQL Server (ADO.NET)

Este repositório contém a solução do Laboratório 12, que foca na persistência de dados utilizando C# (ADO.NET) e SQL Server via Docker.

## 👥 Membros da Equipe

| Nome do Aluno | RM |
| :--- | :--- |
| **Enzzo Monteiro** | **RM552616** |
| **Pedro Henrique Nardaci** | **RM553988** |

## 🛠️ Requisitos e Setup Rápido

O projeto utiliza um container Docker para o banco de dados.

### 1. Iniciar o Servidor SQL

Execute este comando no terminal para iniciar o container:

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=SqlServer2024!" \
-p 1433:1433 --name sqlserver2022 -d \
[mcr.microsoft.com/mssql/server:2022-latest](https://mcr.microsoft.com/mssql/server:2022-latest)
