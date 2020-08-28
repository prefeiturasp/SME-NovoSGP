using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace SME.SGP.Aplicacao
{
    public class ListarObservacaoCartaIntencoesQueryHandler : IRequestHandler<ListarObservacaoCartaIntencoesQuery, IEnumerable<ListarObservacaoCartaIntencoesDto>>
    {
        private readonly IRepositorioCartaIntencoesObservacao repositorioCartaIntencoesObservacao;

        public ListarObservacaoCartaIntencoesQueryHandler(IRepositorioCartaIntencoesObservacao repositorioCartaIntencoesObservacao)
        {
            this.repositorioCartaIntencoesObservacao = repositorioCartaIntencoesObservacao ?? throw new ArgumentNullException(nameof(repositorioCartaIntencoesObservacao));
        }

        public async Task<IEnumerable<ListarObservacaoCartaIntencoesDto>> Handle(ListarObservacaoCartaIntencoesQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCartaIntencoesObservacao.ListarPorCartaIntencoesAsync(request.CartaIntencoesId, request.UsuarioLogadoId);
        }
    }
}
