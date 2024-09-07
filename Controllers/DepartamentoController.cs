﻿
using Empresa.Models;
using Empresa.Reapository;
using Empresa.Reapository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Empresa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentoController : ControllerBase
    {
        private readonly IDepartamentoRepository departamentoRepository;

        public DepartamentoController(IDepartamentoRepository departamentoRepository)
        {
            this.departamentoRepository = departamentoRepository;
        }

        [HttpGet]

        public async Task<ActionResult> getDepartamentos()
        {
            try
            {
                return Ok(await departamentoRepository.GetDepartamentos());
            }
            catch (Exception) {

                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao recuperar dados do banco de dados");
                    
            }
        }

        [HttpGet("{id:int}")]

        public async Task<ActionResult<Departamento>> getDepartamento(int id)
        {
            try
            {
                var result = await departamentoRepository.GetDepartamentoById(id);
                if (result == null) return NotFound();

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao recuperar dados do banco de dados");
            }
        }
        [HttpPost]

        public async Task<ActionResult<Departamento>> createDepartamento([FromBody] Departamento departamento)
        {
            try
            {
                if (departamento == null) return BadRequest();

                var result = await departamentoRepository.AddDepartamento(departamento);

                return CreatedAtAction(nameof(getDepartamento), new { id = result.DepId }, result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao adicionar dados no banco de dados");
            }
        }
        [HttpPut("{DepId:int}")]

        public async Task<ActionResult<Departamento>> Updatedepartamento(int DepId, [FromBody] Departamento departamento)
        {
            try
            {
                if (departamento == null)
                {
                    return BadRequest("Dados do departamento inválidos.");
                }

                departamento.DepId = DepId;

                var result = await departamentoRepository.UpdateDepartamento(departamento);
                if (result == null) return NotFound($"Empregado chamado = {departamento.DepNome} não encontrado");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao atualizar dados no banco de dados");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Departamento>> DeleteDepamento(int id)
        {
            try
            {
                var result = await departamentoRepository.GetDepartamentoById(id);
                if (result == null) return NotFound($"Empregado com id = {id} não encontrado");

                departamentoRepository.DeleteDepartamento(id);

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao deletar dados no banco de dados");
            }

        }

        [HttpGet("empregados/{DepId:int}")]
        public async Task<ActionResult<IEnumerable<Empregado>>> GetEmpregadosByDepId(int DepId)
        {
            try
            {
                var empregados = await departamentoRepository.GetEmpregadosByDepId(DepId);
                if (!empregados.Any()) // Verifica se a lista de empregados está vazia
                {
                    return NotFound($"Nenhum empregado encontrado para o departamento ID: {DepId}");
                }
                return Ok(empregados);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao recuperar dados do banco de dados");
            }
        }
    }
}
