using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Dominio.Constantes;

namespace Dominio.Models
{
    public class Usuario
    {
        public Usuario()
        {
            
        }
        
		public Usuario(string nome, string cpf, DateTime dataNascimento, string email, string perfil)
		{
			GerarNovaHash();
            Nome = nome;
            CPF = cpf;
            Email = email;
            DataNascimento = dataNascimento;
            Perfil = perfil;
            Guid = Guid.NewGuid();
		}
        [Key]
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Senha { get; set; }
        public string Perfil { get; set; }
        public string Hash {get; set; }
		public bool HashUtilizado {get;set;}

        public void AlterarSenha(string novaSenha) => this.Senha = CriptografarSenha(novaSenha);

        public void UtilizarHash() => this.HashUtilizado = true;
        
        public void GerarNovaHash()
        {
            this.Hash = Guid.NewGuid().ToString();
            this.HashUtilizado = false;
        }


        public void Atualizar(string nome, string email, string perfil)
        {
            this.Nome=nome;
            this.Email = email;
            this.Perfil = perfil;
        }

        public bool SenhaCorreta(string senhaDigitada)
        {   
            var senhaDigitadaCriptografada = CriptografarSenha(senhaDigitada);
            return (this.Senha == senhaDigitadaCriptografada);
        }

        private string CriptografarSenha(string txt)
        {            
            var algoritmo = SHA512.Create();
            var senhaEmBytes = Encoding.UTF8.GetBytes(txt);
            var senhaCifrada = algoritmo.ComputeHash(senhaEmBytes);
            
            var sb = new StringBuilder();
            
            foreach (var caractere in senhaCifrada)            
                sb.Append(caractere.ToString("X2"));
            
            return sb.ToString();
        }
        
        private string AbreviarNome(string nomeCompleto)
        {
            var array = nomeCompleto.Split(' ');
            var primeiroNome = array[0];
            var nomeAbreviado = primeiroNome;
            
            if (array.Length > 1)            {
                var ultimoNome = array[array.Length-1];                            
                nomeAbreviado += " " + ultimoNome;
            }

            return nomeAbreviado;
        }

    }
}