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
    public class ExecutaNotificacaoPeriodoFechamentoEncerrandoCommandHandler : IRequestHandler<ExecutaNotificacaoPeriodoFechamentoEncerrandoCommand, bool>
    {
        private readonly IMediator mediator;

        public ExecutaNotificacaoPeriodoFechamentoEncerrandoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExecutaNotificacaoPeriodoFechamentoEncerrandoCommand request, CancellationToken cancellationToken)
        {
            var turmas = await mediator.Send(new ObterTurmasComFechamentoOuConselhoNaoFinalizadosQuery(request.PeriodoFechamentoBimestre.PeriodoFechamento.UeId.Value,
                                                                                                        DateTime.Now.Year,
                                                                                                      request.PeriodoFechamentoBimestre.PeriodoFechamentoId,
                                                                                                      request.ModalidadeTipoCalendario.ObterModalidadesTurma(), 0));
            if (turmas != null && turmas.Any())
                await EnviarNotificacaoProfessores(turmas, request.PeriodoFechamentoBimestre.PeriodoEscolar, request.PeriodoFechamentoBimestre, request.PeriodoFechamentoBimestre.PeriodoFechamento.Ue);

            return true;
        }

        private async Task EnviarNotificacaoProfessores(IEnumerable<Turma> turmas, PeriodoEscolar periodoEscolar, PeriodoFechamentoBimestre periodoFechamentoBimestre, Ue ue)
        {
            var descricaoUe = $"{ue.TipoEscola.ShortName()} {ue.Nome} ({ue.Dre.Abreviacao})";
            var titulo = $"Término do período de fechamento do  {periodoEscolar.Bimestre}º bimestre - {descricaoUe}";
            var mensagem = $@"O fechamento do <b>{periodoEscolar.Bimestre}º bimestre</b> na <b>{descricaoUe}</b> irá encerrar no dia <b>{periodoFechamentoBimestre.FinalDoFechamento.Date:dd/MM/yyyy}</b>.
                <br/><br/>Após esta data o sistema será bloqueado para edições neste bimestre.";

            var professores = await ObterProfessores(turmas);
            if (professores != null && professores.Any())
                await mediator.Send(new EnviarNotificacaoUsuariosCommand(titulo, mensagem, NotificacaoCategoria.Aviso, NotificacaoTipo.Calendario, professores, ue.Dre.CodigoDre, ue.CodigoUe));

            await mediator.Send(new EnviarNotificacaoCommand(titulo, mensagem.ToString(), NotificacaoCategoria.Aviso, NotificacaoTipo.Calendario, ObterCargosGestaoEscola(),
             periodoFechamentoBimestre.PeriodoFechamento.Ue.Dre.CodigoDre,
             periodoFechamentoBimestre.PeriodoFechamento.Ue.CodigoUe));
        }


        private Cargo[] ObterCargosGestaoEscola()
        => new[] { Cargo.CP, Cargo.AD, Cargo.Diretor };


        private async Task<IEnumerable<long>> ObterProfessores(IEnumerable<Turma> turmas)
        {

            var listaUsuarios = new List<long>();
            foreach (var turma in turmas)
            {
                var professores = await mediator.Send(new ObterProfessoresTitularesDaTurmaQuery(turma.CodigoTurma));

                foreach (var professor in professores)
                {
                    if (professor != "")
                        listaUsuarios.Add(await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(professor)));
                }
            }
            return listaUsuarios.Distinct();
        }
    }
}
