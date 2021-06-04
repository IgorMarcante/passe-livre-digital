using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web.Models.ViewModels;
using Dominio.Interfaces;
using Web.ViewModels;
using Web.Extensions;
using Dominio;
using Dominio.Models;

namespace Web.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly Context _context;
        private readonly TiaIdentity.Autenticador tiaIdentity;

        public UsuarioController(Context context, TiaIdentity.Autenticador tiaIdentity)
        {
            _context = context;
            this.tiaIdentity = tiaIdentity;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM viewmodel)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(a => a.CPF == viewmodel.Usuario.Replace(".", "").Replace("-", ""));

            var loginOuSenhaIncorretos = (usuario == null) || !(usuario.SenhaCorreta(viewmodel.Senha));

            if (loginOuSenhaIncorretos)
            {
                this.Error("Usuário ou Senha incorretos!");
                return View();
            }

            if (ModelState.IsValid)
            {
                await tiaIdentity.LoginAsync(usuario.CPF, usuario.Nome, usuario.Perfil, lembrar: true);
                return RedirectToAction("Index", "Candidato");
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await tiaIdentity.LogoutAsync();
            return View(nameof(Login));
        }

        public IActionResult AcessoNegado() => View();

        public IActionResult Login() => View();

        // GET: Usuario
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }

        // GET: Usuario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuario/Create
        // public IActionResult Create()
        // {
        //     return View();
        // }

        // POST: Usuario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create(UsuarioVM usuarioVm)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         bool existe = _context.Usuarios.Any(c => c.CPF == usuarioVm.CPF.Replace(".", "").Replace("-", ""));
        //         bool emailExist = _context.Usuarios.Any(c => c.Email == usuarioVm.Email);
        //         bool invalido = Web.Helpers.ValidadorHelper.ValidarCPF(usuarioVm.CPF.Replace(".", "").Replace("-", ""));

        //         if (!invalido)
        //         {
        //             this.Error("CPF invalido.");
        //             return View(usuarioVm);
        //         }

        //         if (existe)
        //         {
        //             this.Error("Já existe um usuário cadastrado com esse CPF.");
        //             return View(usuarioVm);
        //         }

        //         if (emailExist)
        //         {
        //             this.Error("Já existe um usuário cadastrado com esse email.");
        //             return View(usuarioVm);
        //         }

        //         var _usuario = new Usuario(usuarioVm.Nome, usuarioVm.CPF.Replace(".", "").Replace("-", ""), usuarioVm.Email, "Candidato");
        //         _usuario.AlterarSenha(usuarioVm.Senha);
        //         _context.Add(_usuario);
        //         await _context.SaveChangesAsync();
        //         this.Success();
        //          await tiaIdentity.LoginAsync(_usuario.CPF, _usuario.Nome, _usuario.Perfil, lembrar: true);
        //         return RedirectToAction("Index", "Candidato");
        //     }
        //     this.Error("Erro ao realizar cadastro. Revise os campos preenchidos.");
        //     return View(usuarioVm);
        // }

        // GET: Usuario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        public ActionResult EsqueciMinhaSenha()
        {
            return View();
        }

        // [HttpPost, ValidateAntiForgeryToken]
        // public async Task<IActionResult> EsqueciMinhaSenha(string cpf)
        // {
        //     var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.CPF == cpf.Replace(".", "").Replace("-", ""));

        //     if (usuario is null)
        //         return NotFound();

        //     usuario.GerarNovaHash();

        //     _context.Update(usuario);

        //     await _context.SaveChangesAsync();
        //     await servicoDeEmail.EnviarEmailParaTrocaDeSenha(usuario.Email, usuario.Hash);
        //     this.Success();
        //     return RedirectToAction("Index", "Home");
        // }

        public async Task<IActionResult> AlterarSenha(string id)
        {

            // var usuario = await _context.Usuarios.FirstOrDefaultAsync(c => c.CPF == "NONONO");

            // if (usuario == null)
            // {
            //   //this.Error("Este link está expirado.");
            //   return RedirectToAction(nameof(Login));
            // }

            // var viewModel = new AlterarSenhaVM(usuario);

            // return View(viewModel);
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AlterarSenha(AlterarSenhaVM viewModel)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Hash == viewModel.Id);

                if (usuario == null)
                {
                    //this.Atencao("Link expirado!");
                    return RedirectToAction(nameof(Login));
                }

                usuario.AlterarSenha(viewModel.NovaSenha);
                usuario.UtilizarHash();

                _context.Update(usuario);
                await _context.SaveChangesAsync();
                this.Success();
                return RedirectToAction("Index", "Home");
            }

            return View(viewModel);
        }
    }
}
