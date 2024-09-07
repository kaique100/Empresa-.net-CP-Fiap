using Empresa.Models;

namespace Empresa.Reapository.Interface
{
    public interface IDepartamentoRepository
    {
        Task<IEnumerable<Departamento>> GetDepartamentos();
        Task<Departamento> GetDepartamentoById(int depId);
        Task<IEnumerable<Empregado>> GetEmpregadosByDepId(int DepartamentoId); // Busca os Empregados pelo Nome do Departamento
        Task<Departamento> AddDepartamento(Departamento departamento);
        Task<Departamento> UpdateDepartamento(Departamento departamento);
        void DeleteDepartamento(int depId);
    }
}
