using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoFechamentoReabertura : IServicoFechamentoReabertura
    {
        private readonly IComandosWorkflowAprovacao comandosWorkflowAprovacao;
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;

        public ServicoFechamentoReabertura(IRepositorioFechamentoReabertura repositorioFechamentoReabertura, IUnitOfWork unitOfWork,
            IComandosWorkflowAprovacao comandosWorkflowAprovacao, IServicoUsuario servicoUsuario)
        {
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoReabertura));
            this.unitOfWork = unitOfWork ?? throw new System.ArgumentNullException(nameof(unitOfWork));
            this.comandosWorkflowAprovacao = comandosWorkflowAprovacao ?? throw new ArgumentNullException(nameof(comandosWorkflowAprovacao));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
        }

        public async Task Alterar(FechamentoReabertura fechamentoReabertura)
        {
            var fechamentoReaberturas = await repositorioFechamentoReabertura.Listar(fechamentoReabertura.TipoCalendarioId, null, null);

            fechamentoReaberturas = fechamentoReaberturas.Where(a => a.Id != fechamentoReabertura.Id);

            var usuarioAtual = await servicoUsuario.ObterUsuarioLogado();

            fechamentoReabertura.PodeSalvar(fechamentoReaberturas, usuarioAtual);

            fechamentoReabertura.VerificaStatus();

            unitOfWork.IniciarTransacao();

            var fechamentoReaberturaId = await repositorioFechamentoReabertura.SalvarAsync(fechamentoReabertura);

            repositorioFechamentoReabertura.ExcluirBimestres(fechamentoReabertura.Id);

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

            VerificaFechamentosReaberturasParaAlterar(fechamentoReabertura, fechamentoReaberturas);

            unitOfWork.PersistirTransacao();
        }

        public async Task Salvar(FechamentoReabertura fechamentoReabertura)
        {
            var fechamentoReaberturas = await repositorioFechamentoReabertura.Listar(fechamentoReabertura.TipoCalendarioId, null, null);

            var usuarioAtual = await servicoUsuario.ObterUsuarioLogado();
            fechamentoReabertura.PodeSalvar(fechamentoReaberturas, usuarioAtual);

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

        private void AtualizaFechamentosComDatasDistintas(FechamentoReabertura fechamentoReabertura, List<FechamentoReabertura> fechamentosComDataFinalMaior, List<FechamentoReabertura> fechamentosComDataInicialMenor)
        {
            foreach (var fechamentoComDataMaior in fechamentosComDataFinalMaior)
            {
                fechamentoComDataMaior.Fim = fechamentoReabertura.Fim;
                repositorioFechamentoReabertura.Salvar(fechamentoComDataMaior);
            }

            foreach (var fechamentoComDataMenor in fechamentosComDataInicialMenor)
            {
                fechamentoComDataMenor.Inicio = fechamentoReabertura.Fim;
                repositorioFechamentoReabertura.Salvar(fechamentoComDataMenor);
            }
        }

        private long PersistirWorkflowFechamentoReabertura(FechamentoReabertura fechamentoReabertura)
        {
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
                                           Fim: {fechamentoReabertura.Fim.ToString("dd/MM/yyyy")} <br />
                                           Bimestres: {fechamentoReabertura.ObterBimestresNumeral()}"
            };

            wfAprovacaoEvento.Niveis.Add(new WorkflowAprovacaoNivelDto()
            {
                Cargo = Cargo.Supervisor,
                Nivel = 1
            });

            return comandosWorkflowAprovacao.Salvar(wfAprovacaoEvento);
        }

        private void VerificaFechamentosReaberturasParaAlterar(FechamentoReabertura fechamentoReabertura, IEnumerable<FechamentoReabertura> fechamentoReaberturas)
        {
            if (fechamentoReabertura.EhParaDre() || fechamentoReabertura.EhParaSme())
            {
                var fechamentosParaAtualizarTupple = new List<(FechamentoReabertura, bool, bool)>();
                var fechamentosParaAtualizar = new List<FechamentoReabertura>();

                if (fechamentoReabertura.EhParaDre())
                {
                    fechamentosParaAtualizar = fechamentoReaberturas.Where(a => a.EhParaUe() && a.DreId == fechamentoReabertura.DreId).ToList();
                }
            }
            else if (fechamentoReabertura.EhParaSme())
            {
                //fechamentosComDataFinalMaior = fechamentoReaberturas.Where(a => a.Fim > fechamentoReabertura.Fim && (a.EhParaUe() || a.EhParaDre())).ToList();
                //fechamentosComDataInicialMenor = fechamentoReaberturas.Where(a => a.Inicio < fechamentoReabertura.Inicio && (a.EhParaUe() || a.EhParaDre())).ToList();
            }

            foreach (var fechamentoReaberturasParaAtualizar in fechamentosParaAtualizar)
            {
                var atualizaInicio = false;
                var atualizaFim = false;

                if (fechamentoReaberturasParaAtualizar.Inicio < fechamentoReabertura.Inicio)
                {
                    fechamentoReaberturasParaAtualizar.Inicio = fechamentoReabertura.Inicio;
                    atualizaInicio = true;
                }

                if (fechamentoReaberturasParaAtualizar.Fim > fechamentoReabertura.Fim)
                {
                    fechamentoReaberturasParaAtualizar.Fim = fechamentoReabertura.Fim;
                    atualizaFim = true;
                }

                if (atualizaInicio || atualizaFim)
                    fechamentosParaAtualizarTupple.Add((fechamentoReaberturasParaAtualizar, atualizaInicio, atualizaFim));

                //AtualizaFechamentosComDatasDistintas(fechamentoReabertura, fechamentosComDataFinalMaior, fechamentosComDataInicialMenor);

                //fechamentosComDataFinalMaior.AddRange(fechamentosComDataInicialMenor);

                //var fechamentosParaNotificar = fechamentosComDataFinalMaior.Select(a => a.Id).Distinct();
            }
        }
    }
}