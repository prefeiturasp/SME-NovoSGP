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
                var periodosOrigem = await mediator.Send(new ObterPlanejamentoAnualPeriodosEscolaresCompletoPorIdQuery(comando.Planejamento.PlanejamentoPeriodosEscolaresIds.ToArray()));
                var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

                List<string> excessoes = new List<string>();

                if (!periodosOrigem.Any())
                    throw new NegocioException($"Nenhum período foi encontrado");

                // Validando as turmas
                foreach (var turma in comando.Planejamento.TurmasDestinoIds)
                {
                    Turma checarTurma;
                    if (usuario.PerfilAtual == Perfis.PERFIL_CP)
                        checarTurma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(turma));
                    else
                        checarTurma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turma.ToString()));

                    if (checarTurma == null)
                        throw new NegocioException($"Turma não encontrada");

                    foreach (var periodoOrigem in periodosOrigem)
                    {
                        var periodo = await mediator.Send(new ObterPeriodoEscolarePorIdQuery(periodoOrigem.PeriodoEscolarId));

                        if (usuario.EhProfessor())
                        {
                            var temAtribuicao = await mediator.Send(new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaNoPeriodoQuery(comando.Planejamento.ComponenteCurricularId, checarTurma.CodigoTurma, usuario.CodigoRf, periodo.PeriodoInicio.Date, periodo.PeriodoFim.Date));
                            if (!temAtribuicao)
                                excessoes.Add($"Você não possui atribuição na turma {checarTurma.Nome} - {periodo.Bimestre}° Bimestre.");
                        }

                        var periodoEmAberto = mediator.Send(new TurmaEmPeriodoAbertoQuery(checarTurma, DateTime.Today, periodo.Bimestre, checarTurma.AnoLetivo == DateTime.Today.Year)).Result;
                        if (!periodoEmAberto)
                            excessoes.Add($"O {periodo.Bimestre}° Bimestre não está aberto.");
                    }

                    var planejamentoCopiado = new PlanejamentoAnual(checarTurma.Id, comando.Planejamento.ComponenteCurricularId);
                    planejamentoCopiado.PeriodosEscolares.AddRange(periodosOrigem);

                    if (!excessoes.Any())
                        await mediator.Send(new SalvarCopiaPlanejamentoAnualCommand(planejamentoCopiado));

                }

                if (excessoes.Any())
                {
                    var str = new StringBuilder();
                    str.AppendLine($"Os seguintes erros foram encontrados: ");
                    foreach (var t in excessoes)
                        str.AppendLine($"- {t}");

                    throw new NegocioException(str.ToString());
                }

                unitOfWork.PersistirTransacao();
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }

            return true;
        }
    }
}
