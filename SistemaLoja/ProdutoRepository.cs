using Microsoft.Data.SqlClient;

namespace SistemaLoja.Lab12_ConexaoSQLServer;

public class ProdutoRepository
{
    // EXERCÍCIO 1: Listar todos os produtos
    public void ListarTodosProdutos()
    {
        // TODO: Implemente a listagem de produtos
        // Dica: Use ExecuteReader e while(reader.Read())
            
        string sql = "SELECT * FROM Produtos";
            
        using (SqlConnection conn = DatabaseConnection.GetConnection())
        {
            conn.Open();
                
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("\n--- PRODUTOS ---");
                    while (reader.Read())
                    {
                        Console.WriteLine($"Id: {reader["Id"]}, Nome: {reader["Nome"]}, Preço: {reader["Preco"]:C}, Estoque: {reader["Estoque"]}");
                    }
                    Console.WriteLine("----------------");
                }
            }
        }
    }

    // EXERCÍCIO 2: Inserir novo produto
    public void InserirProduto(Produto produto)
    {
        // TODO: Implemente a inserção de produto
        // IMPORTANTE: Use parâmetros para evitar SQL Injection!
            
        string sql = "INSERT INTO Produtos (Nome, Preco, Estoque, CategoriaId) " +
                     "VALUES (@Nome, @Preco, @Estoque, @CategoriaId)";
            
        using (SqlConnection conn = DatabaseConnection.GetConnection())
        {
            conn.Open();
                
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                // TODO: Adicione os parâmetros
                cmd.Parameters.AddWithValue("@Nome", produto.Nome);
                cmd.Parameters.AddWithValue("@Preco", produto.Preco);
                cmd.Parameters.AddWithValue("@Estoque", produto.Estoque);
                cmd.Parameters.AddWithValue("@CategoriaId", produto.CategoriaId);
                    
                cmd.ExecuteNonQuery();
                    
                Console.WriteLine("Produto inserido com sucesso!");
            }
        }
    }

    // EXERCÍCIO 3: Atualizar produto
    public void AtualizarProduto(Produto produto)
    {
        // TODO: Implemente a atualização de produto
        // Dica: UPDATE Produtos SET ... WHERE Id = @Id
            
        string sql = "UPDATE Produtos SET " +
                     "Nome = @Nome, " +
                     "Preco = @Preco, " +
                     "Estoque = @Estoque " +
                     "WHERE Id = @Id";
            
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Nome", produto.Nome);
                    cmd.Parameters.AddWithValue("@Preco", produto.Preco);
                    cmd.Parameters.AddWithValue("@Estoque", produto.Estoque);
                    cmd.Parameters.AddWithValue("@Id", produto.Id);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Produto {produto.Id} atualizado com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine($"Produto {produto.Id} não encontrado.");
                    }
                }
            }
    }

    // EXERCÍCIO 4: Deletar produto
    public void DeletarProduto(int id)
    {
        // TODO: Implemente a exclusão de produto
        // ATENÇÃO: Verifique se não há pedidos vinculados!
            
        string sql = "DELETE FROM Produtos WHERE Id = @Id";
            
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Produto {id} deletado com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine($"Produto {id} não encontrado.");
                    }
                }
            }
    }

    // EXERCÍCIO 5: Buscar produto por ID
    public Produto BuscarPorId(int id)
    {
        // TODO: Implemente a busca por ID
        // Retorne um objeto Produto ou null se não encontrar
            
        string sql = "SELECT * FROM Produtos WHERE Id = @Id";
        Produto produto = null;
            
        using (SqlConnection conn = DatabaseConnection.GetConnection())
        {
            conn.Open();
                
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                    
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        produto = new Produto
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nome = reader["Nome"].ToString(),
                            Preco = Convert.ToDecimal(reader["Preco"]),
                            Estoque = Convert.ToInt32(reader["Estoque"]),
                            CategoriaId = Convert.ToInt32(reader["CategoriaId"])
                        };
                    }
                }
            }
        }
            
        return produto;
    }

    // EXERCÍCIO 6: Listar produtos por categoria
    public void ListarProdutosPorCategoria(int categoriaId)
    {
        // TODO: Implemente a listagem filtrada por categoria
        // Dica: Faça um JOIN com a tabela Categorias para mostrar o nome
            
        string sql = @"SELECT p.*, c.Nome as NomeCategoria 
                          FROM Produtos p
                          INNER JOIN Categorias c ON p.CategoriaId = c.Id
                          WHERE p.CategoriaId = @CategoriaId";
            
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@CategoriaId", categoriaId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine($"\n--- PRODUTOS NA CATEGORIA (ID: {categoriaId}) ---");
                        while (reader.Read())
                        {
                            Console.WriteLine($"Id: {reader["Id"]}, Nome: {reader["Nome"]}, Preço: {reader["Preco"]:C}, Estoque: {reader["Estoque"]}, Categoria: {reader["NomeCategoria"]}");
                        }
                        Console.WriteLine("------------------------------------------");
                    }
                }
            }
    }

    // DESAFIO 1: Buscar produtos com estoque baixo
    public void ListarProdutosEstoqueBaixo(int quantidadeMinima)
    {
        // TODO: Liste produtos com estoque menor que quantidadeMinima
        // Mostre um alerta visual para chamar atenção
    }

    // DESAFIO 2: Buscar produtos por nome (LIKE)
    public void BuscarProdutosPorNome(string termoBusca)
    {
        // TODO: Implemente busca com LIKE
        // Dica: Use '%' + termoBusca + '%'
    }
}