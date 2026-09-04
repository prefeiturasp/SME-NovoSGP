using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class PersistirRelatorioSecaoCommandHandler : IRequestHandler<PersistirRelatorioSecaoCommand,RelatorioPeriodicoPAPSecao>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioRelatorioPeriodicoPAPSecao repositorio;

        public PersistirRelatorioSecaoCommandHandler(IMediator mediator,
            IRepositorioRelatorioPeriodicoPAPSecao repositorio)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<RelatorioPeriodicoPAPSecao> Handle(PersistirRelatorioSecaoCommand request, CancellationToken cancellationToken)
        {
            var secao = request.Secao;
            if (!secao.Respostas.Any())
                throw new NegocioException(string.Format(MensagemNegocioComuns.NENHUMA_QUESTAO_FOI_ENCONTRADA_NA_SECAO_X, secao.SecaoId));

            var secaoId = secao.Id;
            if (!secaoId.HasValue)
                secaoId = await repositorio.ObterIdSecaoAtiva(request.RelatorioAlunoId, secao.SecaoId);

            if (!secaoId.HasValue)
                return await mediator.Send(
                    new SalvarRelatorioPeriodicoSecaoPAPCommand(secao.SecaoId, request.RelatorioAlunoId),
                    cancellationToken);

            var relatorioSecao = await mediator.Send(new ObterRelatorioPeriodicoSecaoPAPQuery(secaoId.Value), cancellationToken);

            if (relatorioSecao.Id == 0 ||
                relatorioSecao.RelatorioPeriodicoAlunoId != request.RelatorioAlunoId ||
                relatorioSecao.SecaoRelatorioPeriodicoId != secao.SecaoId)
                throw new NegocioException("A seção informada não pertence ao relatório PAP do estudante.");

            await mediator.Send(new AlterarRelatorioPeriodicoSecaoPAPCommand(relatorioSecao), cancellationToken);

            return relatorioSecao;

        }
    }
}
