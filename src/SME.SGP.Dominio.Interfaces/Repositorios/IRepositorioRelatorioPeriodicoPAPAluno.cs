using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRelatorioPeriodicoPAPAluno : IRepositorioBase<RelatorioPeriodicoPAPAluno>
    {
        Task<IEnumerable<string>> ObterCodigosDeAlunosComRelatorioJaPreenchido(long turmaId, long periodoRelatorioPAPId);
        Task<RelatorioPAPAlunoConselhoClasseDto> ObterDadosRelatorioPAPAlunoConselhoClasse(int anoLetivo, string alunoCodigo, int bimestre, ModalidadeTipoCalendario modalidade);
    }
}
