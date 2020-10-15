using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTiposCalendarioPorAnoLetivoModalidadeoUseCase : IObterTiposCalendarioPorAnoLetivoModalidadeoUseCase
    {
        private readonly IMediator mediator;

        public ObterTiposCalendarioPorAnoLetivoModalidadeoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<TipoCalendarioDto>> Executar(int anoLetivo, Modalidade modalidade)
        {
            return (await mediator.Send(new ObterTiposCalendarioPorAnoLetivoModalidadeoQuery(anoLetivo, modalidade))).Select(t => new TipoCalendarioDto { 
                Id = t.Id,
                Modalidade = t.Modalidade,
                AnoLetivo = t.AnoLetivo,
                Nome = t.Nome,
                Periodo = t.Periodo,
                Situacao = t.Situacao,
            });
        }
    }
}
