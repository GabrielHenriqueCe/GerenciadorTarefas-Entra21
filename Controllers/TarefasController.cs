using GerenciadorTarefas.MVC.Data;
using GerenciadorTarefas.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GerenciadorTarefas.MVC.Controllers

{
    // TODO: Pessoa5 adiciona [Authorize] e filtro por UsuarioId aqui
    [Authorize]
    public class TarefasController : Controller
    {
        private readonly ITarefaRepositorio _repositorio;

        public TarefasController(ITarefaRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        // GET: Tarefas
        public async Task<IActionResult> Index(string? status)
        {
            int usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var tarefas = await _repositorio.GetAllAsync();
            tarefas = tarefas.Where(t => t.UsuarioId == usuarioId).ToList();

            if (status == "concluida")
                tarefas = tarefas.Where(t => t.Concluida).ToList();
            else if (status == "pendente")
                tarefas = tarefas.Where(t => !t.Concluida).ToList();

            return View(tarefas);
        }

        // GET: Tarefas/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var tarefa = await _repositorio.GetByIdAsync(id);

            if (tarefa == null) return NotFound();
            return View(tarefa);
        }

        // GET: Tarefas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tarefas/Create
        [HttpPost]
        public async Task<IActionResult> Create(Tarefa tarefa)
        {
            if (!ModelState.IsValid)
                return View(tarefa);

            tarefa.UsuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _repositorio.AddAsync(tarefa);
            return RedirectToAction(nameof(Index));
        }

        // GET: Tarefas/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            int usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var tarefa = await _repositorio.GetByIdAsync(id);

            if (tarefa == null || tarefa.UsuarioId != usuarioId)
                return NotFound();

            return View(tarefa);
        }

        // POST: Tarefas/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Tarefa tarefa)
        {
            int usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (id != tarefa.Id) return NotFound();

            if (!ModelState.IsValid)
            return View(tarefa);

            await _repositorio.UpdateAsync(tarefa);
            return RedirectToAction(nameof(Index));
        }

        // GET: Tarefas/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var tarefa = await _repositorio.GetByIdAsync(id);

            if (tarefa == null) return NotFound();
            return View(tarefa);
        }

        // POST: Tarefas/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repositorio.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
