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
    public class ListarAlunosDaTurmaPorComponenteRegistroIndividualQueryHandler : IRequestHandler<ListarAlunosDaTurmaPorComponenteRegistroIndividualQuery, IEnumerable<AlunoDadosBasicosDto>>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioRegistroIndividual repositorioRegistroIndividual;
        private readonly IRepositorioEventoFechamento repositorioEventoFechamento;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;

        public ListarAlunosDaTurmaPorComponenteRegistroIndividualQueryHandler(IRepositorioRegistroIndividual repositorioRegistroIndividual, IMediator mediator,
                                                            IRepositorioEventoFechamento repositorioEventoFechamento, IRepositorioTipoCalendario repositorioTipoCalendario)
        {
            this.repositorioRegistroIndividual = repositorioRegistroIndividual ?? throw new System.ArgumentNullException(nameof(repositorioRegistroIndividual));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.repositorioEventoFechamento = repositorioEventoFechamento ?? throw new ArgumentNullException(nameof(repositorioEventoFechamento));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
        }

        public async Task<IEnumerable<AlunoDadosBasicosDto>> Handle(ListarAlunosDaTurmaPorComponenteRegistroIndividualQuery request, CancellationToken cancellationToken)
        {


            var periodosAberto = await repositorioEventoFechamento.ObterPeriodosFechamentoEmAberto(request.Turma.UeId, DateTime.Now.Date);

            PeriodoEscolar periodoEscolar;
            if (periodosAberto != null && periodosAberto.Any())
            {
                // caso tenha mais de um periodo em aberto (abertura e reabertura) usa o ultimo bimestre
                periodoEscolar = periodosAberto.OrderBy(c => c.Bimestre).Last();
            }
            else
            {
                // Caso não esteja em periodo de fechamento ou escolar busca o ultimo existente
                var tipoCalendario =   await repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(request.Turma.AnoLetivo, request.Turma.ModalidadeTipoCalendario, request.Turma.Semestre);
                if (tipoCalendario == null)
                    throw new NegocioException("Não foi encontrado calendário cadastrado para a turma");
                var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendario.Id));
                if (periodosEscolares == null)
                    throw new NegocioException("Não foram encontrados periodos escolares cadastrados para a turma");

                periodoEscolar = periodosEscolares?.FirstOrDefault(p => p.DataDentroPeriodo(DateTime.Today));
                if (periodoEscolar == null)
                    periodoEscolar = periodosEscolares.OrderByDescending(o => o.PeriodoInicio).FirstOrDefault(p => p.PeriodoFim <= DateTime.Today);
            }

            var dadosAlunos = await mediator.Send(new ObterDadosAlunosQuery(request.Turma.CodigoTurma, request.Turma.AnoLetivo, periodoEscolar));

            return dadosAlunos.OrderBy(w => w.Nome);
        }
    }
}
