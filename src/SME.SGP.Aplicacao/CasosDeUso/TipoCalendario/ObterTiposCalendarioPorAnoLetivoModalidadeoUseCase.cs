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
    public class ObterTiposCalendarioPorAnoLetivoModalidadeUseCase : IObterTiposCalendarioPorAnoLetivoModalidadeUseCase
    {
        private readonly IMediator mediator;

        public ObterTiposCalendarioPorAnoLetivoModalidadeUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<TipoCalendarioDto>> Executar(int anoLetivo, string modalidades)
        {
            return (await mediator.Send(new ObterTiposCalendarioPorAnoLetivoModalidadeQuery(anoLetivo, modalidades)))
                .Where(tc => tc.Situacao && !tc.Excluido)
                .Select(t => new TipoCalendarioDto
                {
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