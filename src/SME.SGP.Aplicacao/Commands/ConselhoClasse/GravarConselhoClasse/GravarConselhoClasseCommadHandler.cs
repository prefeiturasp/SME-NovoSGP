﻿using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes;

namespace SME.SGP.Aplicacao
{
    public class GravarConselhoClasseCommadHandler : IRequestHandler<GravarConselhoClasseCommad, ConselhoClasseNotaRetornoDto>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioConselhoClasseAlunoConsulta repositorioConselhoClasseAlunoConsulta;

        public GravarConselhoClasseCommadHandler(
                        IMediator mediator, 
                        IRepositorioConselhoClasseAlunoConsulta repositorioConselhoClasseAlunoConsulta)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioConselhoClasseAlunoConsulta = repositorioConselhoClasseAlunoConsulta ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAlunoConsulta));
        }

        public async Task<ConselhoClasseNotaRetornoDto> Handle(GravarConselhoClasseCommad request, CancellationToken cancellationToken)
        {
            var conselhoClasseNotaRetorno = request.ConselhoClasseId == 0 ?
                await mediator.Send(new InserirConselhoClasseNotaCommad(
                                            request.FechamentoTurma,
                                            request.CodigoAluno,
                                            request.ConselhoClasseNotaDto,
                                            request.Bimestre,
                                            request.Usuario), cancellationToken) :
                await mediator.Send(new AlterarConselhoClasseCommad(
                                            request.ConselhoClasseId,
                                            request.FechamentoTurma.Id,
                                            request.CodigoAluno,
                                            request.FechamentoTurma.Turma,
                                            request.ConselhoClasseNotaDto,
                                            request.Bimestre,
                                            request.Usuario), cancellationToken);

            var situacaoConselhoAtualizada = await mediator.Send(new AtualizaSituacaoConselhoClasseCommand(conselhoClasseNotaRetorno.ConselhoClasseId), cancellationToken);
            if (!situacaoConselhoAtualizada)
                throw new NegocioException("Erro ao atualizar situação do conselho de classe");

            await RemoverCache(string.Format(NomeChaveCache.CHAVE_NOTA_CONCEITO_FECHAMENTO_TURMA_BIMESTRE, request.FechamentoTurma.Turma.CodigoTurma, request.Bimestre), cancellationToken);
            await RemoverCache(string.Format(NomeChaveCache.CHAVE_NOTA_CONCEITO_CONSELHO_CLASSE_TURMA_BIMESTRE, request.FechamentoTurma.Turma.CodigoTurma, request.Bimestre), cancellationToken);

            return await Task.FromResult(conselhoClasseNotaRetorno);
        }

        private async Task RemoverCache(string nomeChave, CancellationToken cancellationToken)
        {
            await mediator.Send(new RemoverChaveCacheCommand(nomeChave), cancellationToken);
        }

    }
}
