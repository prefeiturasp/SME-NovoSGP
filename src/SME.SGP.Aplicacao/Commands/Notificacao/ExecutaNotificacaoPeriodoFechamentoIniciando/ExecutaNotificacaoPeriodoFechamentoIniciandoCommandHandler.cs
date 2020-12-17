using MediatR;
using SME.SGP.Aplicacao.Queries.Funcionario;
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
    public class ExecutaNotificacaoPeriodoFechamentoIniciandoCommandHandler : IRequestHandler<ExecutaNotificacaoPeriodoFechamentoIniciandoCommand, bool>
    {
        private readonly IMediator mediator;

        public ExecutaNotificacaoPeriodoFechamentoIniciandoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExecutaNotificacaoPeriodoFechamentoIniciandoCommand request, CancellationToken cancellationToken)
        {
            await NotificarUe(request.PeriodoFechamentoBimestre.PeriodoFechamento.Ue, request.PeriodoFechamentoBimestre.PeriodoEscolar, request.PeriodoFechamentoBimestre);
            return true;
        }

        private async Task NotificarUe(Ue ue, PeriodoEscolar periodoEscolar, PeriodoFechamentoBimestre periodoFechamentoBimestre)
        {
            var descricaoUe = $"{ue.TipoEscola.ShortName()} {ue.Nome} ({ue.Dre.Abreviacao})";
            var titulo = $"Início do período de fechamento do {periodoEscolar.Bimestre}º bimestre - {descricaoUe}";
            var mensagem = $@"O fechamento do <b>{periodoEscolar.Bimestre}º bimestre</b> na <b>{descricaoUe}</b> irá iniciar no dia <b>{periodoFechamentoBimestre.InicioDoFechamento.Date.ToString("dd/MM/yyyy")}</b>.";

            var usuarios = await ObterUsuarios(ue);
            if (usuarios != null && usuarios.Any())
                await mediator.Send(new EnviarNotificacaoUsuariosCommand(titulo, mensagem, NotificacaoCategoria.Aviso, NotificacaoTipo.Calendario, usuarios, ue.Dre.CodigoDre, ue.CodigoUe));

            await mediator.Send(new EnviarNotificacaoCommand(titulo, mensagem.ToString(), NotificacaoCategoria.Aviso, NotificacaoTipo.Calendario, ObterCargosGestaoEscola(),
            periodoFechamentoBimestre.PeriodoFechamento.Ue.Dre.CodigoDre,
            periodoFechamentoBimestre.PeriodoFechamento.Ue.CodigoUe));
        }

        private async Task<IEnumerable<long>> ObterUsuarios(Ue ue)
        {
            var listaUsuarios = new List<long>();

            var usuariosAdm = await ObterUsuariosAdms(ue);
            if (usuariosAdm != null && usuariosAdm.Any())
                listaUsuarios.AddRange(usuariosAdm);

            var usuariosProfessores = await ObterProfessores(ue);
            if (usuariosProfessores != null && usuariosProfessores.Any())
                listaUsuarios.AddRange(usuariosProfessores);

            return listaUsuarios.Distinct();
        }

        private Cargo[] ObterCargosGestaoEscola()
       => new[] { Cargo.CP, Cargo.AD, Cargo.Diretor };


        private async Task<IEnumerable<long>> ObterProfessores(Ue ue)
        {

            var professores = await mediator.Send(new ObterProfessoresDreOuUeAnoLetivoQuery(ue.CodigoUe, DateTime.Now.Year));

            var listaUsuarios = new List<long>();
            foreach (var professor in professores.Select(c => c.CodigoRF).Distinct())
            {
                if (professor != "")
                    listaUsuarios.Add(await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(professor)));
            }

            return listaUsuarios.Distinct();
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
    }
}
