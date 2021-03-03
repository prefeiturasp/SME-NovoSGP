using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class RelatorioAcompanhamentoAprendizagemObterFrequenciaUseCase : IRelatorioAcompanhamentoAprendizagemObterFrequenciaUseCase
    {
        private readonly IMediator mediator;

        public RelatorioAcompanhamentoAprendizagemObterFrequenciaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(long turmaId, int semestre, string codigoAluno)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(turmaId));
            var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery(Modalidade.Infantil, DateTime.Now.Year, 1));
            var tipoCalendarioId = await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma));

            if (!periodosEscolares.Any())
                throw new NegocioException("Não foi encontrado nenhum periodo escolar cadastrado para a turma selecionada");

            var periodosEscolaresId = semestre == 1 ? periodosEscolares.Where(a => a.Bimestre == 1 || a.Bimestre == 2).Select(a => a.Id).ToList() : periodosEscolares.Where(a => a.Bimestre == 3 || a.Bimestre == 4).Select(a => a.Id).ToList();

            var qtdAulasComFrequenciaRegistrada = await mediator.Send(new ObterAulasDadasPorTurmaIdEPeriodoEscolarQuery(turmaId, periodosEscolaresId, tipoCalendarioId));

            return true;
        }
    }
}
