using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioEncaminhamentoAEE : IRepositorioBase<EncaminhamentoAEE>
    {
        Task<PaginacaoResultadoDto<EncaminhamentoAEEAlunoTurmaDto>> ListarPaginado(long dreId, long ueId, long turmaId, string alunoCodigo, int? situacao, string responsavelRf, int anoLetivo, Paginacao paginacao);
        Task<SituacaoAEE> ObterSituacaoEncaminhamentoAEE(long encaminhamentoAEEId);
        Task<EncaminhamentoAEE> ObterEncaminhamentoPorId(long id);
        Task<EncaminhamentoAEE> ObterEncaminhamentoComTurmaPorId(long encaminhamentoId);
        Task<EncaminhamentoAEEAlunoTurmaDto> ObterEncaminhamentoPorEstudante(string codigoEstudante);
        Task<bool> VerificaSeExisteEncaminhamentoPorAluno(string codigoEstudante);
        Task<IEnumerable<UsuarioEolRetornoDto>> ObterResponsaveis(long dreId, long ueId, long turmaId, string alunoCodigo, int anoLetivo, int? situacao);
        Task<IEnumerable<AEETurmaDto>> ObterQuantidadeDeferidos(int ano, long dreId, long ueId);
        Task<IEnumerable<AEESituacaoEncaminhamentoDto>> ObterQuantidadeSituacoes(int ano, long dreId, long ueId);
        Task<IEnumerable<EncaminhamentoAEECodigoArquivoDto>> ObterCodigoArquivoPorEncaminhamentoAEEId(long encaminhamentoId);
    }
}
