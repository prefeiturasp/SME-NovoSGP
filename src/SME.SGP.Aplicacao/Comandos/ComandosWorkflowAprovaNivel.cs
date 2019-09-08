using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ComandosWorkflowAprovaNivel : IComandosWorkflowAprovaNivel
    {
        private readonly IRepositorioWorkflowAprovaNivel repositorioWorkflowAprovaNivel;
        private readonly IUnitOfWork unitOfWork;

        public ComandosWorkflowAprovaNivel(IRepositorioWorkflowAprovaNivel repositorioWorkflowAprovaNivel, IUnitOfWork unitOfWork)
        {
            this.repositorioWorkflowAprovaNivel = repositorioWorkflowAprovaNivel ?? throw new ArgumentNullException(nameof(repositorioWorkflowAprovaNivel));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public void SalvarListaDto(IEnumerable<WorkflowAprovacaoNiveisDto> workflowAprovacaoNiveisDto)
        {
            var chave = Guid.NewGuid().ToString();
            unitOfWork.IniciarTransacao();
            foreach (var workflowAprovacaoNivelDto in workflowAprovacaoNiveisDto)
            {
                SalvarDto(workflowAprovacaoNivelDto, chave);
            }
            unitOfWork.PersistirTransacao();
        }

        private WorkflowAprovacao MapearParaEntidade(WorkflowAprovacaoNiveisDto workflowAprovacaoNivelDto, string chave)
        {
            return new WorkflowAprovacao()
            {
                Ano = workflowAprovacaoNivelDto.Ano,
                Descricao = workflowAprovacaoNivelDto.Descricao,
                DreId = workflowAprovacaoNivelDto.DreId,
                EscolaId = workflowAprovacaoNivelDto.EscolaId,
                TurmaId = workflowAprovacaoNivelDto.TurmaId,
                UsuarioId = workflowAprovacaoNivelDto.UsuarioId,
                Nivel = workflowAprovacaoNivelDto.Nivel,
                Chave = chave
            };
        }

        private void SalvarDto(WorkflowAprovacaoNiveisDto workflowAprovacaoNivelDto, string chave)
        {
            WorkflowAprovacao workflowAprovaNivel = MapearParaEntidade(workflowAprovacaoNivelDto, chave);
            repositorioWorkflowAprovaNivel.Salvar(workflowAprovaNivel);
        }
    }
}