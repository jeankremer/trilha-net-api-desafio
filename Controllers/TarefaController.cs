using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            // Verificar se existe um registro com o ID fornecido no banco de dados.
            var tarefa = _context.Tarefas.FirstOrDefault(t => t.Id == id);
            
            // Se o registro não for encontrado, retornamos um NotFound.
            if (tarefa == null)
            {
                return NotFound();
            }

             // Se encontrarmos o registro, retornamos um Ok com o objeto encontrado.
            return Ok(tarefa);

        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            // Buscar todas as tarefas no banco utilizando EF
            var tarefas = _context.Tarefas.ToList();

            // Agora, vamos retornar um resultado Ok com a lista de tarefas encontradas
            return Ok(tarefas);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            // Filtrar tarefas cuja data seja igual a data fornecida
            var tarefas = _context.Tarefas.Where(x => x.Titulo == titulo).ToList();

            // Agora, vamos retornar um resultado Ok com as tarefas encontradas
            return Ok(tarefas);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            // Buscar as tarefas no banco utilizando o EF, que contenham o status recebido por parâmetro
            var tarefa = _context.Tarefas.Where(x => x.Status == status).ToList();
            return Ok(tarefa);
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
            {
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });    
            }


            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();



            // Agora, vamos retornar um resultado CreatedAtAction com o ID da tarefa criada
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // Atualizar as informações da variável tarefaBanco com a tarefa recebida via parâmetro
             tarefaBanco.Descricao = tarefa.Descricao;
             tarefaBanco.Titulo = tarefa.Titulo;
             tarefaBanco.Status = tarefa.Status;
             tarefaBanco.Data = tarefa.Data;

             // Atualizar a variável tarefaBanco no EF e salvar as mudanças
             _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            // Buscar a tarefa no banco pelo ID
            var tarefaBanco = _context.Tarefas.Find(id);

            // Verificar se a tarefa existe
            if (tarefaBanco == null)
                return NotFound();

            // Remover a tarefa encontrada através do EF e salvar as mudanças
            _context.Tarefas.Remove(tarefaBanco);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
