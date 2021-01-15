using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarEventoCommandHandler : IRequestHandler<NotificarEventoCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IServicoEol servicoEol;

        public NotificarEventoCommandHandler(IMediator mediator, IServicoEol servicoEol)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
        }

        public async Task<bool> Handle(NotificarEventoCommand request, CancellationToken cancellationToken)
        {
            var evento = request.Evento;

            var descricaoUe = $"{evento.Ue.TipoEscola.ShortName()} {evento.Ue.Nome} ({evento.Dre.Abreviacao})";

            var titulo = $"{evento.TipoEvento.Descricao} ({evento.DataInicio:dd/MM/yyyy}) - {descricaoUe}";
            var mensagem = $"A {evento.TipoEvento.Descricao} da <b>{descricaoUe}</b> ocorrerá no dia <b>{evento.DataInicio:dd/MM/yyyy}</b>."
                + $"<br/><br/>Pauta:<br/>{evento.Descricao}";

            await mediator.Send(new EnviarNotificacaoUsuariosCommand(titulo,
                                                                     mensagem,
                                                                     NotificacaoCategoria.Aviso,
                                                                     NotificacaoTipo.Calendario,
                                                                     await ObterFuncionarios(evento.Ue),
                                                                     evento.Dre.CodigoDre,
                                                                     evento.Ue.CodigoUe));

            return true;
        }

        private async Task<IEnumerable<long>> ObterFuncionarios(Ue ue)
        {
            var funcionarios = await servicoEol.ObterFuncionariosPorUe(new BuscaFuncionariosFiltroDto()
            {
                CodigoUE = ue.CodigoUe
            });

            var listaUsuarios = new List<long>();
            foreach (var funcionario in funcionarios)
            {
                listaUsuarios.Add(await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(funcionario.CodigoRf)));
            }

            return listaUsuarios;
        }
    }
}
