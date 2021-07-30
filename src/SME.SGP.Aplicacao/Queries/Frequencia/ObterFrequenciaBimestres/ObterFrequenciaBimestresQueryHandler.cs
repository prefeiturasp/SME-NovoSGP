using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaBimestresQueryHandler : IRequestHandler<ObterFrequenciaBimestresQuery, IEnumerable<FrequenciaBimestreAlunoDto>>
    {
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo;

        public ObterFrequenciaBimestresQueryHandler(IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo)
        {
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
        }

        public async Task<IEnumerable<FrequenciaBimestreAlunoDto>> Handle(ObterFrequenciaBimestresQuery request, CancellationToken cancellationToken)
        {
            var frequenciaAluno = await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterFrequenciaBimestresAsync(request.CodigoAluno, request.Bimestre, request.CodigoTurma, TipoFrequenciaAluno.Geral);

            var frequenciaBimestreAlunoDto = new List<FrequenciaBimestreAlunoDto>();

            foreach (var frequencia in frequenciaAluno)
            {
                var frequenciaBimestreAluno = new FrequenciaBimestreAlunoDto()
                {
                    Bimestre = frequencia.Bimestre,
                    CodigoAluno = frequencia.CodigoAluno,
                    Frequencia = frequencia?.PercentualFrequencia != null ? frequencia.PercentualFrequencia : 0,
                    QuantidadeAusencias = frequencia.TotalAusencias,
                    QuantidadeCompensacoes = frequencia.TotalCompensacoes,
                    TotalAulas = frequencia.TotalAulas
                    
                };

                frequenciaBimestreAlunoDto.Add(frequenciaBimestreAluno);
            }

            return frequenciaBimestreAlunoDto;
        }


    }
}
