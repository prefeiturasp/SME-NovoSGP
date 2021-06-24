using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioRegistroIndividual : IRepositorioBase<RegistroIndividual>
    {
        Task<RegistroIndividual> ObterPorAlunoData(long turmaId, long componenteCurricularId, long alunoCodigo, DateTime data);

        Task<PaginacaoResultadoDto<RegistroIndividual>> ObterPorAlunoPeriodoPaginado(long turmaId, long componenteCurricularId, long alunoCodigo, DateTime dataInicio, DateTime dataFim, Paginacao paginacao);

        Task<IEnumerable<UltimoRegistroIndividualAlunoTurmaDto>> ObterUltimosRegistrosPorAlunoTurma(long turmaId);
        Task<SugestaoTopicoRegistroIndividualDto> ObterSugestaoTopicoPorMes(int mes);
        
        Task<IEnumerable<QuantidadeRegistrosIndividuaisPorAnoTurmaDTO>> ObterQuantidadeRegistrosIndividuaisPorAnoTurmaAsync(int anoLetivo, long dreId, long ueId, Modalidade modalidade);

        Task<IEnumerable<RegistroIndividualDTO>> ObterTurmasComRegistrosIndividuaisInfantilEAnoAsync(int anoLetivo, int[] modalidades);

        Task<IEnumerable<AlunoInfantilComRegistroIndividualDTO>> ObterAlunosInfantilComRegistrosIndividuaisPorTurmaAnoAsync(long turmaCodigo, int anoLetivo, int[] modalidades);

        Task<IEnumerable<RegistroIndividualAlunoDTO>> ObterRegistrosIndividuaisPorTurmaAlunoAsync(long turmaCodigo, long codigoAluno);
        Task<IEnumerable<RegistroItineranciaPorAnoDto>> ObterQuantidadeDeAunosSemRegistroPorPeriodoAsync(int anoLetivo, long dreId, Modalidade modalidade, DateTime dataInicial);
        Task<IEnumerable<GraficoBaseDto>> ObterQuantidadeDeAunosSemRegistroPorPeriodoUeAsync(int anoLetivo, long ueId, Modalidade modalidade, DateTime dataInicial);
    }
}
