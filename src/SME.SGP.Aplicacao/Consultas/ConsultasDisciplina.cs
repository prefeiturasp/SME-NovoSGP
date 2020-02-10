using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasDisciplina : IConsultasDisciplina
    {
        private readonly IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem;
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;
        private readonly IRepositorioCache repositorioCache;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoUsuario servicoUsuario;

        public ConsultasDisciplina(IServicoEOL servicoEOL,
                                   IRepositorioCache repositorioCache,
                                   IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem,
                                   IServicoUsuario servicoUsuario,
                                   IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ)
        {
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.repositorioCache = repositorioCache ?? throw new System.ArgumentNullException(nameof(repositorioCache));
            this.consultasObjetivoAprendizagem = consultasObjetivoAprendizagem ?? throw new System.ArgumentNullException(nameof(consultasObjetivoAprendizagem));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new System.ArgumentNullException(nameof(repositorioAtribuicaoCJ));
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

            disciplinasDto = await MapearParaDto(disciplinas, filtroDisciplinaPlanejamentoDto.TurmaPrograma);

            await repositorioCache.SalvarAsync(chaveCache, JsonConvert.SerializeObject(disciplinasDto));

            return TratarRetornoDisciplinasPlanejamento(disciplinasDto, filtroDisciplinaPlanejamentoDto);
        }

        public async Task<List<DisciplinaDto>> ObterDisciplinasPorProfessorETurma(string codigoTurma, bool turmaPrograma)
        {
            var disciplinasDto = new List<DisciplinaDto>();
            string chaveCache;

            var login = servicoUsuario.ObterLoginAtual();
            var perfilAtual = servicoUsuario.ObterPerfilAtual();
            var ehPefilCJ = perfilAtual == Perfis.PERFIL_CJ;

            var disciplinasCacheString = ObterDisciplinasRedis(codigoTurma, login, perfilAtual, out chaveCache);

            if (!string.IsNullOrWhiteSpace(disciplinasCacheString))
                return JsonConvert.DeserializeObject<List<DisciplinaDto>>(disciplinasCacheString);

            var disciplinas = ehPefilCJ ? await ObterDisciplinasPerfilCJ(codigoTurma, login) :
                await servicoEOL.ObterDisciplinasPorCodigoTurmaLoginEPerfil(codigoTurma, login, perfilAtual);

            if (disciplinas == null || !disciplinas.Any())
                return disciplinasDto;

            disciplinasDto = await MapearParaDto(disciplinas, turmaPrograma);

            await repositorioCache.SalvarAsync(chaveCache, JsonConvert.SerializeObject(disciplinasDto));

            return disciplinasDto;
        }

        public async Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasPorProfessorETurmaTeste(string codigoTurma)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfilAtual = servicoUsuario.ObterPerfilAtual();

            var disciplinas = await servicoEOL.ObterDisciplinasPorCodigoTurmaLoginEPerfil(codigoTurma, login, perfilAtual);

            return disciplinas;
        }

        public async Task<List<DisciplinaDto>> ObterDisciplinasPorTurma(string codigoTurma, bool turmaPrograma)
        {
            var disciplinasDto = new List<DisciplinaDto>();

            var login = servicoUsuario.ObterLoginAtual();
            var perfilAtual = servicoUsuario.ObterPerfilAtual();

            var chaveCache = $"Disciplinas-{codigoTurma}";
            var disciplinasCacheString = repositorioCache.Obter(chaveCache);

            if (!string.IsNullOrWhiteSpace(disciplinasCacheString))
            {
                disciplinasDto = JsonConvert.DeserializeObject<List<DisciplinaDto>>(disciplinasCacheString);
            }
            else
            {
                IEnumerable<DisciplinaResposta> disciplinas;

                if (perfilAtual == Perfis.PERFIL_CJ)
                {
                    var atribuicoes = await repositorioAtribuicaoCJ.ObterPorFiltros(null, codigoTurma, string.Empty, 0, login, string.Empty, true);
                    if (atribuicoes != null && atribuicoes.Any())
                    {
                        var disciplinasEol = servicoEOL.ObterDisciplinasPorIds(atribuicoes.Select(a => a.DisciplinaId).Distinct().ToArray());

                        disciplinas = TransformarListaDisciplinaEolParaRetornoDto(disciplinasEol);
                    }
                    else disciplinas = null;
                }
                else
                    disciplinas = await servicoEOL.ObterDisciplinasPorCodigoTurma(codigoTurma);

                if (disciplinas != null && disciplinas.Any())
                {
                    disciplinasDto = await MapearParaDto(disciplinas, turmaPrograma);

                    await repositorioCache.SalvarAsync(chaveCache, JsonConvert.SerializeObject(disciplinasDto));
                }
            }
            return disciplinasDto;
        }

        public string ObterTiposIdsDisciplina()
        {
            Type t = typeof(DisciplinaResposta);

            string tiposRetorno = String.Empty;

            tiposRetorno += "DISCIPLINARESPOSTA; ";

            foreach (var prop in t.GetProperties())
                tiposRetorno += String.Format("   {0} ({1})", prop.Name,
                                  prop.PropertyType.Name);

            t = typeof(ProfessorTitularDisciplinaEol);

            tiposRetorno += "PROFESSORTITULARDISCIPLINAEOL; ";

            foreach (var prop in t.GetProperties())
                tiposRetorno += String.Format("   {0} ({1})", prop.Name,
                                  prop.PropertyType.Name);

            t = typeof(RetornoDisciplinaDto);

            tiposRetorno += "RETORNODISCIPLINADTO; ";

            foreach (var prop in t.GetProperties())
                tiposRetorno += String.Format("   {0} ({1})", prop.Name,
                                  prop.PropertyType.Name);

            t = typeof(DisciplinaDto);

            tiposRetorno += "DISCIPLINADTO; ";

            foreach (var prop in t.GetProperties())
                tiposRetorno += String.Format("   {0} ({1})", prop.Name,
                                  prop.PropertyType.Name);

            return tiposRetorno;
        }

        private async Task<List<DisciplinaDto>> MapearParaDto(IEnumerable<DisciplinaResposta> disciplinas, bool turmaPrograma = false)
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
                        Compartilhada = disciplina.Compartilhada,
                        RegistraFrequencia = disciplina.RegistroFrequencia,
                        PossuiObjetivos = !turmaPrograma && await consultasObjetivoAprendizagem
                        .DisciplinaPossuiObjetivosDeAprendizagem(disciplina.CodigoComponenteCurricular),
                        TerritorioSaber = disciplina.TerritorioSaber
                    });
                }
            }
            return retorno;
        }

        private async Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasPerfilCJ(string codigoTurma, string login)
        {
            var atribuicoes = await repositorioAtribuicaoCJ.ObterPorFiltros(null, codigoTurma, string.Empty, 0, login, string.Empty, true);

            if (atribuicoes == null || !atribuicoes.Any())
                return null;

            var disciplinasEol = servicoEOL.ObterDisciplinasPorIds(atribuicoes.Select(a => a.DisciplinaId).Distinct().ToArray());

            return TransformarListaDisciplinaEolParaRetornoDto(disciplinasEol);
        }

        private string ObterDisciplinasRedis(string codigoTurma, string login, Guid perfilAtual, out string chaveCache)
        {
            chaveCache = $"Disciplinas-{codigoTurma}-{login}--{perfilAtual}";

            return repositorioCache.Obter(chaveCache);
        }

        private IEnumerable<DisciplinaResposta> TransformarListaDisciplinaEolParaRetornoDto(IEnumerable<DisciplinaDto> disciplinasEol)
        {
            foreach (var disciplinaEol in disciplinasEol)
            {
                yield return new DisciplinaResposta()
                {
                    CodigoComponenteCurricular = disciplinaEol.CodigoComponenteCurricular,
                    Nome = disciplinaEol.Nome,
                    Regencia = disciplinaEol.Regencia,
                    Compartilhada = disciplinaEol.Compartilhada,
                    RegistroFrequencia = disciplinaEol.RegistraFrequencia
                };
            }
        }

        private IEnumerable<DisciplinaDto> TratarRetornoDisciplinasPlanejamento(IEnumerable<DisciplinaDto> disciplinas, FiltroDisciplinaPlanejamentoDto filtroDisciplinaPlanejamentoDto)
        {
            if (filtroDisciplinaPlanejamentoDto.CodigoDisciplina == 0)
                return disciplinas;

            if (filtroDisciplinaPlanejamentoDto.Regencia)
                return disciplinas.Where(x => !x.Regencia);

            return disciplinas.Where(x => x.CodigoComponenteCurricular == filtroDisciplinaPlanejamentoDto.CodigoDisciplina);
        }
    }
}