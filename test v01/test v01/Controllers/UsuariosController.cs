using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using test_v01.Repository;
using test_v01.Repository.Models;

namespace test_v01.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly SITEtccDbContext _context = new SITEtccDbContext();
        

     
         // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var isAdminClaim = User.FindFirst("IsAdmin");
            if (isAdminClaim == null || !bool.Parse(isAdminClaim.Value))
            {
                return Forbid(); // Retorna um erro 403 - Acesso proibido
            }

           
            return _context.Usuarios != null ? 
                          View(await _context.Usuarios.ToListAsync()) :
                          Problem("Entity set 'SITEtccDbContext.Usuarios'  is null.");
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var isAdminClaim = User.FindFirst("IsAdmin");
            if (isAdminClaim == null || !bool.Parse(isAdminClaim.Value))
            {
                return Forbid(); // Retorna um erro 403 - Acesso proibido
            }
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Idusuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()

        {
            var isAdminClaim = User.FindFirst("IsAdmin");
            if (isAdminClaim == null || !bool.Parse(isAdminClaim.Value))
            {
                return Forbid(); // Retorna um erro 403 - Acesso proibido
            }
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idusuario,Emailusuario,Senhausuario,Recmail,Nomeusuario,Telefoneusuario")] Usuario usuario)
        {
            var isAdminClaim = User.FindFirst("IsAdmin");
            if (isAdminClaim == null || !bool.Parse(isAdminClaim.Value))
            {
                return Forbid(); // Retorna um erro 403 - Acesso proibido
            }
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var isAdminClaim = User.FindFirst("IsAdmin");
            if (isAdminClaim == null || !bool.Parse(isAdminClaim.Value))
            {
                return Forbid(); // Retorna um erro 403 - Acesso proibido
            }
            if (id == null || _context.Usuarios == null)
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

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idusuario,Emailusuario,Senhausuario,Recmail,Nomeusuario,Telefoneusuario")] Usuario usuario)
        {
            var isAdminClaim = User.FindFirst("IsAdmin");
            if (isAdminClaim == null || !bool.Parse(isAdminClaim.Value))
            {
                return Forbid(); // Retorna um erro 403 - Acesso proibido
            }
            if (id != usuario.Idusuario)
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
                    if (!UsuarioExists(usuario.Idusuario))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var isAdminClaim = User.FindFirst("IsAdmin");
            if (isAdminClaim == null || !bool.Parse(isAdminClaim.Value))
            {
                return Forbid(); // Retorna um erro 403 - Acesso proibido
            }
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Idusuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var isAdminClaim = User.FindFirst("IsAdmin");
            if (isAdminClaim == null || !bool.Parse(isAdminClaim.Value))
            {
                return Forbid(); // Retorna um erro 403 - Acesso proibido
            }
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'SITEtccDbContext.Usuarios'  is null.");
            }
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
          return (_context.Usuarios?.Any(e => e.Idusuario == id)).GetValueOrDefault();
        }
    }
}
