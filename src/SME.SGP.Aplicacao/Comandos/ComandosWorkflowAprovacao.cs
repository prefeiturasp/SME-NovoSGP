using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ComandosWorkflowAprovacao : IComandosWorkflowAprovacao
    {
        private readonly IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao;
        private readonly IUnitOfWork unitOfWork;

        public ComandosWorkflowAprovacao(IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao, IUnitOfWork unitOfWork)
        {
            this.repositorioWorkflowAprovacao = repositorioWorkflowAprovacao ?? throw new ArgumentNullException(nameof(repositorioWorkflowAprovacao));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public void SalvarListaDto(WorkflowAprovacaoNiveisDto workflowAprovacaoNiveisDto)
        {
            WorkflowAprovacao workflowAprovacao = MapearDtoParaEntidade(workflowAprovacaoNiveisDto);
            



            //unitOfWork.IniciarTransacao();
            //foreach (var workflowAprovacaoNivelDto in workflowAprovacaoNiveisDto)
            //{
            //    SalvarDto(workflowAprovacaoNivelDto, chave);
            //}
            //unitOfWork.PersistirTransacao();
        }

        private WorkflowAprovacao MapearDtoParaEntidade(WorkflowAprovacaoNiveisDto workflowAprovacaoNiveisDto)
        {
            WorkflowAprovacao workflowAprovacao = new WorkflowAprovacao();
            workflowAprovacao.Ano = workflowAprovacaoNiveisDto.Ano;
            workflowAprovacao.DreId = workflowAprovacaoNiveisDto.DreId;
            workflowAprovacao.EscolaId = workflowAprovacaoNiveisDto.EscolaId;
            workflowAprovacao.TurmaId = workflowAprovacaoNiveisDto.TurmaId;
            workflowAprovacao.NotifacaoMensagem = workflowAprovacaoNiveisDto.NotificacaoMensagem;
            workflowAprovacao.NotifacaoTitulo = workflowAprovacaoNiveisDto.NotificacaoTitulo;

            foreach (var nivel in workflowAprovacaoNiveisDto.Niveis)
            {
                workflowAprovacao.Adicionar(new WorkflowAprovacaoNivel() {
                   Descricao = nivel.Descricao,
                   Nivel = nivel.Nivel,
                   UsuarioId = nivel.UsuarioId
                });
            }
            return workflowAprovacao;
        }

        //private WorkflowAprovacao MapearParaEntidade(WorkflowAprovacaoNiveisDto workflowAprovacaoNivelDto, string chave)
        //{
        //    return new WorkflowAprovacao()
        //    {
        //        Ano = workflowAprovacaoNivelDto.Ano,
        //        Descricao = workflowAprovacaoNivelDto.Descricao,
        //        DreId = workflowAprovacaoNivelDto.DreId,
        //        EscolaId = workflowAprovacaoNivelDto.EscolaId,
        //        TurmaId = workflowAprovacaoNivelDto.TurmaId,
        //        UsuarioId = workflowAprovacaoNivelDto.UsuarioId,
        //        Nivel = workflowAprovacaoNivelDto.Nivel,
        //        Chave = chave
        //    };
        //}

        //private void SalvarDto(WorkflowAprovacaoNiveisDto workflowAprovacaoNivelDto, string chave)
        //{
        //    WorkflowAprovacao workflowAprovaNivel = MapearParaEntidade(workflowAprovacaoNivelDto, chave);
        //    repositorioWorkflowAprovaNivel.Salvar(workflowAprovaNivel);
        //}
    }
}