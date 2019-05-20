﻿using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace ControleEstoque.Web.Models
{
    public class Produto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o código.")]
        [MaxLength(10, ErrorMessage = "O código pode ter no máximo 10 caracteres.")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Preencha o nome.")]
        [MaxLength(50, ErrorMessage = "O nome pode ter no máximo 50 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Preencha o preço de custo.")]
        public decimal PrecoCusto { get; set; }

        [Required(ErrorMessage = "Preencha o preço de venda.")]
        public decimal PrecoVenda { get; set; }

        [Required(ErrorMessage = "Preencha a quantidade em estoque.")]
        public int QuantEstoque { get; set; }

        [Required(ErrorMessage = "Selecione a unidade de medida.")]
        public int IdUnidadeMedida { get; set; }

        public virtual UnidadeMedida UnidadeMedida { get; set; }

        [Required(ErrorMessage = "Selecione o grupo.")]
        public int IdGrupo { get; set; }
        public virtual GrupoProduto GrupoProduto { get; set; }

        [Required(ErrorMessage = "Selecione a marca.")]
        public int IdMarca { get; set; }

        public virtual MarcaProduto MarcaProduto { get; set; }

        [Required(ErrorMessage = "Selecione o fornecedor.")]
        public int IdFornecedor { get; set; }

        public virtual Fornecedor Fornecedor { get; set; }

        [Required(ErrorMessage = "Selecione o local de armazenamento.")]
        public int IdLocalArmazenamento { get; set; }

        public virtual LocalArmazenamento LocalArmazenamento { get; set; }

        public bool Ativo { get; set; }

    }
}