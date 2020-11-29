using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarPeriodoFechamentoUeCommandHandler : IRequestHandler<NotificarPeriodoFechamentoUeCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IServicoEol servicoEol;

        public NotificarPeriodoFechamentoUeCommandHandler(IMediator mediator, IServicoEol servicoEol)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
        }

        public async Task<bool> Handle(NotificarPeriodoFechamentoUeCommand request, CancellationToken cancellationToken)
        {
            var ues = await ObterUEsPorModalidade(request.ModalidadeTipoCalendario);

            foreach(var ue in ues)
            {
                if (!await ExistePeriodoFechamentoCadastrado(ue, request.PeriodoFechamentoBimestre))
                    await NotificarUe(ue, request.ModalidadeTipoCalendario);
            }

            return true;
        }

        private async Task NotificarUe(Ue ue, ModalidadeTipoCalendario modalidadeTipoCalendario)
        {
            var descricaoUe = $"{ue.TipoEscola.ShortName()} {ue.Nome} ({ue.Dre.Abreviacao})";
            var titulo = $"Cadastro de período de fechamento pendente {descricaoUe}";
            var mensagem = $"O período de fechamento da <b>{descricaoUe}</b> ainda não foi cadastrado para o tipo de calendário <b>{modalidadeTipoCalendario.Name()}</b>.";

            await mediator.Send(new EnviarNotificacaoCommand(titulo, mensagem, NotificacaoCategoria.Aviso, NotificacaoTipo.Calendario, ObterCargosGestaoEscola(), ue.Dre.CodigoDre, ue.CodigoUe));

            var admins = await ObterUsuariosAdms(ue);
            if (admins != null && admins.Any())
                await mediator.Send(new EnviarNotificacaoUsuariosCommand(titulo, mensagem, NotificacaoCategoria.Aviso, NotificacaoTipo.Calendario, admins, ue.Dre.CodigoDre, ue.CodigoUe));
        }

        private async Task<IEnumerable<long>> ObterUsuariosAdms(Ue ue)
        {
            var adms = await mediator.Send(new ObterAdministradoresPorUEQuery(ue.CodigoUe));

            var listaUsuarios = new List<long>();
            foreach (var adm in adms)
                listaUsuarios.Add(await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(adm)));

            return listaUsuarios;
        }

        private Cargo[] ObterCargosGestaoEscola()
            => new[] { Cargo.CP, Cargo.AD, Cargo.Diretor };

        private async Task<bool> ExistePeriodoFechamentoCadastrado(Ue ue, PeriodoFechamentoBimestre periodoFechamentoBimestre)
            =>  await mediator.Send(new ExistePeriodoFechmantoPorUePeriodoEscolarQuery(ue.Id, periodoFechamentoBimestre.PeriodoEscolarId));

        private async Task<IEnumerable<Ue>> ObterUEsPorModalidade(ModalidadeTipoCalendario modalidadeTipoCalendario)
            => await mediator.Send(new ObterUEsPorModalidadeCalendarioQuery(modalidadeTipoCalendario));
    }
}
