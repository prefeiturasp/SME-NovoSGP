using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasDisciplina : IConsultasDisciplina
    {
        private readonly IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem;
        private readonly IRepositorioCache repositorioCache;
        private readonly IServicoEOL servicoEOL;

        public ConsultasDisciplina(IServicoEOL servicoEOL,
                                   IRepositorioCache repositorioCache,
                                   IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem)
        {
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.repositorioCache = repositorioCache ?? throw new System.ArgumentNullException(nameof(repositorioCache));
            this.consultasObjetivoAprendizagem = consultasObjetivoAprendizagem ?? throw new System.ArgumentNullException(nameof(consultasObjetivoAprendizagem));
        }

        public async Task<IEnumerable<DisciplinaDto>> ObterDisciplinasPorProfessorETurma(long codigoTurma, string rfProfessor)
        {
            IEnumerable<DisciplinaDto> disciplinasDto = null;
            var chaveCache = $"Disciplinas-{codigoTurma}-{rfProfessor}";
            var disciplinasCacheString = repositorioCache.Obter(chaveCache);

            if (!string.IsNullOrWhiteSpace(disciplinasCacheString))
            {
                disciplinasDto = JsonConvert.DeserializeObject<IEnumerable<DisciplinaDto>>(disciplinasCacheString);
            }
            else
            {
                var disciplinas = await servicoEOL.ObterDisciplinasPorProfessorETurma(codigoTurma, rfProfessor);
                if (disciplinas != null && disciplinas.Any())
                {
                    disciplinasDto = MapearParaDto(disciplinas);

                    await repositorioCache.SalvarAsync(chaveCache, JsonConvert.SerializeObject(disciplinasDto));
                }
            }
            return disciplinasDto;
        }

        private IEnumerable<DisciplinaDto> MapearParaDto(IEnumerable<DisciplinaResposta> disciplinas)
        {
            if (disciplinas != null)
            {
                foreach (var disciplina in disciplinas)
                {
                    yield return new DisciplinaDto()
                    {
                        CodigoComponenteCurricular = disciplina.CodigoComponenteCurricular,
                        Nome = disciplina.Nome,
                        PossuiObjetivos = consultasObjetivoAprendizagem.DisciplinaPossuiObjetivosDeAprendizagem(disciplina.CodigoComponenteCurricular)
                    };
                }
            }
        }
    }
}