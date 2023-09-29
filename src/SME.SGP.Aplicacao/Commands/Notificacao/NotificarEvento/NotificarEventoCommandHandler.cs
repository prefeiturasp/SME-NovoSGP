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

        public NotificarEventoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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
            var funcionarios = await mediator.Send(new ObterFuncionariosPorRfUeNomeServidorQuery(string.Empty,ue.CodigoUe, string.Empty));

            var listaUsuarios = new List<long>();
            foreach (var funcionario in funcionarios)
                listaUsuarios.Add(await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(funcionario.CodigoRf)));

            return listaUsuarios;
        }
    }
}
