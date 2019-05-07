﻿using ControleEstoque.Web.Dal.Cadastro;
using ControleEstoque.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ControleEstoque.Web.Controllers.Cadastro
{
    
    public class CadUsuarioController : Controller
    {


        private const string _senhaPadrao = "{$127;$188}";
        private const int _quantMaxLinhasPorPagina = 5;
        [Authorize(Roles = "Gerente, Desenvolvedor")]
        public ActionResult Index()
        {

            
            ViewBag.SenhaPadrao = _senhaPadrao;
           
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);


            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = 1;

            var lista = UsuarioDao.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);

            var quant = UsuarioDao.RecuperarQuantidade();
            var difQuantPaginas = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;

            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + difQuantPaginas;
            return View(lista);



        }


        [HttpPost]
        [Authorize(Roles = "Gerente, Desenvolvedor")]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperarUsuario(int id)
        {
            return Json(UsuarioDao.RecuperarPeloId(id));
        }
        [Authorize(Roles = "Gerente, Desenvolvedor, Analista")]
        public ActionResult RecuperarUsuarioComMd5(int id)
        {
            return Json(UsuarioDao.RecuperarPeloId(id));
        }



        [HttpPost]
        [Authorize(Roles = "Gerente, Desenvolvedor")]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirUsuario(int id)
        {
            return Json(UsuarioDao.ExcluirPeloId(id));
        }

        [HttpPost]
        [Authorize(Roles = "Gerente, Desenvolvedor")]
        [ValidateAntiForgeryToken]
        public ActionResult SalvarUsuario(UsuarioModel model)
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

                    if (model.Senha == _senhaPadrao)
                    {
                        model.Senha = "";
                    }


                    var id = UsuarioDao.Salvar(model);
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



        [HttpPost]
        [Authorize(Roles = "Gerente, Desenvolvedor")]
        [ValidateAntiForgeryToken]
        public JsonResult UsuarioPagina(int pagina, int tamPag, string filtro, string ordem)
        {
            var lista = UsuarioDao.RecuperarLista(pagina, tamPag, filtro);
            return Json(lista);
        }



    }
}