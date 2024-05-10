using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioEncaminhamentoAEE : IRepositorioBase<EncaminhamentoAEE>
    {
        Task<PaginacaoResultadoDto<EncaminhamentoAEEAlunoTurmaDto>> ListarPaginado(long dreId, long ueId, long turmaId, string alunoCodigo, int? situacao,
            string responsavelRf, int anoLetivo, string[] turmasCodigos, Paginacao paginacao, bool exibirEncerrados);
        Task<SituacaoAEE> ObterSituacaoEncaminhamentoAEE(long encaminhamentoAEEId);
        Task<EncaminhamentoAEE> ObterEncaminhamentoPorId(long id);
        Task<EncaminhamentoAEE> ObterEncaminhamentoComTurmaPorId(long encaminhamentoId);
        Task<EncaminhamentoAEEAlunoTurmaDto> ObterEncaminhamentoPorEstudante(string estudanteCodigo, string ueCodigo);
        Task<bool> VerificaSeExisteEncaminhamentoPorAluno(string codigoEstudante, long ueId);
        Task<IEnumerable<UsuarioEolRetornoDto>> ObterResponsaveis(long dreId, long ueId, long turmaId, string alunoCodigo, int anoLetivo, int? situacao, bool exibirEncerrados);
        Task<IEnumerable<AEETurmaDto>> ObterQuantidadeDeferidos(int ano, long dreId, long ueId);
        Task<DashboardAEEEncaminhamentosDto> ObterDashBoardAEEEncaminhamentos(int ano, long dreId, long ueId);
        Task<IEnumerable<EncaminhamentoAEECodigoArquivoDto>> ObterCodigoArquivoPorEncaminhamentoAEEId(long encaminhamentoId);
        Task<IEnumerable<EncaminhamentoAEEVigenteDto>> ObterEncaminhamentosVigentes(long? anoLetivo = null);
    }
}
