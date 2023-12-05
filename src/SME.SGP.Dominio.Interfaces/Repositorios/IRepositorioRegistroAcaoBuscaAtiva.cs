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
        Task<PaginacaoResultadoDto<RegistroAcaoBuscaAtivaListagemDto>> ListarPaginado(int anoLetivo, long? dreId, long? ueId, long? turmaId, 
                                                                                      int? modalidade, int semestre,
                                                                                      string nomeAluno, DateTime? dataRegistroInicio,
                                                                                      DateTime? dataRegistroFim, int? ordemRespostaQuestaoProcedimentoRealizado, 
                                                                                      Paginacao paginacao);
    }
}
