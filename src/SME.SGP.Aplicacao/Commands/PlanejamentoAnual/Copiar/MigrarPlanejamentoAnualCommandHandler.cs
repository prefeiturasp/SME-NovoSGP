using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utilities;

namespace SME.SGP.Aplicacao
{
    public class MigrarPlanejamentoAnualCommandHandler : AbstractUseCase, IRequestHandler<MigrarPlanejamentoAnualCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        public MigrarPlanejamentoAnualCommandHandler(IMediator mediator,
                                              IUnitOfWork unitOfWork) : base(mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new System.ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(MigrarPlanejamentoAnualCommand comando, CancellationToken cancellationToken)
        {
            unitOfWork.IniciarTransacao();

            try
            {
                var periodosOrigem = await ObterPlanejamentoAnualPeriodosEscolares(comando.Planejamento.PlanejamentoPeriodosEscolaresIds.ToArray());
                var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
                List<string> excessoes = new List<string>();

                // Validando as turmas
                foreach (var turma in comando.Planejamento.TurmasDestinoIds)
                {
                    Turma checarTurma = await ObterTurma(turma, usuario.PerfilAtual);
                    foreach (var periodoOrigem in periodosOrigem)
                    {
                        var periodo = await mediator.Send(new ObterPeriodoEscolarePorIdQuery(periodoOrigem.PeriodoEscolarId));
                        await AvaliarExcecaoProfessorSemAtribuicao(usuario, checarTurma, comando.Planejamento.ComponenteCurricularId, periodo, excessoes);
                        await AvaliarExcecaoPeriodoAberto(checarTurma, periodo, excessoes);
                    }

                    var planejamentoCopiado = new PlanejamentoAnual(checarTurma.Id, comando.Planejamento.ComponenteCurricularId);
                    planejamentoCopiado.PeriodosEscolares.AddRange(periodosOrigem);

                    if (!excessoes.Any())
                        await mediator.Send(new SalvarCopiaPlanejamentoAnualCommand(planejamentoCopiado));

                }

                TratarLancamentoExcecoes(excessoes);
                unitOfWork.PersistirTransacao();
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }

            return true;
        }

        private void TratarLancamentoExcecoes(List<string> excessoes)
        {
            if (excessoes.Any())
            {
                var str = new StringBuilder();
                str.AppendLine($"Os seguintes erros foram encontrados: ");
                foreach (var t in excessoes)
                    str.AppendLine($"- {t}");

                throw new NegocioException(str.ToString());
            }
        }

        private async Task AvaliarExcecaoPeriodoAberto(Turma turma, PeriodoEscolar periodo, List<string> excessoes)
        {
            var periodoEmAberto = await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, periodo.Bimestre, turma.AnoLetivo == DateTime.Today.Year));
            if (!periodoEmAberto)
                excessoes.Add($"O {periodo.Bimestre}° Bimestre não está aberto.");
        }

        private async Task AvaliarExcecaoProfessorSemAtribuicao(Usuario usuario, Turma turma, long componenteCurricularId,
                                                                PeriodoEscolar periodo, List<string> excessoes)
        {
            if (usuario.EhProfessor())
            {
                var temAtribuicao = await mediator.Send(new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaNoPeriodoQuery(componenteCurricularId,
                                                            turma.CodigoTurma, usuario.CodigoRf, 
                                                            periodo.PeriodoInicio.Date, periodo.PeriodoFim.Date));
                if (!temAtribuicao)
                    excessoes.Add($"Você não possui atribuição na turma {turma.Nome} - {periodo.Bimestre}° Bimestre.");
            }
        }

        private async Task<Turma> ObterTurma(long codigoIdTurma, Guid perfilUsuario)
        {
            if (perfilUsuario == Perfis.PERFIL_CP)
                return await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(codigoIdTurma)) ?? throw new NegocioException($"Turma não encontrada"); ;
            return await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(codigoIdTurma.ToString())) ?? throw new NegocioException($"Turma não encontrada"); ;

        }

        private async Task<IEnumerable<PlanejamentoAnualPeriodoEscolar>> ObterPlanejamentoAnualPeriodosEscolares(long[] ids)
        {
            var periodos = await mediator.Send(new ObterPlanejamentoAnualPeriodosEscolaresCompletoPorIdQuery(ids));
            if (periodos.NaoPossuiRegistros()) 
                throw new NegocioException($"Nenhum período foi encontrado");
            return periodos;
        }
    }
}
