using Microsoft.AspNetCore.Mvc;

using GerenciadorTarefas.MVC.Data;

using GerenciadorTarefas.MVC.Models;

namespace GerenciadorTarefas.MVC.Controllers

{
    // TODO: Pessoa5 adiciona [Authorize] e filtro por UsuarioId aqui
    public class TarefasController : Controller
    {
        private readonly ITarefaRepositorio _repositorio;

        public TarefasController(ITarefaRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        // GET: Tarefas
        public async Task<IActionResult> Index()
        {
            var tarefas = await _repositorio.GetAllAsync();
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

            await _repositorio.AddAsync(tarefa);
            return RedirectToAction(nameof(Index));
        }

        // GET: Tarefas/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var tarefa = await _repositorio.GetByIdAsync(id);

            if (tarefa == null) return NotFound();
            return View(tarefa);
        }

        // POST: Tarefas/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Tarefa tarefa)
        {
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
