﻿using ControleEstoque.Web.Dal.Cadastro;
using ControleEstoque.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ControleEstoque.Web.Controllers
{
    [Authorize(Roles = "Gerente,Administrativo,Operador")]
    public class CadCidadeController : Controller
    {
        private const int _quantMaxLinhasPorPagina = 5;

        public ActionResult Index()
        {
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = 1;

            var lista = CidadeDao.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);
            var quant = CidadeDao.RecuperarQuantidade();

            var difQuantPaginas = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + difQuantPaginas;
            ViewBag.Paises = PaisDao.RecuperarLista();

            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CidadePagina(int pagina, int tamPag, string filtro, string ordem)
        {
            var lista = CidadeDao.RecuperarLista(pagina, tamPag, filtro, ordem: ordem);

            return Json(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarCidade(int id)
        {
            return Json(CidadeDao.RecuperarPeloId(id));
        }

        [HttpPost]
        [Authorize(Roles = "Gerente,Administrativo")]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirCidade(int id)
        {

            /*-----GERANDO LOG-------*/
            List<IList<Object>> objNewRecords = new List<IList<Object>>();
            IList<Object> obj = new List<Object>();
            obj.Add(User.Identity.Name.ToString());
            obj.Add("Remover Cidade");
            obj.Add((string)Log.IpUsuario());
            obj.Add((string)Log.MacAddressUsuario());
            obj.Add("ALTA");
            obj.Add((string)DateTime.Now.ToString());
            string dados = "Id: " + id;
            obj.Add(dados);
            objNewRecords.Add(obj);
            Apis.GoogleSheets.GoogleSheetsAPI.RequestLogsGravar(objNewRecords);
            /*---- LOG GERADA -----*/

            return Json(CidadeDao.ExcluirPeloId(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarCidadesDoEstado(int idEstado)
        {
            var lista = CidadeDao.RecuperarLista(idEstado: idEstado);

            return Json(lista);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarCidade(CidadeModel model)
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

                    /*-----GERANDO LOG-------*/
                    List<IList<Object>> objNewRecords = new List<IList<Object>>();
                    IList<Object> obj = new List<Object>();
                    obj.Add(User.Identity.Name.ToString());
                    obj.Add("Adicionar Cidade");
                    obj.Add((string)Log.IpUsuario());
                    obj.Add((string)Log.MacAddressUsuario());
                    obj.Add("ALTA");
                    obj.Add((string)DateTime.Now.ToString());
                    string dados = "Id: " + model.Id + " | Nome: " + model.Nome + " | IdEstado: " + model.IdEstado + " | IdPais: " + model.IdPais + " | Status: " + model.Ativo;
                    obj.Add(dados);
                    objNewRecords.Add(obj);
                    Apis.GoogleSheets.GoogleSheetsAPI.RequestLogsGravar(objNewRecords);
                    /*---- LOG GERADA -----*/

                    var id = CidadeDao.Salvar(model);
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