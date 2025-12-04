using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoDistorcaoIdade;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoPlanoAEE;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPlanoAEEConsulta : IRepositorioBase<PlanoAEE>
    {
        Task<PaginacaoResultadoDto<PlanoAEEAlunoTurmaDto>> ListarPaginado(long dreId, long[] ueId, long turmaId, string alunoCodigo, int? situacao, string[] turmasCodigos, bool ehAdmin, bool ehPAEE, Paginacao paginacao, bool exibirEncerrados, string responsavelRf, string responsavelRfPaai);

        Task<PlanoAEEResumoDto> ObterPlanoPorEstudante(FiltroEstudantePlanoAEEDto filtro);

        Task<PlanoAEE> ObterPlanoComTurmaPorId(long planoId);
        Task<IEnumerable<PlanoAEE>> ObterPlanosAtivos();
        Task<PlanoAEEResumoDto> ObterPlanoPorEstudanteEAno(string codigoEstudante, int anoLetivo);
        Task<IEnumerable<PlanoAEEResumoDto>> ObterPlanosPorAlunosEAno(string[] codigoEstudante, int ano);
        Task<IEnumerable<PlanoAEE>> ObterPorDataFinalVigencia(DateTime dataFim, bool desconsiderarPendencias = true, bool desconsiderarNotificados = false, NotificacaoPlanoAEETipo tipo = NotificacaoPlanoAEETipo.PlanoCriado);
        Task<IEnumerable<AEEAcessibilidateDto>> ObterQuantidadeAcessibilidades(int ano, long dreId, long ueId);
        Task<DashboardAEEPlanosVigentesDto> ObterDashboardPlanosVigentes(int ano, long dreId, long ueId);
        Task<DashboardAEEPlanosSituacaoDto> ObterDashBoardPlanosSituacoes(int ano, long dreId, long ueId);
        Task<IEnumerable<PlanoAEEReduzidoDto>> ObterPlanosAEEAtivosComTurmaEVigencia();
        Task<PlanoAEE> ObterPorReestruturacaoId(long reestruturacaoId);
        Task<PlanoAEE> ObterPlanoComTurmaUeDrePorId(long planoId);
        Task<IEnumerable<PlanoAEE>> ObterPlanosEncerradosAutomaticamente(int pagina, int quantidadeRegistrosPagina);
        Task<Pendencia> ObterUltimaPendenciaPlano(long planoId);
        Task<IEnumerable<UsuarioEolRetornoDto>> ObterResponsaveis(long dreId, long[] ueId, long turmaId, string alunoCodigo, int? situacao, bool exibirEncerrados);
        Task<IEnumerable<PlanoAEETurmaDto>> ObterPlanosComSituacaoDiferenteDeEncerrado(long? anoLetivo);
        Task<IEnumerable<DadosParaConsolidarPlanosAEEDto>> ObterPlanosConsolidarPainelEducacional(int anoLetivo);
        Task<IEnumerable<PainelEducacionalConsolidacaoPlanoAEE>> ObterConsolidacaoPlanosPainelEducacional(FiltroPainelEducacionalPlanosAEE filtro);
    }
}
