using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System;
// Adicionando o using System.Data para tipos SqlDbType, embora não seja estritamente necessário aqui
// Mas pode ser útil se quiser usar SqlDbType.
using System.Data;

namespace SistemaLoja.Lab12_ConexaoSQLServer
{
    public class PedidoRepository
    {
        // EXERCÍCIO 7: Criar pedido com itens (transação)
        public void CriarPedido(Pedido pedido, List<PedidoItem> itens)
        {
            // TODO: Implemente criação de pedido com transação
            // 1. Inserir Pedido
            // 2. Inserir cada PedidoItem
            // 3. Atualizar estoque dos produtos
            // IMPORTANTE: Use SqlTransaction!

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                // TODO: Inicie a transação
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // TODO: 1. Inserir pedido e obter ID
                    // ALTERAÇÃO: Parâmetros do SQL alterados para @Arg...
                    string sqlPedido = "INSERT INTO Pedidos (ClienteId, DataPedido, ValorTotal) " +
                                       "OUTPUT INSERTED.Id " +
                                       "VALUES (@ArgClienteId, @ArgDataPedido, @ArgValorTotal)";

                    int pedidoId = 0;
                    using (SqlCommand cmd = new SqlCommand(sqlPedido, conn, transaction))
                    {
                        // ALTERAÇÃO: Uso dos novos nomes de parâmetros
                        cmd.Parameters.AddWithValue("@ArgClienteId", pedido.ClienteId);
                        cmd.Parameters.AddWithValue("@ArgDataPedido", pedido.DataPedido);
                        cmd.Parameters.AddWithValue("@ArgValorTotal", pedido.ValorTotal);
                        pedidoId = (int)cmd.ExecuteScalar();
                    }

                    // 2. Inserir itens do pedido
                    // ALTERAÇÃO: Parâmetros do SQL alterados para @Arg...
                    string sqlItem = "INSERT INTO PedidoItens (PedidoId, ProdutoId, Quantidade, PrecoUnitario) " +
                                     "VALUES (@ArgPedidoId, @ArgProdutoId, @ArgQuantidade, @ArgPrecoUnitario)";
                    foreach (var item in itens)
                    {
                        using (SqlCommand cmdItem = new SqlCommand(sqlItem, conn, transaction))
                        {
                            // ALTERAÇÃO: Uso dos novos nomes de parâmetros
                            cmdItem.Parameters.AddWithValue("@ArgPedidoId", pedidoId);
                            cmdItem.Parameters.AddWithValue("@ArgProdutoId", item.ProdutoId);
                            cmdItem.Parameters.AddWithValue("@ArgQuantidade", item.Quantidade);
                            cmdItem.Parameters.AddWithValue("@ArgPrecoUnitario", item.PrecoUnitario);
                            cmdItem.ExecuteNonQuery();
                        }

                        // 3. Atualizar estoque dos produtos
                        // ALTERAÇÃO: Parâmetros do SQL alterados para @Arg...
                        string sqlUpdateEstoque = "UPDATE Produtos SET Estoque = Estoque - @ArgQuantidade WHERE Id = @ArgProdutoId";
                        using (SqlCommand cmdUpdate = new SqlCommand(sqlUpdateEstoque, conn, transaction))
                        {
                            // ALTERAÇÃO: Uso dos novos nomes de parâmetros
                            cmdUpdate.Parameters.AddWithValue("@ArgQuantidade", item.Quantidade);
                            cmdUpdate.Parameters.AddWithValue("@ArgProdutoId", item.ProdutoId);
                            cmdUpdate.ExecuteNonQuery();
                        }
                    }

                    // TODO: 3. Atualizar estoque

                    transaction.Commit();
                    Console.WriteLine("Pedido criado com sucesso!");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Erro ao criar pedido: {ex.Message}");
                    throw;
                }
            }
        }

        // EXERCÍCIO 8: Listar pedidos de um cliente
        public void ListarPedidosCliente(int clienteId)
        {
            // TODO: Liste todos os pedidos de um cliente
            // Mostre: Id, Data, ValorTotal

            string sql = "SELECT * FROM Pedidos WHERE ClienteId = @ClienteId ORDER BY DataPedido DESC";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ClienteId", clienteId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine($"\n--- PEDIDOS DO CLIENTE (ID: {clienteId}) ---");
                        while (reader.Read())
                        {
                            // ALTERAÇÃO: Leitura alterada para usar índice e GetType
                            int idPedido = reader.GetInt32(0);          // Coluna 0: Id
                            DateTime data = reader.GetDateTime(2);      // Coluna 2: DataPedido
                            decimal valorTotal = reader.GetDecimal(3);  // Coluna 3: ValorTotal

                            // ALTERAÇÃO: Usando as variáveis locais criadas
                            Console.WriteLine($"Id: {idPedido}, Data: {data:d}, Valor Total: {valorTotal:C}");
                        }
                        Console.WriteLine("--------------------------------------");
                    }
                }
            }
        }

        // EXERCÍCIO 9: Obter detalhes completos de um pedido
        public void ObterDetalhesPedido(int pedidoId)
        {
            // TODO: Mostre o pedido com todos os itens
            // Faça JOIN com Produtos para mostrar nomes

            string sql = @"SELECT 
                                pi.*, 
                                p.Nome as NomeProduto,
                                (pi.Quantidade * pi.PrecoUnitario) as Subtotal
                              FROM PedidoItens pi
                              INNER JOIN Produtos p ON pi.ProdutoId = p.Id
                              WHERE pi.PedidoId = @PedidoId";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                // Primeiro, obter os detalhes do pedido
                string sqlPedidoDetalhes = "SELECT Id, ClienteId, DataPedido, ValorTotal FROM Pedidos WHERE Id = @PedidoId";
                using (SqlCommand cmdPedido = new SqlCommand(sqlPedidoDetalhes, conn))
                {
                    cmdPedido.Parameters.AddWithValue("@PedidoId", pedidoId);
                    using (SqlDataReader readerPedido = cmdPedido.ExecuteReader())
                    {
                        if (readerPedido.Read())
                        {
                            Console.WriteLine($"\n=== DETALHES DO PEDIDO (ID: {readerPedido["Id"]}) ===");
                            Console.WriteLine($"Cliente ID: {readerPedido["ClienteId"]}");
                            Console.WriteLine($"Data do Pedido: {readerPedido["DataPedido"]:d}");
                            Console.WriteLine($"Valor Total: {readerPedido["ValorTotal"]:C}");
                            Console.WriteLine("--- ITENS DO PEDIDO ---");
                        }
                        else
                        {
                            Console.WriteLine($"Pedido com ID {pedidoId} não encontrado.");
                            return;
                        }
                    }
                }

                // Em seguida, obter os itens do pedido
                using (SqlCommand cmdItens = new SqlCommand(sql, conn))
                {
                    cmdItens.Parameters.AddWithValue("@PedidoId", pedidoId);

                    using (SqlDataReader readerItens = cmdItens.ExecuteReader())
                    {
                        while (readerItens.Read())
                        {
                            Console.WriteLine($"  Produto: {readerItens["NomeProduto"].ToString().PadRight(20)} | Qtd: {readerItens["Quantidade"].ToString().PadRight(4)} | Preço Unit.: {Convert.ToDecimal(readerItens["PrecoUnitario"]):C} | Subtotal: {Convert.ToDecimal(readerItens["Subtotal"]):C}");
                        }
                        Console.WriteLine("--------------------------------------");
                    }
                }
            }
        }

        // DESAFIO 3: Calcular total de vendas por período
        public void TotalVendasPorPeriodo(DateTime dataInicio, DateTime dataFim)
        {
            // TODO: Calcule o total de vendas em um período
            // Use ExecuteScalar para obter a soma
        }
    }
}