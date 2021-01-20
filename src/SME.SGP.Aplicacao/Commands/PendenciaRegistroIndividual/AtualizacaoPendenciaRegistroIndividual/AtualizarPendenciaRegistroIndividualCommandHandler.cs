using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarPendenciaRegistroIndividualCommandHandler : AsyncRequestHandler<AtualizarPendenciaRegistroIndividualCommand>
    {
        private readonly IRepositorioPendenciaRegistroIndividual repositorioPendenciaRegistroIndividual;
        private readonly IRepositorioPendenciaRegistroIndividualAluno repositorioPendenciaRegistroIndividualAluno;
        private readonly IRepositorioPendencia repositorioPendencia;
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;

        public AtualizarPendenciaRegistroIndividualCommandHandler(IRepositorioPendenciaRegistroIndividual repositorioPendenciaRegistroIndividual,
            IRepositorioPendenciaRegistroIndividualAluno repositorioPendenciaRegistroIndividualAluno, IRepositorioPendencia repositorioPendencia,
            IMediator mediator, IUnitOfWork unitOfWork)
        {
            this.repositorioPendenciaRegistroIndividual = repositorioPendenciaRegistroIndividual;
            this.repositorioPendenciaRegistroIndividualAluno = repositorioPendenciaRegistroIndividualAluno;
            this.repositorioPendencia = repositorioPendencia;
            this.mediator = mediator;
            this.unitOfWork = unitOfWork;
        }

        protected override async Task Handle(AtualizarPendenciaRegistroIndividualCommand request, CancellationToken cancellationToken)
        {
            var pendenciaRegistroIndividual = await repositorioPendenciaRegistroIndividual.ObterPendenciaRegistroIndividualPorTurmaESituacao(request.TurmaId, SituacaoPendencia.Pendente);
            if (pendenciaRegistroIndividual is null) return;

            var alunoDaPendencia = pendenciaRegistroIndividual.Alunos?.FirstOrDefault(x => x.CodigoAluno == request.CodigoAluno && x.Situacao == SituacaoPendenciaRegistroIndividualAluno.Pendente);
            if (alunoDaPendencia is null) return;

            var diasDeAusencia = await ObterDiasDeAusenciaParaPendenciaRegistroIndividualAsync();
            var direrencaEntreDataAtualEUltimoRegistro = DateTime.Today.Subtract(request.DataRegistro.Date);
            if (direrencaEntreDataAtualEUltimoRegistro.Days >= diasDeAusencia) return;

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    alunoDaPendencia.ResolverPendenciaDoAluno();
                    await repositorioPendenciaRegistroIndividualAluno.SalvarAsync(alunoDaPendencia);

                    if (pendenciaRegistroIndividual.Alunos.All(x => x.Situacao == SituacaoPendenciaRegistroIndividualAluno.Resolvido))
                    {
                        pendenciaRegistroIndividual.ResolverPendencia();
                        await repositorioPendencia.SalvarAsync(pendenciaRegistroIndividual.Pendencia);
                    }

                    unitOfWork.PersistirTransacao();
                }
                catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        private async Task<int> ObterDiasDeAusenciaParaPendenciaRegistroIndividualAsync()
        {
            var parametroDiasSemRegistroIndividual = await mediator.Send(new ObterParametroSistemaPorTipoQuery(TipoParametroSistema.PendenciaPorAusenciaDeRegistroIndividual));
            if (string.IsNullOrEmpty(parametroDiasSemRegistroIndividual))
                throw new NegocioException($"Não foi possível obter o parâmetro {TipoParametroSistema.PendenciaPorAusenciaDeRegistroIndividual.Name()} ");

            return int.Parse(parametroDiasSemRegistroIndividual);
        }
    }
}