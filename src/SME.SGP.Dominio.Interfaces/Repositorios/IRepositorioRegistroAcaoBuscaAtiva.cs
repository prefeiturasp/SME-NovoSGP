using System;
using System.Collections.Generic;
using SME.SGP.Infra;
using System.Threading.Tasks;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRegistroAcaoBuscaAtiva : IRepositorioBase<RegistroAcaoBuscaAtiva>
    {
        Task<IEnumerable<long>> ObterIdsRegistrosAlunoResponsavelContatado(DateTime dataRef, long ueId, int anoLetivo);
        Task<RegistroAcaoBuscaAtivaAlunoDto> ObterRegistroBuscaAtivaAluno(long id);
        Task<PaginacaoResultadoDto<RegistroAcaoBuscaAtivaCriancaEstudanteAusenteDto>> ListarPaginadoCriancasEstudantesAusentes(string codigoAluno, long turmaId, Paginacao paginacao);
        Task<RegistroAcaoBuscaAtiva> ObterRegistroAcaoPorId(long id);
        Task<RegistroAcaoBuscaAtiva> ObterRegistroAcaoComTurmaPorId(long id);
        Task<IEnumerable<string>> ObterCodigoArquivoPorRegistroAcaoId(long id);
        Task<PaginacaoResultadoDto<RegistroAcaoBuscaAtivaListagemDto>> ListarPaginado(FiltroTurmaRegistrosAcaoDto filtroTurma,
                                                                                      FiltroRespostaRegistrosAcaoDto filtroRespostas, 
                                                                                      Paginacao paginacao);
        Task<PaginacaoResultadoDto<RegistroAcaoBuscaAtivaNAAPADto>> ListarPaginadoRegistroAcaoParaNAAPA(string codigoAluno, Paginacao paginacao);
        Task<int> ObterQdadeRegistrosAcaoAlunoMes(string alunoCodigo, int mes, int anoLetivo);
        Task<DadosBuscaAtivaAlunoDto> ObterDadosBuscaAtivaPorAluno(string alunoCodigo, int anoLetivo);
    }
}
