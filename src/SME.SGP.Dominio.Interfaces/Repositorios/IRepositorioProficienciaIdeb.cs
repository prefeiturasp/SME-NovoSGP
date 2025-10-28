using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioProficienciaIdeb : IRepositorioBase<ProficienciaIdeb>
    {
        Task<bool> ExcluirProficienciaAsync(int anoLetivo, string codigoUe, SerieAnoIndiceDesenvolvimentoEnum serieAno, ComponenteCurricularEnum componenteCurricular);
    }
}
