using Microsoft.EntityFrameworkCore;

using GerenciadorTarefas.MVC.Models;

namespace GerenciadorTarefas.MVC.Data

{
    public class TarefaRepositorio : ITarefaRepositorio
    {
        private readonly ApplicationDbContext _context;

        public TarefaRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Tarefa>> GetAllAsync()
        {
            return await _context.Tarefas.ToListAsync();
        }

        public async Task<Tarefa?> GetByIdAsync(int id)
        {
            return await _context.Tarefas.FindAsync(id);
        }

        public async Task AddAsync(Tarefa tarefa)
        {
            _context.Tarefas.Add(tarefa);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tarefa tarefa)
        {
            _context.Tarefas.Update(tarefa);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);

            if (tarefa != null)
            {
                _context.Tarefas.Remove(tarefa);
                await _context.SaveChangesAsync();
            }
        }
    }
}