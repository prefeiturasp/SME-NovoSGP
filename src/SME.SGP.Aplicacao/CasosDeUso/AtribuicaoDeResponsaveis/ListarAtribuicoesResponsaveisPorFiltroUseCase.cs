using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ListarAtribuicoesResponsaveisPorFiltroUseCase : AbstractUseCase, IListarAtribuicoesResponsaveisPorFiltroUseCase
    {
        public ListarAtribuicoesResponsaveisPorFiltroUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<IEnumerable<AtribuicaoResponsavelDto>> Executar(AtribuicaoResponsaveisFiltroDto param)
        {
            return await mediator.Send(new ObterAtribuicaoResponsaveisPorUeTipoQuery(param.UeCodigo, param.Tipo));
        }
    }
}
