using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class AlteraWorkflowAprovacaoNivelNotificacaoCargoCommandHandler : IRequestHandler<AlteraWorkflowAprovacaoNivelNotificacaoCargoCommand, bool>
    {
        private readonly IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao;
        private readonly IRepositorioWorkflowAprovacaoNivel repositorioWorkflowAprovacaoNivel;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;
        private readonly IRepositorioWorkflowAprovacaoNivelNotificacao repositorioWorkflowAprovacaoNivelNotificacao;
        private readonly IRepositorioNotificacao repositorioNotificacao;

        public AlteraWorkflowAprovacaoNivelNotificacaoCargoCommandHandler(IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao,
            IRepositorioWorkflowAprovacaoNivel repositorioWorkflowAprovacaoNivel, IUnitOfWork unitOfWork, IMediator mediator,
            IRepositorioWorkflowAprovacaoNivelNotificacao repositorioWorkflowAprovacaoNivelNotificacao, IRepositorioNotificacao repositorioNotificacao)
        {
            this.repositorioWorkflowAprovacao = repositorioWorkflowAprovacao ?? throw new ArgumentNullException(nameof(repositorioWorkflowAprovacao));
            this.repositorioWorkflowAprovacaoNivel = repositorioWorkflowAprovacaoNivel ?? throw new ArgumentNullException(nameof(repositorioWorkflowAprovacaoNivel));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioWorkflowAprovacaoNivelNotificacao = repositorioWorkflowAprovacaoNivelNotificacao ?? throw new ArgumentNullException(nameof(repositorioWorkflowAprovacaoNivelNotificacao));
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
        }
        public async Task<bool> Handle(AlteraWorkflowAprovacaoNivelNotificacaoCargoCommand request, CancellationToken cancellationToken)
        {

            var wfAprovacao = repositorioWorkflowAprovacao.ObterEntidadeCompleta(request.WorkflowId);
            if (wfAprovacao == null)
                throw new NegocioException("Não foi possível obter o workflow de aprovação.");

            var nivelParaModificar = wfAprovacao.ObterNivelPorNotificacaoId(request.NotificacaoId);
            if (nivelParaModificar == null)
                throw new NegocioException("Não foi possível obter o nível do workflow de aprovação.");

            switch (nivelParaModificar.Cargo)
            {
                case Cargo.CP:
                    await TrataCargoCP(request.FuncionariosCargos, wfAprovacao, nivelParaModificar);
                    break;
                case Cargo.AD:
                    await TrataCargoAD(request.FuncionariosCargos, wfAprovacao, nivelParaModificar);
                    break;
                case Cargo.Diretor:
                    await TrataCargoDiretor(request.FuncionariosCargos, wfAprovacao, nivelParaModificar);
                    break;
                case Cargo.Supervisor:
                    await TrataCargoSupervisor(request.FuncionariosCargos, wfAprovacao, nivelParaModificar);
                    break;
                case Cargo.SupervisorTecnico:
                    break;
                default:
                    break;
            }

            return true;

        }

        private async Task TrataCargoDiretor(List<FuncionarioCargoDTO> funcionariosCargosDaUe, WorkflowAprovacao wfAprovacao, WorkflowAprovacaoNivel nivelParaModificar)
        {

            var ad = funcionariosCargosDaUe.FirstOrDefault(a => a.CargoId == (int)Cargo.AD);
            if (ad != null)
            {
                var adicionouNivelAd = await VerificaSeExisteNivelEadiciona(wfAprovacao, nivelParaModificar, Cargo.AD, funcionariosCargosDaUe);
                if (!adicionouNivelAd)
                    await TrataSupervisoresDiretor(funcionariosCargosDaUe, wfAprovacao, nivelParaModificar);
            }                
            else
            {
                await TrataSupervisoresDiretor(funcionariosCargosDaUe, wfAprovacao, nivelParaModificar);
            }
        }

        private async Task TrataSupervisoresDiretor(List<FuncionarioCargoDTO> funcionariosCargosDaUe, WorkflowAprovacao wfAprovacao, WorkflowAprovacaoNivel nivelParaModificar)
        {
            var supervisor = funcionariosCargosDaUe.FirstOrDefault(a => a.CargoId == (int)Cargo.Supervisor);
            if (supervisor != null)
                await VerificaSeExisteNivelEadiciona(wfAprovacao, nivelParaModificar, Cargo.Supervisor, funcionariosCargosDaUe);
            else
            {
                var supervisorTecnico = funcionariosCargosDaUe.FirstOrDefault(a => a.CargoId == (int)Cargo.SupervisorTecnico);
                if (supervisorTecnico != null)
                    await VerificaSeExisteNivelEadiciona(wfAprovacao, nivelParaModificar, Cargo.SupervisorTecnico, funcionariosCargosDaUe);
            }
        }

        private async Task TrataCargoAD(List<FuncionarioCargoDTO> funcionariosCargosDaUe, WorkflowAprovacao wfAprovacao, WorkflowAprovacaoNivel nivelParaModificar)
        {
            var diretor = funcionariosCargosDaUe.FirstOrDefault(a => a.CargoId == (int)Cargo.Diretor);
            if (diretor != null)
                await VerificaSeExisteNivelEadiciona(wfAprovacao, nivelParaModificar, Cargo.Diretor, funcionariosCargosDaUe);
            else
            {
                var supervisor = funcionariosCargosDaUe.FirstOrDefault(a => a.CargoId == (int)Cargo.Supervisor);
                if (supervisor != null)
                    await VerificaSeExisteNivelEadiciona(wfAprovacao, nivelParaModificar, Cargo.Supervisor, funcionariosCargosDaUe);
                else
                {
                    var supervisorTecnico = funcionariosCargosDaUe.FirstOrDefault(a => a.CargoId == (int)Cargo.SupervisorTecnico);
                    if (supervisorTecnico != null)
                        await VerificaSeExisteNivelEadiciona(wfAprovacao, nivelParaModificar, Cargo.SupervisorTecnico, funcionariosCargosDaUe);
                }
            }
        }

        private async Task TrataCargoSupervisor(List<FuncionarioCargoDTO> funcionariosCargosDaUe, WorkflowAprovacao wfAprovacao, WorkflowAprovacaoNivel nivelParaModificar)
        {
            var supervisorTecnico = funcionariosCargosDaUe.FirstOrDefault(a => a.CargoId == (int)Cargo.SupervisorTecnico);
            if (supervisorTecnico != null)
                await VerificaSeExisteNivelEadiciona(wfAprovacao, nivelParaModificar, Cargo.SupervisorTecnico, funcionariosCargosDaUe);
        }

        private async Task TrataCargoCP(List<FuncionarioCargoDTO> funcionariosCargosDaUe, WorkflowAprovacao wfAprovacao, WorkflowAprovacaoNivel nivelParaModificar)
        {
            var ad = funcionariosCargosDaUe.FirstOrDefault(a => a.CargoId == (int)Cargo.AD);
            if (ad != null)
                await VerificaSeExisteNivelEadiciona(wfAprovacao, nivelParaModificar, Cargo.AD, funcionariosCargosDaUe);
            else
            {
                var diretor = funcionariosCargosDaUe.FirstOrDefault(a => a.CargoId == (int)Cargo.Diretor);
                if (diretor != null)
                    await VerificaSeExisteNivelEadiciona(wfAprovacao, nivelParaModificar, Cargo.Diretor, funcionariosCargosDaUe);
                else
                {
                    var supervisor = funcionariosCargosDaUe.FirstOrDefault(a => a.CargoId == (int)Cargo.Supervisor);
                    if (supervisor != null)
                        await VerificaSeExisteNivelEadiciona(wfAprovacao, nivelParaModificar, Cargo.Supervisor, funcionariosCargosDaUe);
                    else
                        await VerificaSeExisteNivelEadiciona(wfAprovacao, nivelParaModificar, Cargo.SupervisorTecnico, funcionariosCargosDaUe);
                }
            }
        }

        private async Task<bool> VerificaSeExisteNivelEadiciona(WorkflowAprovacao wfAprovacao, WorkflowAprovacaoNivel nivelParaModificar, Cargo cargo, List<FuncionarioCargoDTO> funcionariosCargosDaUe)
        {
            var nivelDoCargo = wfAprovacao.ObterPrimeiroNivelPorCargo(cargo);

            var modificaNiveisPosteriores = false;

            if (nivelDoCargo == null)
            {
                modificaNiveisPosteriores = true;

                nivelDoCargo = new WorkflowAprovacaoNivel()
                {
                    Cargo = cargo,
                    Nivel = nivelParaModificar.Nivel + 1,
                    WorkflowId = wfAprovacao.Id
                };
            }
            else
            {
                if (nivelDoCargo.Notificacoes.Any())
                    return false;
            }

            await TrataModificacaoDosNiveis(funcionariosCargosDaUe, wfAprovacao, nivelParaModificar, true, nivelDoCargo, modificaNiveisPosteriores);
            
            return true;
        }

        private async Task TrataModificacaoDosNiveis(List<FuncionarioCargoDTO> funcionariosCargosDaUe, WorkflowAprovacao wfAprovacao, WorkflowAprovacaoNivel nivelParaModificar, bool modificarNivelAtual, WorkflowAprovacaoNivel nivelDoCargo, bool modificaNiveisPosteriores)
        {
            if (modificarNivelAtual)
            {
                if (modificaNiveisPosteriores)
                {
                    var niveisParaModificar = wfAprovacao.Niveis.Where(a => a.Nivel > nivelParaModificar.Nivel);

                    foreach (var nivel in niveisParaModificar)
                    {
                        nivel.Nivel += 1;
                        await repositorioWorkflowAprovacaoNivel.SalvarAsync(nivel);
                    }
                }

                await repositorioNotificacao.ExcluirLogicamentePorIdsAsync(nivelParaModificar.Notificacoes.Select(a => a.Id).ToArray());

                nivelParaModificar.Status = WorkflowAprovacaoNivelStatus.Substituido;
                nivelDoCargo.Status = WorkflowAprovacaoNivelStatus.AguardandoAprovacao;

                await repositorioWorkflowAprovacaoNivel.SalvarAsync(nivelParaModificar);
                nivelDoCargo.Id = await repositorioWorkflowAprovacaoNivel.SalvarAsync(nivelDoCargo);
                await TrataNovoNivel(wfAprovacao, nivelDoCargo, funcionariosCargosDaUe);

            }
        }

        private async Task TrataNovoNivel(WorkflowAprovacao wfAprovacao, WorkflowAprovacaoNivel nivelDoCargo, List<FuncionarioCargoDTO> funcionariosCargosDaUe)
        {
            var funcionariosDoNivel = funcionariosCargosDaUe.Where(a => a.CargoId == (int)nivelDoCargo.Cargo);
            var notificacaoBase = wfAprovacao.Niveis.Where(a => a.Notificacoes.Any()).SelectMany(a => a.Notificacoes).FirstOrDefault();

            foreach (var funcionarioDoNivel in funcionariosDoNivel)
            {
                var notificarUsuarioCommand = new NotificarUsuarioCommand(
              wfAprovacao.NotifacaoTitulo,
              wfAprovacao.NotifacaoMensagem,
              funcionarioDoNivel.FuncionarioRF,
              (NotificacaoCategoria)wfAprovacao.NotificacaoCategoria,
              (NotificacaoTipo)wfAprovacao.NotificacaoTipo,
              wfAprovacao.DreId,
              wfAprovacao.UeId,
              wfAprovacao.TurmaId,
              wfAprovacao.Ano,
              notificacaoBase.Codigo,
              notificacaoBase.CriadoEm);

                var notificacaoId = await mediator.Send(notificarUsuarioCommand);

                repositorioWorkflowAprovacaoNivelNotificacao.Salvar(new WorkflowAprovacaoNivelNotificacao()
                {
                    NotificacaoId = notificacaoId,
                    WorkflowAprovacaoNivelId = nivelDoCargo.Id
                });

            }

        }
    }
}
