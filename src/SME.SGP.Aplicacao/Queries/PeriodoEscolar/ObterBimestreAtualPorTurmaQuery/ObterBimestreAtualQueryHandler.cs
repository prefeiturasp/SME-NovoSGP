using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterBimestreAtualQueryHandler : IRequestHandler<ObterBimestreAtualQuery, int>
    {
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTurma repositorioTurma;

        public ObterBimestreAtualQueryHandler(IRepositorioPeriodoEscolar repositorioPeriodoEscolar, IRepositorioTurma repositorioTurma)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }
        public async Task<int> Handle(ObterBimestreAtualQuery request, CancellationToken cancellationToken)
        {
            var turma = repositorioTurma.ObterPorCodigo(request.CodigoTurma);
            if (turma == null)
                throw new NegocioException("Turma não encontrada");

            return await repositorioPeriodoEscolar.ObterBimestreAtualAsync(request.CodigoTurma, turma.ModalidadeTipoCalendario, request.DataReferencia);
        }
    }
}
