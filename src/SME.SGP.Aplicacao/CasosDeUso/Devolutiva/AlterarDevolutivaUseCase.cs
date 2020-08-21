using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarDevolutivaUseCase : AbstractUseCase, IAlterarDevolutivaUseCase
    {
        public AlterarDevolutivaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AuditoriaDto> Executar(AlterarDevolutivaDto param)
        {
            Devolutiva devolutiva = await mediator.Send(new ObterDevolutivaPorIdQuery(param.Id));
            if (devolutiva == null)
            {
                throw new NegocioException("Devolutiva informada não existe");
            }

            IEnumerable<Tuple<long, DateTime>> dados = await mediator.Send(new ObterDatasEfetivasDiariosQuery(param.PeriodoInicio, param.PeriodoFim));

            if (!dados.Any())
            {
                throw new NegocioException("Diários de bordo não encontrados para atualizar Devolutiva.");
            }

            DateTime inicioEfetivo = dados.Select(x => x.Item2).Min();
            DateTime fimEfetivo = dados.Select(x => x.Item2).Max();

            IEnumerable<long> idsDiarios = dados.Select(x => x.Item1);

            AuditoriaDto auditoria = await mediator.Send(new AlterarDevolutivaCommand(devolutiva, param.CodigoComponenteCurricular, idsDiarios, inicioEfetivo, fimEfetivo, param.Descricao));

            bool diariosAtualizados = await mediator.Send(new AtualizarDiarioBordoComDevolutivaCommand(idsDiarios, auditoria.Id));

            return auditoria;
        }
    }
}
