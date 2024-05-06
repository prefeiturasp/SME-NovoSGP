using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao
{
    public class PersistirRelatorioSecaoCommandHandler : IRequestHandler<PersistirRelatorioSecaoCommand,RelatorioPeriodicoPAPSecao>
    {
        private readonly IMediator mediator;

        public PersistirRelatorioSecaoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<RelatorioPeriodicoPAPSecao> Handle(PersistirRelatorioSecaoCommand request, CancellationToken cancellationToken)
        {
            var secao = request.Secao;
            if (!secao.Respostas.Any())
                throw new NegocioException(string.Format(MensagemNegocioComuns.NENHUMA_QUESTAO_FOI_ENCONTRADA_NA_SECAO_X, secao.SecaoId));

            if (!secao.Id.HasValue)
                return await mediator.Send(
                    new SalvarRelatorioPeriodicoSecaoPAPCommand(secao.SecaoId, request.RelatorioAlunoId),
                    cancellationToken);
            var relatorioSecao = await mediator.Send(new ObterRelatorioPeriodicoSecaoPAPQuery(secao.Id.Value), cancellationToken);

            await mediator.Send(new AlterarRelatorioPeriodicoSecaoPAPCommand(relatorioSecao), cancellationToken);

            return relatorioSecao;

        }
    }
}