using AlunosApi.Models;
using AlunosApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlunosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Produces("application/json")] só retorna json, poderia ser XML
    public class AlunosController : ControllerBase
    {
        private IAlunoService _alunoservice;

        public AlunosController(IAlunoService alunoservice)
        {
            _alunoservice = alunoservice;
        }

        [HttpGet]
        //[ProducesReponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IAsyncEnumerable<Aluno>>> GetAlunos()
        {
            try
            {
                var alunos = await _alunoservice.GetAlunos();
                return Ok(alunos);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"Error ao obter Alunos");
            }
        }

        [HttpGet("AlunoPorNome")]
        //[ProducesReponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IAsyncEnumerable<Aluno>>> GetAlunosByName([FromQuery] string name)
        {
            try
            {
                var alunos = await _alunoservice.GetAlunoByNome(name);
                if (alunos == null)
                    return NotFound($"Não Existem alunos com critérios {name}");

                return Ok(alunos);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error ao obter Alunos");
            }
        }

        [HttpGet("{id:int}", Name = "GetAluno")]
        public async Task<ActionResult<Aluno>> GetAluno(int id)
        {
            try
            {
                var aluno = await _alunoservice.GetAlunoById(id);

                if (aluno == null)
                    return NotFound($"Aluno com id= {id} não encontrado");

                return Ok(aluno);
            }
            catch
            {
                return BadRequest("Request inválido");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(Aluno aluno)
        {
            try
            {
                await _alunoservice.CreateAluno(aluno);

                return CreatedAtRoute(nameof(GetAluno), new { id = aluno.AlunoId}, aluno); //Cadastra 201 que é o objeto criado e já retorna o objecto recem criado.
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error ao obter Aluno");
            }
        }

        [HttpPut("{alunoId:int}")]
        public async Task<ActionResult> Edit(int alunoId, [FromBody] Aluno aluno)
        {
            try
            {
                if(aluno.AlunoId == alunoId)
                {
                    await _alunoservice.UpdateAluno(aluno);
                    return Ok($"Aluno atualizado com sucesso");
                }
                else
                {
                    return BadRequest("Dados incosistentes");
                }
            }
            catch
            {
                return BadRequest("Error ao obter Aluno");
            }
        }

        [HttpDelete("{alunoId:int}")]
        public async Task<ActionResult> Delete(int alunoId)
        {
            try
            {
                var aluno = await _alunoservice.GetAlunoById(alunoId);
                if (aluno != null)
                {
                    await _alunoservice.DeleteAluno(aluno);
                    return Ok($"Aluno deletado com sucesso");
                }
                else
                {
                    return NotFound("Aluno não encontrado");
                }
            }
            catch
            {
                return BadRequest("Error ao obter Aluno");
            }
        }
    }
}
