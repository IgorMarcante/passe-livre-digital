using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Web.Extensions
{
    public static class ControllerExtensions
    {
        public static string CPF(this System.Security.Claims.ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;                        
        }  
              
        public static string IdDoUsuarioLogado(this Controller controller)
        {
            var id = controller.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier).Value;
            return Convert.ToString(id);
        }

         public static void Success(this Controller controller, string mensagem = "Operação concluída com sucesso.")
        {   
            controller.TempData["success"] = mensagem;
        }
        
        public static void Info(this Controller controller, string mensagem)
        {   
            controller.TempData["info"] = mensagem;
        }        
       
               public static void Error(this Controller controller, string mensagem)
        {   
            controller.TempData["error"] = mensagem;
        }     
    }
}