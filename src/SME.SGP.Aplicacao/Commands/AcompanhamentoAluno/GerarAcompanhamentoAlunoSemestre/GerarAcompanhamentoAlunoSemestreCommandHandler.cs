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
    public class GerarAcompanhamentoAlunoSemestreCommandHandler : IRequestHandler<GerarAcompanhamentoAlunoSemestreCommand, AcompanhamentoAlunoSemestre>
    {
        private readonly IRepositorioAcompanhamentoAlunoSemestre repositorioAcompanhamentoAlunoSemestre;

        public GerarAcompanhamentoAlunoSemestreCommandHandler(IRepositorioAcompanhamentoAlunoSemestre repositorioAcompanhamentoAlunoSemestre)
        {
            this.repositorioAcompanhamentoAlunoSemestre = repositorioAcompanhamentoAlunoSemestre ?? throw new ArgumentNullException(nameof(repositorioAcompanhamentoAlunoSemestre));
        }

        public async Task<AcompanhamentoAlunoSemestre> Handle(GerarAcompanhamentoAlunoSemestreCommand request, CancellationToken cancellationToken)
        {
            var acompanhamento = new AcompanhamentoAlunoSemestre()
            {
                AcompanhamentoAlunoId = request.AcompanhamentoAlunoId,
                Semestre = request.Semestre,
                Observacoes = request.Observacoes,
                PercursoIndividual = request.PercursoIndividual
            };

            await repositorioAcompanhamentoAlunoSemestre.SalvarAsync(acompanhamento);

            return acompanhamento;
        }
    }
}
