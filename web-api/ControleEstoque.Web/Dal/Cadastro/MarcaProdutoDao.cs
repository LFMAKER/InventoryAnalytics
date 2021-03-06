﻿using ControleEstoque.Web.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using VendasOsorioA.DAL;

namespace ControleEstoque.Web.Dal.Cadastro
{
    public class MarcaProdutoDao
    {
        private static Context ctx = SingletonContext.GetInstance();

        public static int RecuperarQuantidade()
        {
            var ret = 0;

            ret = ctx.MarcasProdutos.AsNoTracking().Count();
            return ret;
        }

        public static List<MarcaProduto> RecuperarLista(int pagina = 0, int tamPagina = 0, string filtro = "", bool somenteAtivos = false)
        {

            var ret = new List<MarcaProduto>();

            using (var ctx = new Context())
            {
                if (tamPagina != 0 && pagina != 0)
                {
                    var pos = (pagina - 1) * tamPagina;
                    if (!string.IsNullOrEmpty(filtro))
                    {

                        ret = ctx.MarcasProdutos.AsNoTracking().OrderBy(x => x.Nome).Where(x => x.Nome.ToLower().Contains(filtro.ToLower())).Skip(pos > 0 ? pos - 1 : 0).Take(tamPagina).ToList();
                    }
                    else
                    {

                        ret = ctx.MarcasProdutos.AsNoTracking().OrderBy(x => x.Nome).Skip(pos > 0 ? pos - 1 : 0).Take(tamPagina).ToList();
                    }
                }else if (somenteAtivos)
                {
                    ret = ctx.MarcasProdutos.AsNoTracking().OrderBy(x => x.Nome).Where(x => x.Ativo == true).ToList();
                }else
                {
                    ret = ctx.MarcasProdutos.AsNoTracking().OrderBy(x => x.Nome).ToList();
                }



            }

            return ret;
        }

        public static MarcaProduto RecuperarPeloId(int? id)
        {
            if (id != null)
            {
                return ctx.MarcasProdutos.AsNoTracking().Where(x => x.Id == id).FirstOrDefault();
            }
            else
            {
                return null;
            }


        }

        public static bool ExcluirPeloId(int id)
        {
            var ret = false;
            var existing = ctx.MarcasProdutos.FirstOrDefault(x => x.Id == id);
            if (existing != null)
            {
                ctx.Entry(existing).State = EntityState.Detached;
            }

            try
            {
                ctx.MarcasProdutos.Attach(existing);
                ctx.Entry(existing).State = EntityState.Deleted;
                ctx.SaveChanges();
                ret = true;
            }
            catch (DbUpdateException)
            {
                ret = false;
            }
            catch (InvalidOperationException)
            {
                ret = false;
            }
            //Limpando qualquer Exception que tenha ficado gravado no Object do Entity
            //Se não limpar, caso ocorra uma excessão na exclusão, ele sempre vai ficar persistindo 
            //o erro, mesmo que o proximo objeto esteja sem nenhum problema.
            ctx.DetachAllEntities();
            return ret;
        }

        public static int Salvar(MarcaProduto mp)
        {
            var ret = 0;
            var model = RecuperarPeloId(mp.Id);
            if (model == null)
            {
                Cadastrar(mp);
            }
            else
            {
                Alterar(mp);
            }
            ret = mp.Id;
            return ret;
        }

        public static bool Cadastrar(MarcaProduto mp)
        {
            ctx.MarcasProdutos.Add(mp);
            ctx.SaveChanges();
            return true;
        }
        public static bool Alterar(MarcaProduto mp)
        {
            var existing = ctx.MarcasProdutos.FirstOrDefault(x => x.Id == mp.Id);
            if (existing != null)
            {
                ctx.Entry(existing).State = EntityState.Detached;
            }

            try
            {
                ctx.MarcasProdutos.Attach(mp);
                ctx.Entry(mp).State = EntityState.Modified;
                ctx.SaveChanges();
            }
            #pragma warning disable 0168
            catch (System.Exception ex)
            {
                return false;
            }
            return true;
        }


        public static bool VerificarNome(MarcaProduto p)
        {
            var result = ctx.MarcasProdutos.FirstOrDefault(x => x.Nome.Equals(p.Nome));
            if (result == null)
            {
                return false;
            }

            return true;
        }


        public static bool VerificarNomeEId(MarcaProduto p)
        {
            var result = ctx.MarcasProdutos.FirstOrDefault(x => x.Nome.Equals(p.Nome) && x.Id == p.Id);
            if (result == null)
            {
                return false;
            }

            return true;
        }



    }
}