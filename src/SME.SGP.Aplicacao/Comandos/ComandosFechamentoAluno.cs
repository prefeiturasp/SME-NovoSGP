﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosFechamentoAluno : IComandosFechamentoAluno
    {
        private readonly IRepositorioFechamentoAlunoConsulta repositorio;
        private readonly IMediator mediator;

        public ComandosFechamentoAluno(IRepositorioFechamentoAlunoConsulta repositorio, IMediator mediator)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<AuditoriaPersistenciaDto> SalvarAnotacaoAluno(AnotacaoAlunoDto anotacaoAluno)
        {
            var anotacao = await MapearParaEntidade(anotacaoAluno);

            // Excluir anotacao quando enviado string vazia
            if (string.IsNullOrEmpty(anotacaoAluno.Anotacao))
                anotacao.Excluido = true;

            await repositorio.SalvarAsync(anotacao);
            return (AuditoriaPersistenciaDto)anotacao;
        }

        private async Task<FechamentoAluno> MapearParaEntidade(AnotacaoAlunoDto anotacaoAluno)
        {
            var anotacao = await repositorio.ObterFechamentoAluno(anotacaoAluno.FechamentoId, anotacaoAluno.CodigoAluno);
            MoverRemoverExcluidos(anotacaoAluno, anotacao);
            if (anotacao == null)
                anotacao = new FechamentoAluno()
                {
                    FechamentoTurmaDisciplinaId = anotacaoAluno.FechamentoId,
                    AlunoCodigo = anotacaoAluno.CodigoAluno,
                    Anotacao = anotacaoAluno.Anotacao
                };
            else
                anotacao.Anotacao = anotacaoAluno.Anotacao;

            return anotacao;
        }
        private async Task MoverRemoverExcluidos(AnotacaoAlunoDto anotacaoAluno, FechamentoAluno anotacao)
        {
            if (!string.IsNullOrEmpty(anotacaoAluno?.Anotacao))
            {
                var moverArquivo = mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.FechamentoAnotacao, anotacao!=null? anotacao.Anotacao:string.Empty, anotacaoAluno.Anotacao));
                anotacaoAluno.Anotacao = moverArquivo.Result;
            }
            if (!string.IsNullOrEmpty(anotacao?.Anotacao))
            {
                var aquivoNovo = anotacaoAluno?.Anotacao !=null ? anotacaoAluno.Anotacao : string.Empty;
                await mediator.Send(new RemoverArquivosExcluidosCommand(arquivoAtual: anotacao.Anotacao, arquivoNovo: aquivoNovo,caminho:TipoArquivo.FechamentoAnotacao.Name()));
            }
        }
    }
}
