using System;
using System.Threading.Tasks;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Dominio.Models
{
public class Anexo : IAnexo
	{        
        public Anexo()
        {
            
        }

        public Anexo(IFormFile arquivo)
        {            
            this.Nome = arquivo.FileName;
            this.Mime = arquivo.ContentType;
        }
        
        public Guid Id { get; set; }
        public string Mime { get; set; }
        public string Nome { get; set; }        
        public string LocalizadorDoAnexo { get; private set; } = Guid.NewGuid().ToString();
        public byte[] Bytes { get; set; }
    }
}