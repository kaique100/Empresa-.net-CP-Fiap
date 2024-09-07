using Empresa.Data;
using Empresa.Models;
using Empresa.Reapository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Empresa.Repository;

public class DepartamentoRepository : IDepartamentoRepository
{
    private readonly dbContext dbContext;

    public DepartamentoRepository(dbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<Departamento>> GetDepartamentos()
    {
        return await dbContext.Departamentos.ToListAsync();
    }

    public async Task<Departamento> GetDepartamentoById(int DepId)
    {
        return await dbContext.Departamentos.FirstOrDefaultAsync(d => d.DepId == DepId);
    }

    public async Task<IEnumerable<Empregado>> GetEmpregadosByDepId(int DepId)
    {
        var departamento = await dbContext.Departamentos.FindAsync(DepId);

        if (departamento == null)
        {
            return Enumerable.Empty<Empregado>(); // Retorna uma lista vazia se o departamento não existir
        }

        return await dbContext.Empregados
            .Where(e => e.DepId == DepId)
            .ToListAsync();
    }

    public async Task<Departamento> AddDepartamento(Departamento departamento)
    {
        var result = await dbContext.Departamentos.AddAsync(departamento);
        await dbContext.SaveChangesAsync();

        return result.Entity;
    }

    public async Task<Departamento> UpdateDepartamento(Departamento departamento)
    {
        var existingDepartamento = await dbContext.Departamentos.FirstOrDefaultAsync(d => d.DepId == departamento.DepId);
        if (existingDepartamento == null)
        {
            return null; // Retorna null se o departamento não for encontrado
        }

        // Atualiza o nome do departamento
        existingDepartamento.DepNome = departamento.DepNome;

        await dbContext.SaveChangesAsync();
        return existingDepartamento;
    }

    public async void DeleteDepartamento(int DepId)
    {
        var result = await dbContext.Departamentos.FirstOrDefaultAsync(d => d.DepId == DepId);
        if (result != null)
        {
            dbContext.Departamentos.Remove(result);
            await dbContext.SaveChangesAsync();
        }
    }
}
