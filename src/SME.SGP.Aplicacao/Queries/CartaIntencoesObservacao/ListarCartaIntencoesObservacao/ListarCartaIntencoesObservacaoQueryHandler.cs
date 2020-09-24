using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace SME.SGP.Aplicacao
{
    public class ListarObservacaoCartaIntencoesQueryHandler : IRequestHandler<ListarCartaIntencoesObservacaoQuery, IEnumerable<CartaIntencoesObservacaoDto>>
    {
        private readonly IRepositorioCartaIntencoesObservacao repositorioCartaIntencoesObservacao;

        public ListarObservacaoCartaIntencoesQueryHandler(IRepositorioCartaIntencoesObservacao repositorioCartaIntencoesObservacao)
        {
            this.repositorioCartaIntencoesObservacao = repositorioCartaIntencoesObservacao ?? throw new ArgumentNullException(nameof(repositorioCartaIntencoesObservacao));
        }

        public async Task<IEnumerable<CartaIntencoesObservacaoDto>> Handle(ListarCartaIntencoesObservacaoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCartaIntencoesObservacao.ListarPorTurmaEComponenteCurricularAsync(request.TurmaId, request.ComponenteCurricularId, request.UsuarioLogadoId);
        }
    }
}
