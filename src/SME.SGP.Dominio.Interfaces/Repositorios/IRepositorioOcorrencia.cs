using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioOcorrencia : IRepositorioBase<Ocorrencia>
    {
        Task<PaginacaoResultadoDto<Ocorrencia>> ListarPaginado(FiltroOcorrenciaListagemDto filtro, string[] codigosServidores, Paginacao paginacao);

        Task<PaginacaoResultadoDto<OcorrenciasPorAlunoDto>> ObterOcorrenciasPorTurmaAlunoEPeriodoPaginadas(long turmaId, long codigoAluno, DateTime periodoInicio, DateTime periodoFim, Paginacao paginacao);
    }
}
