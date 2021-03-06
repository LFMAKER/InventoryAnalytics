﻿using Dapper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace ControleEstoque.Web.Models
{
    public class Fornecedor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o nome.")]
        [MaxLength(200, ErrorMessage = "O nome pode ter no máximo 200 caracteres.")]
        public string Nome { get; set; }

        [MaxLength(200, ErrorMessage = "A razão social pode ter no máximo 200 caracteres.")]
        public string RazaoSocial { get; set; }

        [MaxLength(20, ErrorMessage = "O número do documento pode ter no máximo 20 caracteres.")]
        public string NumDocumento { get; set; }

        [Required]
        public TipoPessoa Tipo { get; set; }

        [Required(ErrorMessage = "Preencha o telefone.")]
        [MaxLength(20, ErrorMessage = "O telefone deve ter 20 caracteres.")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Preencha o contato.")]
        [MaxLength(60, ErrorMessage = "O contato deve ter 60 caracteres.")]
        public string Contato { get; set; }

        [Required(ErrorMessage = "Preencha o logradouro.")]
        [MaxLength(200, ErrorMessage = "O logradouro do endereço pode ter no máximo 200 caracteres.")]
        public string Logradouro { get; set; }

        [Required(ErrorMessage = "Preencha o numero.")]
        [MaxLength(20, ErrorMessage = "O número do endereço pode ter no máximo 20 caracteres.")]
        public string Numero { get; set; }

        [MaxLength(100, ErrorMessage = "O complemento do endereço pode ter no máximo 100 caracteres.")]
        public string Complemento { get; set; }

        [Required(ErrorMessage = "Preencha o CEP.")]
        [MaxLength(10, ErrorMessage = "O CEP do endereço pode ter no máximo 10 caracteres.")]
        public string Cep { get; set; }
        
        [MaxLength(50, ErrorMessage = "O bairro do endereço pode ter no máximo 50 caracteres.")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "Preencha a cidade.")]
        [MaxLength(50, ErrorMessage = "A cidade do endereço pode ter no máximo 50 caracteres.")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "Preencha o estado.")]
        [MaxLength(50, ErrorMessage = "O Estado do endereço pode ter no máximo 50 caracteres.")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "Preencha o pais.")]
        [MaxLength(50, ErrorMessage = "O Pais do endereço pode ter no máximo 50 caracteres.")]
        public string Pais { get; set; }


        public bool Ativo { get; set; }

        public override string ToString()
        {
            return "Id: " + Id + " | Nome: " + Nome + " | Razão Social: " + RazaoSocial + " | Número Documento: " + NumDocumento +
                   " | Tipo Pessoa: " + Tipo + " | Telefone: " + Telefone + " | Contato: " + Contato + " | Logradouro: " + Logradouro +
                   " | CEP: " + Cep + " | Numero: " + Numero + " | Complemento: " + Complemento + " | Bairro: " + Bairro +
                   " | Cidade: " + Cidade + " | Estado: " + Estado + " | País: " + Pais + " | Ativo: " + Ativo;
        }

    }
}