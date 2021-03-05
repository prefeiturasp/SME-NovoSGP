using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioOcorrencia : IRepositorioBase<Ocorrencia>
    {
        Task<PaginacaoResultadoDto<Ocorrencia>> ListarPaginado(long turmaId, string titulo, string alunoNome, DateTime? dataOcorrenciaInicio, DateTime? dataOcorrenciaFim, long[] codigosAluno, Paginacao paginacao);

        Task<PaginacaoResultadoDto<OcorrenciasPorAlunoDto>> ObterOcorrenciasPorTurmaAlunoEPeriodoPaginadas(long turmaId, long codigoAluno, DateTime periodoInicio, DateTime periodoFim, Paginacao paginacao);
    }
}
