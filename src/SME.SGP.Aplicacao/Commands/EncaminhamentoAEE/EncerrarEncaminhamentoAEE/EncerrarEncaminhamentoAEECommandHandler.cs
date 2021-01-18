using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EncerrarEncaminhamentoAEECommandHandler : IRequestHandler<EncerrarEncaminhamentoAEECommand, bool>
    {
        public EncerrarEncaminhamentoAEECommandHandler(IMediator mediator, IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioEncaminhamentoAEE = repositorioEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoAEE));
        }

        public IMediator mediator { get; }
        public IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE { get; }

        public async Task<bool> Handle(EncerrarEncaminhamentoAEECommand request, CancellationToken cancellationToken)
        {
            try
            {
                var encaminhamentoAEE = await mediator.Send(new ObterEncaminhamentoAEEComTurmaPorIdQuery(request.EncaminhamentoId));

                if (encaminhamentoAEE == null)
                    throw new NegocioException("O encaminhamento informado não foi encontrado");

                var aluno = await mediator.Send(new ObterAlunoPorCodigoEolQuery(encaminhamentoAEE.AlunoCodigo, DateTime.Now.Year));

                if (aluno == null)
                    throw new NegocioException("O aluno informado não foi encontrado");


                var idEntidadeEncaminhamento = await repositorioEncaminhamentoAEE.SalvarAsync(MapearParaEntidade(request, encaminhamentoAEE.AlunoCodigo));

                return idEntidadeEncaminhamento != 0;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private EncaminhamentoAEE MapearParaEntidade(EncerrarEncaminhamentoAEECommand request, string alunoCodigo)
            => new EncaminhamentoAEE()
            {
                Id = request.EncaminhamentoId,
                Situacao = Dominio.Enumerados.SituacaoAEE.Encerrado,
                AlunoCodigo = alunoCodigo,
                MotivoEncerramento = request.MotivoEncerramento
            };
    }
}
