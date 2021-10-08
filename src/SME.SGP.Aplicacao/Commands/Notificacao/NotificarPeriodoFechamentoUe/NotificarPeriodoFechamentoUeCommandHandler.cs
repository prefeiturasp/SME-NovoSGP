using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarPeriodoFechamentoUeCommandHandler : IRequestHandler<NotificarPeriodoFechamentoUeCommand, bool>
    {
        private readonly IMediator mediator;

        public NotificarPeriodoFechamentoUeCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(NotificarPeriodoFechamentoUeCommand request, CancellationToken cancellationToken)
        {
            var ues = await ObterUEsPorModalidade(request.ModalidadeTipoCalendario);

            foreach (var ue in ues)
            {
                if (!await ExistePeriodoFechamentoCadastrado(ue, request.PeriodoFechamentoBimestre))
                {
                    try
                    {
                        await NotificarUe(ue, request.ModalidadeTipoCalendario);
                    }
                    catch (Exception ex)
                    {
                        await mediator.Send(new SalvarLogViaRabbitCommand($"Erro na consolidação de Frequência da turma.", LogNivel.Negocio, LogContexto.Fechamento, ex.Message));
                    }
                }
            }

            return true;
        }

        private async Task NotificarUe(Ue ue, ModalidadeTipoCalendario modalidadeTipoCalendario)
        {
            var anoAtual = DateTime.Now.Year;
            var descricaoUe = $"{ue.TipoEscola.ShortName()} {ue.Nome} ({ue.Dre.Abreviacao})";
            var titulo = $"Cadastro de período de fechamento pendente {descricaoUe}";
            var mensagem = $"O período de fechamento da <b>{descricaoUe}</b> ainda não foi cadastrado para o tipo de calendário <b>{modalidadeTipoCalendario.Name()} {anoAtual}</b>.";

            await mediator.Send(new EnviarNotificacaoCommand(titulo, mensagem, NotificacaoCategoria.Aviso, NotificacaoTipo.Calendario, ObterCargosGestaoEscola(), ue.Dre.CodigoDre, ue.CodigoUe));

            var admins = await ObterUsuariosAdms(ue);
            if (admins != null && admins.Any())
                await mediator.Send(new EnviarNotificacaoUsuariosCommand(titulo, mensagem, NotificacaoCategoria.Aviso, NotificacaoTipo.Calendario, admins, ue.Dre.CodigoDre, ue.CodigoUe));
        }

        private async Task<IEnumerable<long>> ObterUsuariosAdms(Ue ue)
        {
            var adms = await mediator.Send(new ObterFuncionariosDreOuUePorPerfisQuery(ue.CodigoUe, ObterPerfis()));

            var listaUsuarios = new List<long>();
            foreach (var adm in adms)
                listaUsuarios.Add(await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(adm)));

            return listaUsuarios;
        }

        private IEnumerable<Guid> ObterPerfis()
        {
            return new List<Guid>() { Perfis.PERFIL_ADMUE };
        }

        private Cargo[] ObterCargosGestaoEscola()
            => new[] { Cargo.CP, Cargo.AD, Cargo.Diretor };

        private async Task<bool> ExistePeriodoFechamentoCadastrado(Ue ue, PeriodoFechamentoBimestre periodoFechamentoBimestre)
            => await mediator.Send(new ExistePeriodoFechmantoPorUePeriodoEscolarQuery(ue.Id, periodoFechamentoBimestre.PeriodoEscolarId));

        private async Task<IEnumerable<Ue>> ObterUEsPorModalidade(ModalidadeTipoCalendario modalidadeTipoCalendario)
            => await mediator.Send(new ObterUEsPorModalidadeCalendarioQuery(modalidadeTipoCalendario));
    }
}
