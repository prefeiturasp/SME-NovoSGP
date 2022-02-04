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
    public class ObterDiarioDeBordoPorIdQueryHandler : IRequestHandler<ObterDiarioDeBordoPorIdQuery, DiarioBordoDetalhesDto>
    {
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;
        private readonly IRepositorioDiarioBordoObservacao repositorioDiarioBordoObservacao;
        private readonly IMediator mediator;

        public ObterDiarioDeBordoPorIdQueryHandler(IRepositorioDiarioBordo repositorioDiarioBordo, IMediator mediator, IRepositorioDiarioBordoObservacao repositorioDiarioBordoObservacao)
        {
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
            this.repositorioDiarioBordoObservacao = repositorioDiarioBordoObservacao ?? throw new ArgumentNullException(nameof(repositorioDiarioBordoObservacao));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<DiarioBordoDetalhesDto> Handle(ObterDiarioDeBordoPorIdQuery request, CancellationToken cancellationToken)
        {
            var diarioBordo = await repositorioDiarioBordo.ObterPorIdAsync(request.Id);
            var usuario = await mediator.Send(new ObterUsuarioLogadoIdQuery());

            var observacoes = await mediator.Send(new ListarObservacaoDiarioBordoQuery(diarioBordo.Id, usuario));
            var observacoesComUsuariosNotificados = await ObterUsuariosNotificados(observacoes);
            return MapearParaDto(diarioBordo, observacoesComUsuariosNotificados);
        }

        private async Task<IEnumerable<ListarObservacaoDiarioBordoDto>> ObterUsuariosNotificados(IEnumerable<ListarObservacaoDiarioBordoDto> observacoes)
        {
            var listaObservacoes = new List<ListarObservacaoDiarioBordoDto>();
            foreach (var item in observacoes)
            {
                var usuariosNotificados = await repositorioDiarioBordoObservacao.ObterNomeUsuariosNotificadosObservacao(item.Id);
                item.QtdUsuariosNotificados = usuariosNotificados.Count();
                item.NomeUsuariosNotificados = string.Join(", ", usuariosNotificados);
                listaObservacoes.Add(item);
            }

            return listaObservacoes;
        }
        private DiarioBordoDetalhesDto MapearParaDto(Dominio.DiarioBordo diarioBordo, IEnumerable<ListarObservacaoDiarioBordoDto> observacoes)
        {
            return new DiarioBordoDetalhesDto()
            {
                Auditoria = (AuditoriaDto)diarioBordo,
                AulaId = diarioBordo.AulaId,
                DevolutivaId = diarioBordo.DevolutivaId,
                Excluido = diarioBordo.Excluido,
                Id = diarioBordo.Id,
                Migrado = diarioBordo.Migrado,
                Planejamento = diarioBordo.Planejamento,
                ReflexoesReplanejamento = diarioBordo.ReflexoesReplanejamento,
                InseridoCJ = diarioBordo.InseridoCJ,
                Observacoes = observacoes.Select(obs =>
                {
                    return new ObservacaoNotificacoesDiarioBordoDto()
                    {
                        Auditoria = obs.Auditoria,
                        Id = obs.Id,
                        Observacao = obs.Observacao,
                        QtdUsuariosNotificacao = obs.QtdUsuariosNotificados,
                        NomeUsuariosNotificados = obs.NomeUsuariosNotificados,
                        Proprietario = obs.Proprietario
                    };
                })
            };
        }
    }
}

