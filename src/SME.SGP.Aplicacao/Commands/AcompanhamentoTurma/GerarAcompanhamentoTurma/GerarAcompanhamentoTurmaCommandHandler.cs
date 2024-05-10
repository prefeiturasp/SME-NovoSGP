using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarAcompanhamentoTurmaCommandHandler : IRequestHandler<GerarAcompanhamentoTurmaCommand, AcompanhamentoTurma>
    {
        private readonly IRepositorioAcompanhamentoTurma repositorioAcompanhamentoTurma;

        public GerarAcompanhamentoTurmaCommandHandler(IRepositorioAcompanhamentoTurma repositorioAcompanhamentoTurma)
        {
            this.repositorioAcompanhamentoTurma = repositorioAcompanhamentoTurma ?? throw new ArgumentNullException(nameof(repositorioAcompanhamentoTurma));
        }

        public async Task<AcompanhamentoTurma> Handle(GerarAcompanhamentoTurmaCommand request, CancellationToken cancellationToken)
        {
            var acompanhamento = new AcompanhamentoTurma()
            {
                TurmaId = request.TurmaId,
                Semestre = request.Semestre,
                ApanhadoGeral = request.ApanhadoGeral
            };

            await repositorioAcompanhamentoTurma.SalvarAsync(acompanhamento);

            return acompanhamento;
        }
    }
}
