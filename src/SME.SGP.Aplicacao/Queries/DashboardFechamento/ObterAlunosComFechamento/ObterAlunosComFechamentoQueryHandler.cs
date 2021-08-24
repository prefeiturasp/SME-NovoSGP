using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class
        ObterAlunosComFechamentoQueryHandler : IRequestHandler<ObterAlunosComFechamentoQuery,
            IEnumerable<TurmaAlunoBimestreFechamentoDto>>
    {
        private readonly IRepositorioFechamentoAluno repositorio;

        public ObterAlunosComFechamentoQueryHandler(IRepositorioFechamentoAluno repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<TurmaAlunoBimestreFechamentoDto>> Handle(ObterAlunosComFechamentoQuery request,
            CancellationToken cancellationToken)
            => await repositorio.ObterAlunosComFechamento(request.UeId,
                request.Ano, request.DreId, request.Modalidade,
                request.Semestre, request.Bimestre);
    }
}