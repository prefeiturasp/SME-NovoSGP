using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirDevolutivaUseCase : AbstractUseCase, IInserirDevolutivaUseCase
    {
        public InserirDevolutivaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AuditoriaDto> Executar(InserirDevolutivaDto param)
        {
            IEnumerable<Tuple<long, DateTime>> dados = await mediator.Send(new ObterDatasEfetivasDiariosQuery(param.PeriodoInicio, param.PeriodoFim));

            DateTime inicioEfetivo = dados.Select(x => x.Item2).Min();
            DateTime fimEfetivo = dados.Select(x => x.Item2).Max();

            IEnumerable<long> idsDiarios = dados.Select(x => x.Item1);

            AuditoriaDto auditoria = await mediator.Send(new InserirDevolutivaCommand(param.CodigoComponenteCurricular, idsDiarios, inicioEfetivo, fimEfetivo, param.Descricao));

            bool diariosAtualizados = await mediator.Send(new AtualizarDiarioBordoComDevolutivaCommand(param.DiariosBordoIds, auditoria.Id));

            return auditoria;
        }
    }
}
