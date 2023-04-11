using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class AlterarCompensacaoAusenciaAlunoEAulaCommandHandler : IRequestHandler<AlterarCompensacaoAusenciaAlunoEAulaCommand, bool>
    {
        private readonly IRepositorioCompensacaoAusenciaAlunoConsulta repositorioCompensacaoAusenciaAlunoConsulta;
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno;
        private readonly IRepositorioCompensacaoAusenciaAlunoAula repositorioCompensacaoAusenciaAlunoAula;
        private readonly IMediator mediator;

        public AlterarCompensacaoAusenciaAlunoEAulaCommandHandler(IRepositorioCompensacaoAusenciaAlunoConsulta repositorioCompensacaoAusenciaAlunoConsulta,
               IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno, IUnitOfWork unitOfWork,
               IRepositorioCompensacaoAusenciaAlunoAula repositorioCompensacaoAusenciaAlunoAula,IMediator mediator)
        {
            this.repositorioCompensacaoAusenciaAlunoConsulta = repositorioCompensacaoAusenciaAlunoConsulta ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAlunoConsulta));
            this.repositorioCompensacaoAusenciaAluno = repositorioCompensacaoAusenciaAluno ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAluno));
            this.repositorioCompensacaoAusenciaAlunoAula = repositorioCompensacaoAusenciaAlunoAula ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAlunoAula));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(AlterarCompensacaoAusenciaAlunoEAulaCommand request, CancellationToken cancellationToken)
        {
            var compensacoes = (await repositorioCompensacaoAusenciaAlunoConsulta.ObterCompensacoesAusenciasAlunosPorRegistroFrequenciaAlunoIdsQuery(request.RegistroFrequenciaAlunoIds)).ToList();

            if (compensacoes.Any())
            {
                try
                {
                    unitOfWork.IniciarTransacao();
                    
                    foreach (var compensacao in compensacoes)
                    {
                        var qtdeFaltasAtualizadas = compensacao.QuantidadeCompensacoes - compensacao.QuantidadeRegistrosFrequenciaAluno;
                    
                        if (qtdeFaltasAtualizadas > 0)
                            await repositorioCompensacaoAusenciaAluno.AlterarQuantidadeFaltasCompensadasPorId(compensacao.CompensacaoAusenciaAlunoId, qtdeFaltasAtualizadas);
                        else
                            await repositorioCompensacaoAusenciaAluno.RemoverLogico(compensacao.CompensacaoAusenciaAlunoId);
                    }

                    await repositorioCompensacaoAusenciaAlunoAula.RemoverLogico(request.RegistroFrequenciaAlunoIds.ToArray(),"registro_frequencia_aluno_id");
                    
                    unitOfWork.PersistirTransacao();
                    
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ExclusaoCompensacaoAusenciaPorIds, new FiltroCompensacaoAusenciaDto(compensacoes.Select(s=> s.CompensacaoAusenciaId).ToArray()), Guid.NewGuid(), null));
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
            return true;
        }
    }
}
