using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterResponsaveisEncaminhamentosAEE : AbstractUseCase, IObterResponsaveisEncaminhamentosAEE
    {
        public ObterResponsaveisEncaminhamentosAEE(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> Executar(FiltroPesquisaEncaminhamentosAEEDto filtros)
            => await mediator.Send(new ObterResponsaveisDosEncaminhamentosAEEQuery(filtros.DreId, filtros.UeId, filtros.TurmaId, filtros.AlunoCodigo, filtros.Situacao, filtros.AnoLetivo));
    }
}
