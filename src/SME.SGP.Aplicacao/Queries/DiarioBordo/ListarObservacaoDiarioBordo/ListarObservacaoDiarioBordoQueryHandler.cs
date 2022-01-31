using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var consulta = await repositorioDiarioBordoObservacao.ListarPorDiarioBordoAsync(request.DiarioBordoId, request.UsuarioLogadoId);
            return await ObterUsuariosNotificados(consulta);
        }
        private async Task<IEnumerable<ListarObservacaoDiarioBordoDto>> ObterUsuariosNotificados(IEnumerable<ListarObservacaoDiarioBordoDto> observacoes)
        {
            var listaObservacoes = new List<ListarObservacaoDiarioBordoDto>();
            foreach (var item in observacoes)
            {
                var usuariosNotificados = await repositorioDiarioBordoObservacao.ObterNomeUsuariosNotificadosObservacao(item.Id);
                item.QtdUsuariosNotificados = usuariosNotificados.Count();
                item.NomeUsuariosNotificados = string.Join(",", usuariosNotificados);
                listaObservacoes.Add(item);
            }

            return listaObservacoes;
        }
    }
}
