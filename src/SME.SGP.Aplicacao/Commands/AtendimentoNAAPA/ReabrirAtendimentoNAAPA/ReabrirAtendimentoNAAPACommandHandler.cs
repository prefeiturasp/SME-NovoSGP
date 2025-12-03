using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Aplicacao
{
    public class ReabrirAtendimentoNAAPACommandHandler : IRequestHandler<ReabrirAtendimentoNAAPACommand, SituacaoDto>
    {
        private readonly IUnitOfWork unitOfWork;
        public ReabrirAtendimentoNAAPACommandHandler(IUnitOfWork unitOfWork, IMediator mediator, IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public IMediator mediator { get; }
        public IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA { get; }

        public async Task<SituacaoDto> Handle(ReabrirAtendimentoNAAPACommand request, CancellationToken cancellationToken)
        {
            var encaminhamentoNAAPA = await mediator.Send(new ObterCabecalhoEncaminhamentoNAAPAQuery(request.EncaminhamentoId), cancellationToken);
            await ValidarRegras(encaminhamentoNAAPA);

            var encaminhamentoNAAPAPersistido = encaminhamentoNAAPA.Clone();
            encaminhamentoNAAPA.Situacao = await ObterSituacaoNAAPAReabertura(request.EncaminhamentoId);
            var situacaoDTO = new SituacaoDto() { Codigo = (int)encaminhamentoNAAPA.Situacao, Descricao = encaminhamentoNAAPA.Situacao.GetAttribute<DisplayAttribute>().Name };
            
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    await repositorioEncaminhamentoNAAPA.SalvarAsync(encaminhamentoNAAPA);
                    await mediator.Send(new RegistrarHistoricoDeAlteracaoDaSituacaoDoAtendimentoNAAPACommand(encaminhamentoNAAPAPersistido, encaminhamentoNAAPA.Situacao), cancellationToken);
                    unitOfWork.PersistirTransacao();
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }

            return situacaoDTO;
        }

        private async Task<SituacaoNAAPA> ObterSituacaoNAAPAReabertura(long encaminhamentoId)
        {
            return (await repositorioEncaminhamentoNAAPA.EncaminhamentoContemAtendimentosItinerancia(encaminhamentoId))
                    ? SituacaoNAAPA.EmAtendimento : SituacaoNAAPA.AguardandoAtendimento;
        }

        private async Task ValidarRegras(EncaminhamentoNAAPA encaminhamentoNAAPA)
        {
            if (encaminhamentoNAAPA.EhNulo() || encaminhamentoNAAPA.Id == 0)
                throw new NegocioException(MensagemNegocioEncaminhamentoNAAPA.ENCAMINHAMENTO_NAO_ENCONTRADO);

            if (encaminhamentoNAAPA.Situacao != SituacaoNAAPA.Encerrado)
                throw new NegocioException(MensagemNegocioEncaminhamentoNAAPA.ENCAMINHAMENTO_NAO_PODE_SER_REABERTO_NESTA_SITUACAO);

            var matriculasAlunoEol = await mediator
                .Send(new ObterAlunosEolPorCodigosQuery(long.Parse(encaminhamentoNAAPA.AlunoCodigo), true));

            var matriculaVigenteAluno = FiltrarMatriculaVigenteAluno(matriculasAlunoEol);

            if (matriculaVigenteAluno.EhNulo() || matriculaVigenteAluno.Inativo)
                throw new NegocioException(MensagemNegocioEncaminhamentoNAAPA.ENCAMINHAMENTO_ALUNO_INATIVO_NAO_PODE_SER_REABERTO);
        }

        private static TurmasDoAlunoDto FiltrarMatriculaVigenteAluno(IEnumerable<TurmasDoAlunoDto> matriculasAluno)
        {
            return matriculasAluno.Where(turma => turma.CodigoTipoTurma == (int)TipoTurma.Regular
                                                     && turma.AnoLetivo <= DateTimeExtension.HorarioBrasilia().Year
                                                     && turma.DataSituacao.Date <= DateTimeExtension.HorarioBrasilia().Date)
                                     .OrderByDescending(turma => turma.AnoLetivo)
                                     .ThenByDescending(turma => (turma.DataSituacao.Ticks, turma.DataAtualizacaoTabela.Ticks))
                                     .FirstOrDefault();
        }
    }
}
