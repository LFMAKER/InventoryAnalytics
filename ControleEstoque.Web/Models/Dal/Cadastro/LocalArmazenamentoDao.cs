﻿using ControleEstoque.Web.Models;
using Dapper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace ControleEstoque.Web.Dal.Cadastro
{
    public class LocalArmazenamentoDao
    {

        public static int RecuperarQuantidade()
        {
            var ret = 0;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                ret = conexao.ExecuteScalar<int>("select count(*) from local_armazenamento");
            }

            return ret;
        }

        public static List<LocalArmazenamentoModel> RecuperarLista(int pagina, int tamPagina, string ordem = "")
        {
            var ret = new List<LocalArmazenamentoModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();

                var pos = (pagina - 1) * tamPagina;

                var sql = string.Format(
                        "select *" +
                        " from local_armazenamento" +
                        " order by " + (!string.IsNullOrEmpty(ordem) ? ordem : "nome") +
                        " offset {0} rows fetch next {1} rows only",
                        pos > 0 ? pos - 1 : 0, tamPagina);

                ret = conexao.Query<LocalArmazenamentoModel>(sql).ToList();
            }

            return ret;
        }

        public static LocalArmazenamentoModel RecuperarPeloId(int id)
        {
            LocalArmazenamentoModel ret = null;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();

                var sql = "select * from local_armazenamento where (id = @id)";
                var parametros = new { id };
                ret = conexao.Query<LocalArmazenamentoModel>(sql, parametros).SingleOrDefault();
            }

            return ret;
        }

        public static bool ExcluirPeloId(int id)
        {
            var ret = false;

            if (RecuperarPeloId(id) != null)
            {
                using (var conexao = new SqlConnection())
                {
                    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                    conexao.Open();

                    var sql = "delete from local_armazenamento where (id = @id)";
                    var parametros = new { id };
                    ret = (conexao.Execute(sql, parametros) > 0);
                }
            }

            return ret;
        }

        public static int Salvar(LocalArmazenamentoModel la)
        {
            var ret = 0;

            var model = RecuperarPeloId(la.Id);

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();

                if (model == null)
                {
                    var sql = "insert into local_armazenamento (nome, ativo) values (@nome, @ativo); select convert(int, scope_identity())";
                    var parametros = new { nome = la.Nome, ativo = (la.Ativo ? 1 : 0) };
                    ret = conexao.ExecuteScalar<int>(sql, parametros);
                }
                else
                {
                    var sql = "update local_armazenamento set nome=@nome, ativo=@ativo where id = @id";
                    var parametros = new { id = la.Id, nome = la.Nome, ativo = (la.Ativo ? 1 : 0) };
                    if (conexao.Execute(sql, parametros) > 0)
                    {
                        ret = la.Id;
                    }
                }
            }

            return ret;
        }

    }
}