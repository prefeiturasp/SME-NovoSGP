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
    public class ObterDadosAlunosFechamentoQueryHandler : IRequestHandler<ObterDadosAlunosFechamentoQuery, IEnumerable<AlunoDadosBasicosDto>>
    {
        private const int PRIMEIRO_BIMESTRE = 1;

        private readonly IMediator mediator;
        private readonly IRepositorioEventoFechamentoConsulta repositorioEventoFechamento;

        public ObterDadosAlunosFechamentoQueryHandler(IMediator mediator, IRepositorioEventoFechamentoConsulta repositorioEventoFechamento)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioEventoFechamento = repositorioEventoFechamento ?? throw new ArgumentNullException(nameof(repositorioEventoFechamento));
        }

        public async Task<IEnumerable<AlunoDadosBasicosDto>> Handle(ObterDadosAlunosFechamentoQuery request, CancellationToken cancellationToken)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(request.TurmaCodigo));
            var periodosAberto = await repositorioEventoFechamento.ObterPeriodosFechamentoEmAberto(turma.UeId, DateTime.Now.Date, turma.AnoLetivo);
            var tipoCalendario = await mediator.Send(new ObterTipoDeCalendarioDaTurmaQuery { Turma = turma });

            if (tipoCalendario == null)
                throw new NegocioException("Não foi encontrado calendário cadastrado para a turma");

            var periodosEscolares = await mediator.Send(new ObterPeridosEscolaresPorTipoCalendarioIdQuery(tipoCalendario.Id));

            if (periodosEscolares == null)
                throw new NegocioException("Não foram encontrados periodos escolares cadastrados para a turma");

            DateTime primeiroPeriodoDoCalendario = periodosEscolares.Where(p => p.Bimestre == PRIMEIRO_BIMESTRE).Select(pe => pe.PeriodoInicio).FirstOrDefault();


            PeriodoEscolar periodoEscolar;
            if (periodosAberto != null && periodosAberto.Any())
            {
                // caso tenha mais de um periodo em aberto (abertura e reabertura) usa o ultimo bimestre
                periodoEscolar = periodosAberto.OrderBy(c => c.Bimestre).Last();
            }
            else
            {
                // Caso não esteja em periodo de fechamento ou escolar busca o ultimo existente

                periodoEscolar = periodosEscolares?.FirstOrDefault(p => p.DataDentroPeriodo(DateTimeExtension.HorarioBrasilia().Date));

                if (periodoEscolar == null)
                    periodoEscolar = periodosEscolares.OrderByDescending(o => o.PeriodoInicio).FirstOrDefault(p => p.PeriodoFim <= DateTimeExtension.HorarioBrasilia().Date);
            }

            var dadosAlunos = await mediator.Send(new ObterDadosAlunosTurmaQuery(request.TurmaCodigo, request.AnoLetivo, periodoEscolar, turma.EhTurmaInfantil));

            var dadosAlunosFiltrados = dadosAlunos.Where(d => !d.EstaInativo() || d.EstaInativo() && d.DataSituacao >= primeiroPeriodoDoCalendario).OrderBy(d => d.Nome);

            return dadosAlunosFiltrados.OrderBy(aluno => aluno.CodigoEOL)
                                       .ThenByDescending(aluno => aluno.DataSituacao)
                                       .GroupBy(aluno => aluno.CodigoEOL)
                                       .Select(aluno => aluno.First())
                                       .OrderBy(aluno => aluno.Nome)
                                       .ToList();
        }
    }
}
