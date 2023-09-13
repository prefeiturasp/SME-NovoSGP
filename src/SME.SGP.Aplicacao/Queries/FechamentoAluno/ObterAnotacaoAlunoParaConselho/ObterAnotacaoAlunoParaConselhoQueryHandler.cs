using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAnotacaoAlunoParaConselhoQueryHandler : IRequestHandler<ObterAnotacaoAlunoParaConselhoQuery, IEnumerable<FechamentoAlunoAnotacaoConselhoDto>>
    {
        private readonly IRepositorioAnotacaoFechamentoAlunoConsulta repositorioAnotacaoFechamentoAlunoConsulta;
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricularConsulta;

        public ObterAnotacaoAlunoParaConselhoQueryHandler(IRepositorioAnotacaoFechamentoAlunoConsulta repositorioAnotacaoFechamentoAlunoConsulta,IRepositorioComponenteCurricularConsulta repositorioComponenteCurricularConsulta)
        {
            this.repositorioAnotacaoFechamentoAlunoConsulta = repositorioAnotacaoFechamentoAlunoConsulta ?? throw new ArgumentNullException(nameof(repositorioAnotacaoFechamentoAlunoConsulta));
            this.repositorioComponenteCurricularConsulta = repositorioComponenteCurricularConsulta ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricularConsulta));
        }

        public async Task<IEnumerable<FechamentoAlunoAnotacaoConselhoDto>> Handle(ObterAnotacaoAlunoParaConselhoQuery request, CancellationToken cancellationToken)
        {
            var anotacoesDto = await repositorioAnotacaoFechamentoAlunoConsulta.ObterAnotacoesTurmaAlunoBimestreAsync(request.AlunoCodigo, request.TurmasCodigos, request.PeriodoId);
            if (anotacoesDto.EhNulo() || !anotacoesDto.Any())
                return default;

            var disciplinasIds = anotacoesDto.Select(a => long.Parse(a.DisciplinaId)).ToArray();

            var disciplinas = await repositorioComponenteCurricularConsulta.ObterDisciplinasPorIds(disciplinasIds);

            foreach (var anotacao in anotacoesDto)
            {
                var disciplina = disciplinas.FirstOrDefault(a => a.CodigoComponenteCurricular == long.Parse(anotacao.DisciplinaId));
                if (disciplina.NaoEhNulo())
                    anotacao.Disciplina = disciplina.Nome;
            }

            return anotacoesDto;
        }	
    }
}
