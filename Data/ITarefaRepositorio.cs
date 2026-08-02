using GerenciadorTarefas.MVC.Models;

namespace GerenciadorTarefas.MVC.Data

{
    public interface ITarefaRepositorio
    {
        Task<List<Tarefa>> GetAllAsync();

        Task<Tarefa?> GetByIdAsync(int id);

        Task AddAsync(Tarefa tarefa);

        Task UpdateAsync(Tarefa tarefa);

        Task DeleteAsync(int id);
    }
}
