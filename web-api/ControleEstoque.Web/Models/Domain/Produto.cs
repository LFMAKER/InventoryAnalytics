﻿using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Rotativa;

namespace ControleEstoque.Web.Models
{
    public class Produto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o código.")]
        [MaxLength(10, ErrorMessage = "O código pode ter no máximo 10 caracteres.")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Preencha o nome.")]
        [MaxLength(200, ErrorMessage = "O nome pode ter no máximo 200 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Preencha o preço de custo.")]
        public decimal PrecoCusto { get; set; }

        [Required(ErrorMessage = "Preencha o preço de venda.")]
        public decimal PrecoVenda { get; set; }

        [Required(ErrorMessage = "Preencha a quantidade em estoque.")]
        public int QuantEstoque { get; set; }

        public int IdUnidadeMedida { get; set; }
        public UnidadeMedida UnidadeMedida { get; set; }

        public int IdGrupo { get; set; }
        public GrupoProduto GrupoProduto { get; set; }

        public int IdMarca { get; set; }
        public MarcaProduto MarcaProduto { get; set; }

        public int IdFornecedor { get; set; }
        public Fornecedor Fornecedor { get; set; }

        public int IdLocalArmazenamento { get; set; }
        public LocalArmazenamento LocalArmazenamento { get; set; }

        public bool Ativo { get; set; }

        public override string ToString()
        {
            return "Id: " + Id + " | Código: " + Codigo + " | Nome: " + Nome + " | Preço Custo: " + PrecoCusto + " | Preço Venda: " + PrecoVenda + 
                " | Quantidade Estoque: " + QuantEstoque + " | IdUnidadeMedida: " + IdUnidadeMedida + " | IdGrupoProduto: " + IdGrupo +
                " | IdMarcaProduto: " + IdMarca + " | IdFornecedor: " + IdFornecedor + " | Ativo: " + Ativo;
        }

    }
}