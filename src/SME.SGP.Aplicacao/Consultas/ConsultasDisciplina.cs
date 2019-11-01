using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
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
        private readonly IServicoUsuario servicoUsuario;

        public ConsultasDisciplina(IServicoEOL servicoEOL,
                                   IRepositorioCache repositorioCache,
                                   IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem,
                                   IServicoUsuario servicoUsuario)
        {
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.repositorioCache = repositorioCache ?? throw new System.ArgumentNullException(nameof(repositorioCache));
            this.consultasObjetivoAprendizagem = consultasObjetivoAprendizagem ?? throw new System.ArgumentNullException(nameof(consultasObjetivoAprendizagem));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
        }

        public async Task<IEnumerable<DisciplinaDto>> ObterDisciplinasParaPlanejamento(FiltroDisciplinaPlanejamentoDto filtroDisciplinaPlanejamentoDto)
        {
            IEnumerable<DisciplinaDto> disciplinasDto = null;

            var login = servicoUsuario.ObterLoginAtual();

            var chaveCache = $"Disciplinas-planejamento-{filtroDisciplinaPlanejamentoDto.CodigoTurma}-{login}";
            var disciplinasCacheString = repositorioCache.Obter(chaveCache);

            if (!string.IsNullOrWhiteSpace(disciplinasCacheString))
                return TratarRetornoDisciplinasPlanejamento(JsonConvert.DeserializeObject<IEnumerable<DisciplinaDto>>(disciplinasCacheString), filtroDisciplinaPlanejamentoDto);

            var disciplinas = await servicoEOL.ObterDisciplinasParaPlanejamento(filtroDisciplinaPlanejamentoDto.CodigoTurma, login, servicoUsuario.ObterPerfilAtual());

            if (disciplinas == null || !disciplinas.Any())
                return disciplinasDto;

            disciplinasDto = await MapearParaDto(disciplinas);

            await repositorioCache.SalvarAsync(chaveCache, JsonConvert.SerializeObject(disciplinasDto));

            return TratarRetornoDisciplinasPlanejamento(disciplinasDto, filtroDisciplinaPlanejamentoDto);
        }

        private IEnumerable<DisciplinaDto> TratarRetornoDisciplinasPlanejamento(IEnumerable<DisciplinaDto> disciplinas, FiltroDisciplinaPlanejamentoDto filtroDisciplinaPlanejamentoDto)
        {
            if (filtroDisciplinaPlanejamentoDto.CodigoDisciplina == 0)
                return disciplinas;

            var codigosRegencias = new List<int> { 138, 2, 89, 7, 8 };

            if (filtroDisciplinaPlanejamentoDto.Regencia)
                return disciplinas.Where(x => codigosRegencias.Contains(x.CodigoComponenteCurricular));

            return disciplinas.Where(x => x.CodigoComponenteCurricular == filtroDisciplinaPlanejamentoDto.CodigoDisciplina);
        }

        public async Task<IEnumerable<DisciplinaDto>> ObterDisciplinasPorProfessorETurma(long codigoTurma)
        {
            IEnumerable<DisciplinaDto> disciplinasDto = null;

            var login = servicoUsuario.ObterLoginAtual();
            var perfilAtual = servicoUsuario.ObterPerfilAtual();

            var chaveCache = $"Disciplinas-{codigoTurma}-{login}--{perfilAtual}";
            var disciplinasCacheString = repositorioCache.Obter(chaveCache);

            if (!string.IsNullOrWhiteSpace(disciplinasCacheString))
            {
                disciplinasDto = JsonConvert.DeserializeObject<IEnumerable<DisciplinaDto>>(disciplinasCacheString);
            }
            else
            {
                var disciplinas = await servicoEOL.ObterDisciplinasPorCodigoTurmaLoginEPerfil(codigoTurma, login, perfilAtual);
                if (disciplinas != null && disciplinas.Any())
                {
                    disciplinasDto = await MapearParaDto(disciplinas);

                    await repositorioCache.SalvarAsync(chaveCache, JsonConvert.SerializeObject(disciplinasDto));
                }
            }
            return disciplinasDto;
        }

        private async Task<IEnumerable<DisciplinaDto>> MapearParaDto(IEnumerable<DisciplinaResposta> disciplinas)
        {
            var retorno = new List<DisciplinaDto>();

            if (disciplinas != null)
            {
                foreach (var disciplina in disciplinas)
                {
                    retorno.Add(new DisciplinaDto()
                    {
                        CodigoComponenteCurricular = disciplina.CodigoComponenteCurricular,
                        Nome = disciplina.Nome,
                        Regencia = disciplina.Regencia,
                        PossuiObjetivos = await consultasObjetivoAprendizagem.DisciplinaPossuiObjetivosDeAprendizagem(disciplina.CodigoComponenteCurricular)
                    });
                }
            }
            return retorno;
        }
    }
}