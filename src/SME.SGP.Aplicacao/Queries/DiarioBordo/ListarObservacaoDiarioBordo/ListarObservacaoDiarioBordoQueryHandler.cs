using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarObservacaoDiarioBordoQueryHandler : IRequestHandler<ListarObservacaoDiarioBordoQuery, IEnumerable<ListarObservacaoDiarioBordoDto>>
    {
        private readonly IRepositorioDiarioBordoObservacao repositorioDiarioBordoObservacao;

        public ListarObservacaoDiarioBordoQueryHandler(IRepositorioDiarioBordoObservacao repositorioDiarioBordoObservacao)
        {
            this.repositorioDiarioBordoObservacao = repositorioDiarioBordoObservacao ?? throw new ArgumentNullException(nameof(repositorioDiarioBordoObservacao));
        }

        public async Task<IEnumerable<ListarObservacaoDiarioBordoDto>> Handle(ListarObservacaoDiarioBordoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioDiarioBordoObservacao.ListarPorDiarioBordoAsync(request.DiarioBordoId, request.UsuarioLogadoId);
        }
    }
}
