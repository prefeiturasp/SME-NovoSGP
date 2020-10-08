using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class MigrarPlanejamentoAnualCommandHandler : AbstractUseCase, IRequestHandler<MigrarPlanejamentoAnualCommand, bool>
    {
        public MigrarPlanejamentoAnualCommandHandler(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Handle(MigrarPlanejamentoAnualCommand comando, CancellationToken cancellationToken)
        {

            var periodosOrigem = await mediator.Send(new ObterPlanejamentoAnualPeriodosEscolaresCompletoPorIdQuery(comando.Planejamento.PlanejamentoPeriodosEscolaresIds.ToArray()));
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

            if (!periodosOrigem.Any())
                throw new NegocioException($"Nenhum período foi encontrado");

            // Validando as turmas
            foreach (var turma in comando.Planejamento.TurmasDestinoIds)
            {
                var checarTurma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(turma));
                if (checarTurma == null)
                    throw new NegocioException($"Turma não encontrada");

                List<PeriodoEscolar> excecoesAtribuicao = new List<PeriodoEscolar>();
                List<PeriodoEscolar> excecoesEmAberto = new List<PeriodoEscolar>();

                foreach (var periodoOrigem in periodosOrigem)
                {
                    var periodo = await mediator.Send(new ObterPeriodoEscolarePorIdQuery(periodoOrigem.PeriodoEscolarId));
                    var temAtribuicao = await mediator.Send(new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaNoPeriodoQuery(comando.Planejamento.ComponenteCurricularId, checarTurma.CodigoTurma, usuario.CodigoRf, periodo.PeriodoInicio.Date, periodo.PeriodoFim.Date));
                    if (!temAtribuicao)
                        excecoesAtribuicao.Add(periodo);

                    var periodoEmAberto = mediator.Send(new TurmaEmPeriodoAbertoQuery(checarTurma, DateTime.Today, periodo.Bimestre, checarTurma.AnoLetivo == DateTime.Today.Year)).Result;
                    if (!periodoEmAberto)
                        excecoesEmAberto.Add(periodo);
                }

                if (excecoesAtribuicao.Any())
                    throw new NegocioException($"Você não possui atribuição.");

                if (excecoesEmAberto.Any())
                    throw new NegocioException($"Algum bimestre não está com o período aberto.");

                var planejamentoCopiado = new PlanejamentoAnual(checarTurma.Id, comando.Planejamento.ComponenteCurricularId);
                planejamentoCopiado.PeriodosEscolares.AddRange(periodosOrigem);

                await mediator.Send(new SalvarCopiaPlanejamentoAnualCommand(planejamentoCopiado));
            }
            return true;
        }
    }
}
