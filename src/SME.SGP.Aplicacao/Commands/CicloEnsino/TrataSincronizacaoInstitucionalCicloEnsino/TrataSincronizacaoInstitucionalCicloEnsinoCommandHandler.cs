using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TrataSincronizacaoInstitucionalCicloEnsinoCommandHandler : IRequestHandler<TrataSincronizacaoInstitucionalCicloEnsinoCommand, bool>
    {
        private readonly IRepositorioCicloEnsino repositorioCicloEnsino;

        public TrataSincronizacaoInstitucionalCicloEnsinoCommandHandler(IRepositorioCicloEnsino repositorioCicloEnsino)
        {
            this.repositorioCicloEnsino = repositorioCicloEnsino ?? throw new ArgumentNullException(nameof(repositorioCicloEnsino));
        }

        public async Task<bool> Handle(TrataSincronizacaoInstitucionalCicloEnsinoCommand request, CancellationToken cancellationToken)
        {

            if (request.CicloSgp.EhNulo())
            {
                var cicloEnsino = MapearParaEntidade(request.CicloEol);

                await repositorioCicloEnsino.SalvarAsync(cicloEnsino);
            }
            else
            {
                if ((request.CicloSgp.Descricao != request.CicloEol.Descricao)
                    || (request.CicloSgp.CodigoModalidadeEnsino != request.CicloEol.CodigoModalidadeEnsino) 
                    || (request.CicloSgp.CodigoEtapaEnsino != request.CicloSgp.CodigoEtapaEnsino))
                {
                    request.CicloSgp.Descricao = request.CicloEol.Descricao;
                    request.CicloSgp.DtAtualizacao = request.CicloEol.DtAtualizacao;
                    request.CicloSgp.CodigoModalidadeEnsino = request.CicloEol.CodigoModalidadeEnsino;
                    request.CicloSgp.CodigoEtapaEnsino = request.CicloSgp.CodigoEtapaEnsino;
                                        
                    await repositorioCicloEnsino.SalvarAsync(request.CicloSgp);
                }                
            }           

            return true;
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
