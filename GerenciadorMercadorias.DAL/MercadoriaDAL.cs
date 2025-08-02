using Microsoft.Data.Sqlite;
using GerenciadorMercadorias.Model;
using System.Collections.Generic;
using System.IO;
using System;

namespace GerenciadorMercadorias.DAL
{
    public class MercadoriaDAL
    {
        private readonly string _connectionString;

        public MercadoriaDAL(string connectionString)
        {
            _connectionString = connectionString;
            CreateTable();
        }

        private void CreateTable()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Mercadorias (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Nome TEXT NOT NULL,
                        Descricao TEXT,
                        Preco REAL NOT NULL,
                        Estoque INTEGER NOT NULL
                    );";
                command.ExecuteNonQuery();
            }
        }

        public void AdicionarMercadoria(Mercadoria mercadoria)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    "INSERT INTO Mercadorias (Nome, Descricao, Preco, Estoque) VALUES (@Nome, @Descricao, @Preco, @Estoque)";

                command.Parameters.AddWithValue("@Nome", mercadoria.Nome);
                command.Parameters.AddWithValue("@Descricao", (object?)mercadoria.Descricao ?? DBNull.Value);
                command.Parameters.AddWithValue("@Preco", mercadoria.Preco);
                command.Parameters.AddWithValue("@Estoque", mercadoria.Estoque);

                command.ExecuteNonQuery();
            }
        }

        public List<Mercadoria> ListarMercadorias()
        {
            var mercadorias = new List<Mercadoria>();
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Nome, Descricao, Preco, Estoque FROM Mercadorias";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        mercadorias.Add(new Mercadoria
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Nome = reader.GetString(reader.GetOrdinal("Nome")),
                            Descricao = reader.IsDBNull(reader.GetOrdinal("Descricao")) ? null : reader.GetString(reader.GetOrdinal("Descricao")),
                            Preco = reader.GetDecimal(reader.GetOrdinal("Preco")),
                            Estoque = reader.GetInt32(reader.GetOrdinal("Estoque"))
                        });
                    }
                }
            }
            return mercadorias;
        }

        public void AtualizarMercadoria(Mercadoria mercadoria)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    "UPDATE Mercadorias SET Nome = @Nome, Descricao = @Descricao, Preco = @Preco, Estoque = @Estoque WHERE Id = @Id";

                command.Parameters.AddWithValue("@Nome", mercadoria.Nome);
                command.Parameters.AddWithValue("@Descricao", (object?)mercadoria.Descricao ?? DBNull.Value);
                command.Parameters.AddWithValue("@Preco", mercadoria.Preco);
                command.Parameters.AddWithValue("@Estoque", mercadoria.Estoque);
                command.Parameters.AddWithValue("@Id", mercadoria.Id);

                command.ExecuteNonQuery();
            }
        }

        public void ExcluirMercadoria(int id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Mercadorias WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }

        public Mercadoria? ObterMercadoriaPorId(int id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Nome, Descricao, Preco, Estoque FROM Mercadorias WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Mercadoria
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Nome = reader.GetString(reader.GetOrdinal("Nome")),
                            Descricao = reader.IsDBNull(reader.GetOrdinal("Descricao")) ? null : reader.GetString(reader.GetOrdinal("Descricao")),
                            Preco = reader.GetDecimal(reader.GetOrdinal("Preco")),
                            Estoque = reader.GetInt32(reader.GetOrdinal("Estoque"))
                        };
                    }
                }
            }
            return null;
        }
    }
}