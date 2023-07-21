using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP
{
    public class ConsolidarFrequenciaPorTurmaAnualUseCase : ConsolidarFrequenciaPorTurmaAbstractUseCase, IConsolidarFrequenciaPorTurmaAnualUseCase
    {
        public ConsolidarFrequenciaPorTurmaAnualUseCase(IMediator mediator) : base(mediator)
        {
        }

        protected override TipoConsolidadoFrequencia TipoConsolidado => TipoConsolidadoFrequencia.Anual;

        protected override (DateTime?, DateTime?) Periodos => (null, null);

        protected override async Task<IEnumerable<FrequenciaAlunoDto>> ObterFrequenciaConsideradas(string codigoTurma)
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia();
            var alunos = await mediator.Send(new ObterAlunosDentroPeriodoQuery(codigoTurma, (dataAtual, dataAtual)));
            var frequenciaTurma = await mediator.Send(new ObterFrequenciaGeralPorTurmaQuery(codigoTurma));

            return from ft in frequenciaTurma
                   join a in alunos
                    on ft.AlunoCodigo equals a.CodigoAluno
                    where (AnoAnterior && !a.Inativo && a.DataMatricula.Date <= ft.PeriodoFim.Date) ||
                        (!AnoAnterior && a.DataMatricula.Date <= ft.PeriodoFim.Date)
                    select ft;
        }
    }
}
