using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarCicloEnsinoCommandHandler : IRequestHandler<SalvarCicloEnsinoCommand, AuditoriaDto>
    {
        private readonly IRepositorioCicloEnsino repositorioCicloEnsino;

        public SalvarCicloEnsinoCommandHandler(IRepositorioCicloEnsino repositorioCicloEnsino)
        {
            this.repositorioCicloEnsino = repositorioCicloEnsino ?? throw new ArgumentNullException(nameof(repositorioCicloEnsino));
        }

        public async Task<AuditoriaDto> Handle(SalvarCicloEnsinoCommand request, CancellationToken cancellationToken)
        {
            var cicloEnsino = MapearParaEntidade(request.CicloEnsino);

            await repositorioCicloEnsino.SalvarAsync(cicloEnsino);

            return (AuditoriaDto)cicloEnsino;
        }

        private CicloEnsino MapearParaEntidade(CicloRetornoDto ciclo)
            => new CicloEnsino()
            {
                CodEol = ciclo.Codigo,
                Descricao = ciclo.Descricao,
                DtAtualizacao = ciclo.DtAtualizacao,
                CodigoModalidadeEnsino = ciclo.CodigoModalidadeEnsino,
                CodigoEtapaEnsino = ciclo.CodigoEtapaEnsino
            };
    }
}
