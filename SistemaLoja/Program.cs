

using System;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using SistemaLoja.Lab12_ConexaoSQLServer;

namespace SistemaLoja
{
    // ===============================================
    // MODELOS DE DADOS
    // ===============================================

    // ===============================================
    // CLASSE DE CONEXÃO
    // ===============================================

    // ===============================================
    // REPOSITÓRIO DE PRODUTOS
    // ===============================================

    // ===============================================
    // REPOSITÓRIO DE PEDIDOS
    // ===============================================

    // ===============================================
    // CLASSE PRINCIPAL
    // ===============================================
    
    class Program
    {
        static void Main(string[] args)
        {
            // IMPORTANTE: Antes de executar, crie o banco de dados!
            // Execute o script SQL fornecido no arquivo setup.sql
            
            Console.WriteLine("=== LAB 12 - CONEXÃO SQL SERVER ===\n");
            
            var produtoRepo = new ProdutoRepository();
            var pedidoRepo = new PedidoRepository();
            
            bool continuar = true;
            
            while (continuar)
            {
                MostrarMenu();
                string opcao = Console.ReadLine();
                
                try
                {
                    switch (opcao)
                    {
                        case "1":
                            produtoRepo.ListarTodosProdutos();
                            break;
                            
                        case "2":
                            InserirNovoProduto(produtoRepo);
                            break;
                            
                        case "3":
                            AtualizarProdutoExistente(produtoRepo);
                            break;
                            
                        case "4":
                            DeletarProdutoExistente(produtoRepo);
                            break;
                            
                        case "5":
                            ListarPorCategoria(produtoRepo);
                            break;
                            
                        case "6":
                            CriarNovoPedido(pedidoRepo);
                            break;
                            
                        case "7":
                            ListarPedidosDeCliente(pedidoRepo);
                            break;
                            
                        case "8":
                            DetalhesDoPedido(pedidoRepo);
                            break;
                            
                        case "0":
                            continuar = false;
                            break;
                            
                        default:
                            Console.WriteLine("Opção inválida!");
                            break;
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"\n❌ Erro SQL: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n❌ Erro: {ex.Message}");
                }
                
                if (continuar)
                {
                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            
            Console.WriteLine("\nPrograma finalizado!");
        }

        static void MostrarMenu()
        {
            Console.WriteLine("\n╔════════════════════════════════════╗");
            Console.WriteLine("║       MENU PRINCIPAL               ║");
            Console.WriteLine("╠════════════════════════════════════╣");
            Console.WriteLine("║  PRODUTOS                          ║");
            Console.WriteLine("║  1 - Listar todos os produtos      ║");
            Console.WriteLine("║  2 - Inserir novo produto          ║");
            Console.WriteLine("║  3 - Atualizar produto             ║");
            Console.WriteLine("║  4 - Deletar produto               ║");
            Console.WriteLine("║  5 - Listar por categoria          ║");
            Console.WriteLine("║                                    ║");
            Console.WriteLine("║  PEDIDOS                           ║");
            Console.WriteLine("║  6 - Criar novo pedido             ║");
            Console.WriteLine("║  7 - Listar pedidos de cliente     ║");
            Console.WriteLine("║  8 - Detalhes de um pedido         ║");
            Console.WriteLine("║                                    ║");
            Console.WriteLine("║  0 - Sair                          ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            Console.Write("\nEscolha uma opção: ");
        }

        // TODO: Implemente os métodos auxiliares abaixo
        
        static void InserirNovoProduto(ProdutoRepository repo)
        {
            Console.WriteLine("\n=== INSERIR NOVO PRODUTO ===");
            
            // TODO: Solicite os dados do produto ao usuário
            Console.Write("Nome: ");
            string nome = Console.ReadLine();
            
            Console.Write("Preço: ");
            decimal preco = decimal.Parse(Console.ReadLine());
            Console.Write("Estoque: ");
            int estoque = int.Parse(Console.ReadLine());
            Console.Write("ID da Categoria: ");
            int categoriaId = int.Parse(Console.ReadLine());
            
            var produto = new Produto
            {
                Nome = nome,
                Preco = preco,
                Estoque = estoque,
                CategoriaId = categoriaId
            };
            
            repo.InserirProduto(produto);
        }

        static void AtualizarProdutoExistente(ProdutoRepository repo)
        {
            // TODO: Implemente a atualização
            Console.WriteLine("\n=== ATUALIZAR PRODUTO ===");
            
            Console.Write("ID do produto: ");
            int id = int.Parse(Console.ReadLine());
            
            Produto produtoExistente = repo.BuscarPorId(id);
            if (produtoExistente == null)
            {
                Console.WriteLine($"Produto com ID {id} não encontrado.");
                return;
            }

            Console.WriteLine($"Produto encontrado: {produtoExistente.Nome}");
            Console.Write($"Novo Nome ({produtoExistente.Nome}): ");
            string novoNome = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(novoNome)) produtoExistente.Nome = novoNome;

            Console.Write($"Novo Preço ({produtoExistente.Preco}): ");
            string novoPrecoStr = Console.ReadLine();
            if (decimal.TryParse(novoPrecoStr, out decimal novoPreco)) produtoExistente.Preco = novoPreco;

            Console.Write($"Novo Estoque ({produtoExistente.Estoque}): ");
            string novoEstoqueStr = Console.ReadLine();
            if (int.TryParse(novoEstoqueStr, out int novoEstoque)) produtoExistente.Estoque = novoEstoque;

            Console.Write($"Nova Categoria ID ({produtoExistente.CategoriaId}): ");
            string novaCategoriaIdStr = Console.ReadLine();
            if (int.TryParse(novaCategoriaIdStr, out int novaCategoriaId)) produtoExistente.CategoriaId = novaCategoriaId;

            repo.AtualizarProduto(produtoExistente);
        }

        static void DeletarProdutoExistente(ProdutoRepository repo)
        {
            // TODO: Implemente a exclusão
            Console.WriteLine("\n=== DELETAR PRODUTO ===");
            
            Console.Write("ID do produto: ");
            int id = int.Parse(Console.ReadLine());
            
            Console.Write("Tem certeza que deseja deletar este produto? (s/n): ");
            string confirmacao = Console.ReadLine();
            if (confirmacao.ToLower() == "s")
            {
                repo.DeletarProduto(id);
            }
            else
            {
                Console.WriteLine("Operação de exclusão cancelada.");
            }
        }

        static void ListarPorCategoria(ProdutoRepository repo)
        {
            Console.WriteLine("\n=== PRODUTOS POR CATEGORIA ===");
            Console.Write("ID da Categoria: ");
            int categoriaId = int.Parse(Console.ReadLine());
            repo.ListarProdutosPorCategoria(categoriaId);
        }

        static void CriarNovoPedido(PedidoRepository repo)
        {
            Console.WriteLine("\n=== CRIAR NOVO PEDIDO ===");

            Console.Write("ID do Cliente: ");
            int clienteId = int.Parse(Console.ReadLine());

            var itens = new List<PedidoItem>();
            decimal valorTotal = 0;

            while (true)
            {
                Console.Write("ID do Produto (ou 'fim' para encerrar): ");
                string produtoIdStr = Console.ReadLine();
                if (produtoIdStr.ToLower() == "fim") break;

                int produtoId = int.Parse(produtoIdStr);

                Console.Write("Quantidade: ");
                int quantidade = int.Parse(Console.ReadLine());

                // Para obter o preço, precisaríamos de uma instância do ProdutoRepository aqui
                // ou buscar o produto dentro do PedidoRepository.
                // Por simplicidade, vamos assumir um preço fixo ou buscar no banco.
                // A melhor abordagem seria ter um método no ProdutoRepository para buscar o preço.
                Console.Write("Preço Unitário: ");
                decimal precoUnitario = decimal.Parse(Console.ReadLine());

                itens.Add(new PedidoItem
                {
                    ProdutoId = produtoId,
                    Quantidade = quantidade,
                    PrecoUnitario = precoUnitario
                });

                valorTotal += quantidade * precoUnitario;
            }

            if (itens.Count > 0)
            {
                var novoPedido = new Pedido
                {
                    ClienteId = clienteId,
                    DataPedido = DateTime.Now,
                    ValorTotal = valorTotal
                };

                repo.CriarPedido(novoPedido, itens);
            }
            else
            {
                Console.WriteLine("Nenhum item adicionado ao pedido.");
            }
        }

        static void ListarPedidosDeCliente(PedidoRepository repo)
        {
            Console.WriteLine("\n=== PEDIDOS DO CLIENTE ===");
            Console.Write("ID do Cliente: ");
            int clienteId = int.Parse(Console.ReadLine());
            repo.ListarPedidosCliente(clienteId);
        }

        static void DetalhesDoPedido(PedidoRepository repo)
        {
            Console.WriteLine("\n=== DETALHES DO PEDIDO ===");
            Console.Write("ID do Pedido: ");
            int pedidoId = int.Parse(Console.ReadLine());
            repo.ObterDetalhesPedido(pedidoId);
        }
    }
}