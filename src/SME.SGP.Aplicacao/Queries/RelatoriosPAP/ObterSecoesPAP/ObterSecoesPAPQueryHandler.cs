using MediatR;
using Microsoft.Extensions.Logging;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSecoesPAPQueryHandler : IRequestHandler<ObterSecoesPAPQuery, SecaoTurmaAlunoPAPDto>
    {
        private readonly IRepositorioSecaoRelatorioPeriodicoPAP repositorio;
        private readonly ILogger<ObterSecoesPAPQueryHandler> logger;

        public ObterSecoesPAPQueryHandler(IRepositorioSecaoRelatorioPeriodicoPAP repositorio,
            ILogger<ObterSecoesPAPQueryHandler> logger)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<SecaoTurmaAlunoPAPDto> Handle(ObterSecoesPAPQuery request, CancellationToken cancellationToken)
        {
            var resultado = await repositorio.ObterSecoesPorAluno(request.CodigoTurma, request.CodigoAluno,
                request.PAPPeriodoId);

            foreach (var secao in resultado.Secoes.Where(item => item.QuantidadeSecoesPersistidas > 1))
                logger.LogWarning(
                    "Foram encontradas {Quantidade} seções PAP ativas para a turma {TurmaCodigo}, estudante {AlunoCodigo}, período {PeriodoId} e seção {SecaoId}. A seção {PAPSecaoId} foi priorizada.",
                    secao.QuantidadeSecoesPersistidas,
                    request.CodigoTurma,
                    request.CodigoAluno,
                    request.PAPPeriodoId,
                    secao.Id,
                    secao.PAPSecaoId);

            return resultado;
        }
    }
}
