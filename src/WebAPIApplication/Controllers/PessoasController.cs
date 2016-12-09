using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPIApplication {
    [Route("api/pessoas")]
    public class PessoasController : Controller {
        private readonly DataContext _dataContext;

        public PessoasController(DataContext dataContext){
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<IActionResult> ObterPessoas(){
            var pessoas = await _dataContext.Pessoas.ToListAsync();
            return Json(pessoas);
        }

        [HttpGet]
        public async Task<IActionResult> ObterPessoa(int id){
            var pessoa = await _dataContext.Pessoas.FindAsync(id);

            if(pessoa == null)
                return NotFound();
            
            return Json(pessoa);
        }

        [HttpPost]
        public async Task<IActionResult> CriaPessoa([FromBody]Pessoa modelo){
            await _dataContext.Pessoas.AddAsync(modelo);
            await _dataContext.SaveChangesAsync();
            return Json(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> AtualizaPessoa([FromBody]Pessoa modelo){
            var pessoa = await _dataContext.Pessoas.FindAsync(modelo.Id);

            if(pessoa == null)
                return NotFound();
            
            pessoa.Nome = modelo.Nome;
            pessoa.Twitter = modelo.Twitter;

            await _dataContext.SaveChangesAsync();
            return Json(modelo);
        }

        [HttpGet]
        public async Task<IActionResult> RemovePessoa(int id){
            var pessoa = await _dataContext.Pessoas.FindAsync(id);

            if(pessoa == null)
                return NotFound();
            
            _dataContext.Pessoas.Remove(pessoa);
            
            await _dataContext.SaveChangesAsync();
            return StatusCode((int)HttpStatusCode.OK);
        }
    }
}