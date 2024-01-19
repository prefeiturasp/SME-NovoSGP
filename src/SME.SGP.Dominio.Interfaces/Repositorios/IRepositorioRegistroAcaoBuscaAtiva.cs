using System;
using System.Collections.Generic;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRegistroAcaoBuscaAtiva : IRepositorioBase<RegistroAcaoBuscaAtiva>
    {
        Task<PaginacaoResultadoDto<RegistroAcaoBuscaAtivaCriancaEstudanteAusenteDto>> ListarPaginadoCriancasEstudantesAusentes(string codigoAluno, long turmaId, Paginacao paginacao);
        Task<RegistroAcaoBuscaAtiva> ObterRegistroAcaoPorId(long id);
        Task<RegistroAcaoBuscaAtiva> ObterRegistroAcaoComTurmaPorId(long id);
        Task<IEnumerable<string>> ObterCodigoArquivoPorRegistroAcaoId(long id);
        Task<PaginacaoResultadoDto<RegistroAcaoBuscaAtivaListagemDto>> ListarPaginado(FiltroTurmaRegistrosAcaoDto filtroTurma,
                                                                                      FiltroRespostaRegistrosAcaoDto filtroRespostas, 
                                                                                      Paginacao paginacao);
    }
}
