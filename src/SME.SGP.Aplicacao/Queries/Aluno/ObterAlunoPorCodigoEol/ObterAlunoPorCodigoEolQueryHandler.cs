﻿using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class
        ObterAlunoPorCodigoEolQueryHandler : IRequestHandler<ObterAlunoPorCodigoEolQuery, AlunoPorTurmaResposta>
    {
        private readonly IServicoEol servicoEol;

        public ObterAlunoPorCodigoEolQueryHandler(IServicoEol servicoEol)
        {
            this.servicoEol = servicoEol ?? throw new System.ArgumentNullException(nameof(servicoEol));
        }

        public async Task<AlunoPorTurmaResposta> Handle(ObterAlunoPorCodigoEolQuery request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.CodigoTurma))
            {
                return (await servicoEol.ObterDadosAluno(request.CodigoAluno, request.AnoLetivo,
                        request.ConsideraHistorico, request.FiltrarSituacao))
                    .OrderByDescending(a => a.DataSituacao)?.FirstOrDefault();
            }

            var alunos = await ObterAluno(request.CodigoAluno,
                                          request.AnoLetivo,
                                          request.ConsideraHistorico,
                                          request.FiltrarSituacao,
                                          request.CodigoTurma);

            return alunos ?? await ObterAluno(request.CodigoAluno,
                                          request.AnoLetivo,
                                          !request.ConsideraHistorico,
                                          request.FiltrarSituacao,
                                          request.CodigoTurma);
        }


        private async Task<AlunoPorTurmaResposta> ObterAluno(string codigoAluno, int anoLetivo,
            bool historica, bool filtrarSituacao, string codigoTurma)
        {
            var response =
                (await servicoEol.ObterDadosAluno(codigoAluno, anoLetivo, historica, filtrarSituacao))
                            .OrderByDescending(a => a.DataSituacao);

            var retorno = response
                .Where(da => da.CodigoTurma.ToString().Equals(codigoTurma));

            if (retorno == null)
                return null;

            return retorno.FirstOrDefault(a => a.EstaAtivo(DateTime.Today.Date));
        }
    }
}