﻿using ControleEstoque.Web.Dal.Cadastro;
using ControleEstoque.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControleEstoque.Web.Controllers
{
    [Authorize(Roles = "Gerente, Administrativo, Operador, Desenvolvedor")]
    public class CadGrupoProdutoController : Controller
    {
        private const int _quantMaxLinhasPorPagina = 5;

        
        public ActionResult Index()
        {


            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);


            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = 1;

            var lista = GrupoProdutoDao.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);

            var quant = GrupoProdutoDao.RecuperarQuantidade();
            var difQuantPaginas = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;

            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + difQuantPaginas;
            return View(lista);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult GrupoProdutoPagina(int pagina, int tamPag, string filtro, string ordem)
        {
            var lista = GrupoProdutoDao.RecuperarLista(pagina, tamPag, filtro, ordem: ordem);
            return Json(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarGrupoProduto(int id)
        {
            return Json(GrupoProdutoDao.RecuperarPeloId(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirGrupoProduto(int id)
        {
            return Json(GrupoProdutoDao.ExcluirPeloId(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarGrupoProduto(GrupoProdutoModel model)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            if (!ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            }
            else
            {
                try
                {
                    var id = GrupoProdutoDao.Salvar(model);
                    if (id > 0)
                    {
                        idSalvo = id.ToString();
                    }
                    else
                    {
                        resultado = "ERRO";
                    }


                }
                catch (Exception ex)
                {
                    resultado = "ERRO";
                }
            }

            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }
        


    }
}