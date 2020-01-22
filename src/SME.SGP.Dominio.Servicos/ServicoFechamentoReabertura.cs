using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoFechamentoReabertura : IServicoFechamentoReabertura
    {
        private readonly IComandosWorkflowAprovacao comandosWorkflowAprovacao;
        private readonly IRepositorioDre repositorioDre;
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;
        private readonly IRepositorioUe repositorioUe;
        private readonly IUnitOfWork unitOfWork;

        public ServicoFechamentoReabertura(IRepositorioFechamentoReabertura repositorioFechamentoReabertura, IUnitOfWork unitOfWork,
            IRepositorioUe repositorioUe, IRepositorioDre repositorioDre, IComandosWorkflowAprovacao comandosWorkflowAprovacao)
        {
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoReabertura));
            this.unitOfWork = unitOfWork ?? throw new System.ArgumentNullException(nameof(unitOfWork));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
            this.comandosWorkflowAprovacao = comandosWorkflowAprovacao ?? throw new ArgumentNullException(nameof(comandosWorkflowAprovacao));
        }

        public async Task Salvar(FechamentoReabertura fechamentoReabertura)
        {
            var fechamentoReaberturas = await repositorioFechamentoReabertura.Listar(fechamentoReabertura.TipoCalendarioId, null, null);

            fechamentoReabertura.PodeSalvar(fechamentoReaberturas);

            fechamentoReabertura.VerificaStatus();

            unitOfWork.IniciarTransacao();

            var fechamentoReaberturaId = await repositorioFechamentoReabertura.SalvarAsync(fechamentoReabertura);

            foreach (var fechamentoReaberturaBimestre in fechamentoReabertura.Bimestres)
            {
                fechamentoReaberturaBimestre.FechamentoAberturaId = fechamentoReaberturaId;
                await repositorioFechamentoReabertura.SalvarBimestre(fechamentoReaberturaBimestre);
            }

            if (fechamentoReabertura.Status == EntidadeStatus.AguardandoAprovacao)
            {
                fechamentoReabertura.WorkflowAprovacaoId = PersistirWorkflowFechamentoReabertura(fechamentoReabertura);
                await repositorioFechamentoReabertura.SalvarAsync(fechamentoReabertura);
            }
            else if (fechamentoReabertura.DeveCriarEventos())
            {
                //Criar eventos;
            }

            unitOfWork.PersistirTransacao();
        }

        private long PersistirWorkflowFechamentoReabertura(FechamentoReabertura fechamentoReabertura)
        {
            //var ue = repositorioUe.ObterPorId(fechamentoReabertura.UeId.Value);
            //if (ue == null)
            //    throw new NegocioException("Não foi possível localizar esta Ue.");

            //var dre = repositorioDre.ObterPorId(fechamentoReabertura.DreId.Value);
            //if (dre == null)
            //    throw new NegocioException("Não foi possível localizar esta Dre.");

            var wfAprovacaoEvento = new WorkflowAprovacaoDto()
            {
                Ano = fechamentoReabertura.Inicio.Year,
                NotificacaoCategoria = NotificacaoCategoria.Workflow_Aprovacao,
                EntidadeParaAprovarId = fechamentoReabertura.Id,
                Tipo = WorkflowAprovacaoTipo.Fechamento_Reabertura,
                UeId = fechamentoReabertura.Ue.CodigoUe,
                DreId = fechamentoReabertura.Dre.CodigoDre,
                NotificacaoTitulo = "Cadastro de período de reabertura de fechamento - ano anterior",
                NotificacaoTipo = NotificacaoTipo.Calendario,
                NotificacaoMensagem = $@"A {fechamentoReabertura.Ue.Nome} cadastrou um novo período de reabertura de fechamento de bimestre para o tipo de calendário {fechamentoReabertura.TipoCalendario.Nome} do ano de {fechamentoReabertura.TipoCalendario.AnoLetivo}. Para que o período seja considerado válido é necessário que você aceite esta notificação. <br />
                                           Descrição: {fechamentoReabertura.Descricao} <br />
                                           Início: {fechamentoReabertura.Inicio.ToString("dd/MM/yyyy")} <br />
                                           Fim: {fechamentoReabertura.Inicio.ToString("dd/MM/yyyy")} <br />
                                           Bimestres: {fechamentoReabertura.ObterBimestresNumeral()}"
            };

            wfAprovacaoEvento.Niveis.Add(new WorkflowAprovacaoNivelDto()
            {
                Cargo = Cargo.Supervisor,
                Nivel = 1
            });

            return comandosWorkflowAprovacao.Salvar(wfAprovacaoEvento);
        }
    }
}