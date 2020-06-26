using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasDisciplina : IConsultasDisciplina
    {
        private static readonly long[] IDS_COMPONENTES_REGENCIA = { 2, 7, 8, 89, 138 };
        private readonly IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem;
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioComponenteCurricular repositorioComponenteCurricular;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IServicoEol servicoEOL;
        private readonly IServicoUsuario servicoUsuario;

        public ConsultasDisciplina(IServicoEol servicoEOL,
            IRepositorioCache repositorioCache,
            IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem,
            IServicoUsuario servicoUsuario,
            IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ,
            IRepositorioComponenteCurricular repositorioComponenteCurricular,
            IRepositorioTurma repositorioTurma)
        {
            this.servicoEOL = servicoEOL ??
                throw new System.ArgumentNullException(nameof(servicoEOL));
            this.repositorioCache = repositorioCache ??
                throw new System.ArgumentNullException(nameof(repositorioCache));
            this.consultasObjetivoAprendizagem = consultasObjetivoAprendizagem ??
                throw new System.ArgumentNullException(nameof(consultasObjetivoAprendizagem));
            this.servicoUsuario = servicoUsuario ??
                throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ??
                throw new System.ArgumentNullException(nameof(repositorioAtribuicaoCJ));
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ??
                throw new System.ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.repositorioTurma = repositorioTurma ??
                throw new System.ArgumentNullException(nameof(repositorioTurma));
        }

        public IEnumerable<DisciplinaDto> MapearParaDto(IEnumerable<DisciplinaResposta> disciplinas)
        {
            foreach (var disciplina in disciplinas)
                yield return MapearParaDto(disciplina);
        }

        public async Task<IEnumerable<DisciplinaResposta>> ObterComponentesCJ(Modalidade? modalidade, string codigoTurma, string ueId, long codigoDisciplina, string rf, bool ignorarDeParaRegencia = false)
        {
            IEnumerable<DisciplinaResposta> componentes = null;
            var atribuicoes = await repositorioAtribuicaoCJ.ObterPorFiltros(modalidade,
                codigoTurma,
                ueId,
                codigoDisciplina,
                rf,
                string.Empty,
                true);

            if (atribuicoes == null || !atribuicoes.Any())
                return null;

            var disciplinasEol = await servicoEOL.ObterDisciplinasPorIdsAsync(atribuicoes.Select(a => a.DisciplinaId).Distinct().ToArray());

            var componenteRegencia = disciplinasEol?.FirstOrDefault(c => c.Regencia);
            if (componenteRegencia == null || ignorarDeParaRegencia)
                return TransformarListaDisciplinaEolParaRetornoDto(disciplinasEol);

            var componentesRegencia = await servicoEOL.ObterDisciplinasPorIdsAsync(IDS_COMPONENTES_REGENCIA);
            if (componentesRegencia != null)
                return TransformarListaDisciplinaEolParaRetornoDto(componentesRegencia);

            return componentes;
        }

        public async Task<List<DisciplinaDto>> ObterComponentesCurricularesPorProfessorETurma(string codigoTurma, bool turmaPrograma)
        {
            List<DisciplinaDto> disciplinasDto;

            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();

            var chaveCache = $"Disciplinas-{codigoTurma}-{usuarioLogado.PerfilAtual}";

            if (!usuarioLogado.EhProfessor())
            {
                var disciplinasCacheString = await repositorioCache.ObterAsync(chaveCache);

                if (!string.IsNullOrWhiteSpace(disciplinasCacheString))
                    return JsonConvert.DeserializeObject<List<DisciplinaDto>>(disciplinasCacheString);
            }

            var turma = await repositorioTurma.ObterPorCodigo(codigoTurma);
            if (turma == null)
                throw new NegocioException("Não foi possível encontrar a turma");

            if (usuarioLogado.EhProfessorCj())
            {
                var disciplinas = await ObterDisciplinasPerfilCJ(codigoTurma, usuarioLogado.Login);
                disciplinasDto = MapearParaDto(disciplinas, turmaPrograma);
            }
            else
            {
                var componentesCurriculares = await servicoEOL.ObterComponentesCurricularesPorCodigoTurmaLoginEPerfil(codigoTurma, usuarioLogado.Login, usuarioLogado.PerfilAtual);
                
                var componentesCurricularesJurema = await repositorioCache.Obter("ComponentesJurema", () => Task.FromResult(repositorioComponenteCurricular.Listar()));
                if (componentesCurricularesJurema == null)
                {
                    throw new NegocioException("Não foi possível recuperar a lista de componentes curriculares.");
                }
                
                disciplinasDto = componentesCurriculares?.Select(disciplina => new DisciplinaDto()
                {
                    CdComponenteCurricularPai = disciplina.CodigoComponenteCurricularPai,
                    CodigoComponenteCurricular = disciplina.Codigo,
                    Nome = disciplina.Descricao,
                    Regencia = disciplina.Regencia,
                    TerritorioSaber = disciplina.TerritorioSaber,
                    Compartilhada = disciplina.Compartilhada,
                    LancaNota = disciplina.LancaNota,
                    PossuiObjetivos = disciplina.PossuiObjetivosDeAprendizagem(componentesCurricularesJurema, turmaPrograma, turma.ModalidadeCodigo, turma.Ano),
                    RegistraFrequencia = disciplina.RegistraFrequencia
                })?.ToList();

                if (!usuarioLogado.EhProfessor())
                    await repositorioCache.SalvarAsync(chaveCache, JsonConvert.SerializeObject(disciplinasDto));
            }

            return disciplinasDto;
        }

        public async Task<IEnumerable<DisciplinaDto>> ObterComponentesCurricularesPorProfessorETurmaParaPlanejamento(long codigoDisciplina, string codigoTurma, bool turmaPrograma, bool regencia)
        {
            IEnumerable<DisciplinaDto> disciplinasDto = null;
            var usuario = await servicoUsuario.ObterUsuarioLogado();

            var chaveCache = $"Disciplinas-planejamento-{codigoTurma}-{codigoDisciplina}-{usuario.PerfilAtual}";
            if (!usuario.EhProfessor() && !usuario.EhProfessorCj() && !usuario.EhProfessorPoa())
            {
                var disciplinasCacheString = await repositorioCache.ObterAsync(chaveCache);

                if (!string.IsNullOrWhiteSpace(disciplinasCacheString))
                {
                    disciplinasDto = JsonConvert.DeserializeObject<List<DisciplinaDto>>(disciplinasCacheString);
                    return TratarRetornoDisciplinasPlanejamento(disciplinasDto, codigoDisciplina, regencia);
                }
            }

            var componentesCurricularesJurema = await repositorioCache.Obter("ComponentesJurema", () => Task.FromResult(repositorioComponenteCurricular.Listar()));
            if (componentesCurricularesJurema == null)
            {
                throw new NegocioException("Não foi possível recuperar a lista de componentes curriculares.");
            }

            var turma = await repositorioTurma.ObterPorCodigo(codigoTurma);
            if (turma == null)
                throw new NegocioException("Não foi possível encontrar a turma");

            if (usuario.EhProfessorCj())
            {
                var componentesCJ = await ObterComponentesCJ(null, codigoTurma,
                    string.Empty,
                    codigoDisciplina,
                    usuario.Login);
                disciplinasDto = MapearParaDto(componentesCJ, turmaPrograma);
            }
            else
            {
                var componentesCurriculares = await servicoEOL.ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamento(codigoTurma, usuario.Login, usuario.PerfilAtual);
                disciplinasDto = componentesCurriculares?.Select(disciplina => new DisciplinaDto()
                {
                    CdComponenteCurricularPai = disciplina.CodigoComponenteCurricularPai,
                    CodigoComponenteCurricular = disciplina.Codigo,
                    Nome = disciplina.Descricao,
                    Regencia = disciplina.Regencia,
                    TerritorioSaber = disciplina.TerritorioSaber,
                    Compartilhada = disciplina.Compartilhada,
                    LancaNota = disciplina.LancaNota,
                    PossuiObjetivos = disciplina.PossuiObjetivosDeAprendizagem(componentesCurricularesJurema, turmaPrograma, turma.ModalidadeCodigo, turma.Ano)
                })?.ToList();
            }

            if (!usuario.EhProfessor() && !usuario.EhProfessorCj() && !usuario.EhProfessorPoa())
                await repositorioCache.SalvarAsync(chaveCache, JsonConvert.SerializeObject(disciplinasDto));

            return TratarRetornoDisciplinasPlanejamento(disciplinasDto, codigoDisciplina, regencia);
        }

        public async Task<IEnumerable<DisciplinaResposta>> ObterComponentesRegencia(Turma turma, long componenteCurricularCodigo)
        {
            var usuario = await servicoUsuario.ObterUsuarioLogado();
            if (usuario.EhProfessorCj())
                return await ObterComponentesCJ(turma.ModalidadeCodigo, turma.CodigoTurma, turma.Ue.CodigoUe, componenteCurricularCodigo, usuario.CodigoRf);
            else
            {
                var componentesCurriculares = await servicoEOL.ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamento(turma.CodigoTurma, usuario.Login, usuario.PerfilAtual);

                return MapearComponentes(componentesCurriculares.Where(c => c.Regencia));
            }
        }

        public async Task<DisciplinaDto> ObterDisciplina(long disciplinaId)
        {
            var disciplinaEOL = await servicoEOL.ObterDisciplinasPorIdsAsync(new long[] { disciplinaId });
            if (disciplinaEOL == null || !disciplinaEOL.Any())
                throw new NegocioException($"Disciplina nÃ£o localizada no EOL [{disciplinaId}]");

            return disciplinaEOL.FirstOrDefault();
        }

        public async Task<List<DisciplinaDto>> ObterDisciplinasAgrupadasPorProfessorETurma(string codigoTurma, bool turmaPrograma)
        {
            var disciplinasDto = new List<DisciplinaDto>();

            var login = servicoUsuario.ObterLoginAtual();
            var perfilAtual = servicoUsuario.ObterPerfilAtual();

            var chaveCache = $"Disciplinas-Agrupadas-{codigoTurma}-{login}--{perfilAtual}";
            var disciplinasCacheString = repositorioCache.Obter(chaveCache);

            if (!string.IsNullOrWhiteSpace(disciplinasCacheString))
            {
                disciplinasDto = JsonConvert.DeserializeObject<List<DisciplinaDto>>(disciplinasCacheString);
            }
            else
            {
                if (perfilAtual == Perfis.PERFIL_CJ)
                {
                    // Carrega Disciplinas da Atribuição do CJ
                    var atribuicoes = await repositorioAtribuicaoCJ.ObterPorFiltros(null, codigoTurma, string.Empty, 0, login, string.Empty, true);
                    if (atribuicoes != null && atribuicoes.Any())
                    {
                        var disciplinasEol = servicoEOL.ObterDisciplinasPorIds(atribuicoes.Select(a => a.DisciplinaId).Distinct().ToArray());

                        foreach (var disciplinaEOL in disciplinasEol)
                        {
                            if (disciplinaEOL.CdComponenteCurricularPai > 0)
                            {
                                // TODO Consulta por disciplina pai não esta funcionando no EOL. Refatorar na proxima sprint
                                disciplinaEOL.CdComponenteCurricularPai = 11211124;
                                disciplinaEOL.Nome = "REG CLASSE INTEGRAL";

                                //var consultaDisciplinaPai = servicoEOL.ObterDisciplinasPorIds(new long[] { disciplinaEOL.CodigoComponenteCurricularId });
                                //if (consultaDisciplinaPai == null)
                                //    throw new NegocioException($"Disciplina Pai de codigo [{disciplinaEOL.CodigoComponenteCurricularId}] não localizada no EOL.");

                                //disciplinasDto.Add(consultaDisciplinaPai.First());
                            }
                            else
                                disciplinasDto.Add(disciplinaEOL);
                        }
                    }
                }
                else
                {
                    // Carrega disciplinas do professor
                    IEnumerable<DisciplinaResposta> disciplinas = await servicoEOL.ObterDisciplinasPorCodigoTurmaLoginEPerfil(codigoTurma, login, perfilAtual);
                    foreach (var disciplina in disciplinas)
                    {
                        if (disciplina.CodigoComponenteCurricularPai.HasValue)
                        {
                            // TODO Consulta por disciplina pai não esta funcionando no EOL. Refatorar na proxima sprint
                            disciplina.CodigoComponenteCurricular = 11211124;
                            disciplina.Nome = "REG CLASSE INTEGRAL";

                            //var consultaDisciplinaPai = servicoEOL.ObterDisciplinasPorIds(new long[] { disciplina.CodigoComponenteCurricularPai.Value });
                            //if (consultaDisciplinaPai == null)
                            //    throw new NegocioException($"Disciplina Pai de codigo [{disciplina.CodigoComponenteCurricularPai}] não localizada no EOL.");

                            //disciplinasDto.Add(consultaDisciplinaPai.First());
                        }
                        disciplinasDto.Add(MapearParaDto(disciplina, true));
                    }
                }

                if (disciplinasDto.Any())
                    await repositorioCache.SalvarAsync(chaveCache, JsonConvert.SerializeObject(disciplinasDto));
            }

            return disciplinasDto;
        }

        public async Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasPerfilCJ(string codigoTurma, string login)
        {
            var atribuicoes = await repositorioAtribuicaoCJ.ObterPorFiltros(null, codigoTurma, string.Empty, 0, login, string.Empty, true);

            if (atribuicoes == null || !atribuicoes.Any())
                return null;

            var disciplinasEol = servicoEOL.ObterDisciplinasPorIds(atribuicoes.Select(a => a.DisciplinaId).Distinct().ToArray());

            return TransformarListaDisciplinaEolParaRetornoDto(disciplinasEol);
        }

        public async Task<List<DisciplinaDto>> ObterDisciplinasPorProfessorETurma(string codigoTurma, bool turmaPrograma)
        {
            var disciplinasDto = new List<DisciplinaDto>();

            var login = servicoUsuario.ObterLoginAtual();
            var perfilAtual = servicoUsuario.ObterPerfilAtual();
            var ehPefilCJ = perfilAtual == Perfis.PERFIL_CJ;

            var chaveCache = $"Disciplinas-{codigoTurma}-{login}--{perfilAtual}";

            var disciplinasCacheString = await repositorioCache.ObterAsync(chaveCache);

            if (!string.IsNullOrWhiteSpace(disciplinasCacheString))
                return JsonConvert.DeserializeObject<List<DisciplinaDto>>(disciplinasCacheString);

            var disciplinas = ehPefilCJ ? await ObterDisciplinasPerfilCJ(codigoTurma, login) :
                await servicoEOL.ObterDisciplinasPorCodigoTurmaLoginEPerfil(codigoTurma, login, perfilAtual);

            if (disciplinas == null || !disciplinas.Any())
                return disciplinasDto;

            disciplinasDto = MapearParaDto(disciplinas, turmaPrograma);

            await repositorioCache.SalvarAsync(chaveCache, JsonConvert.SerializeObject(disciplinasDto));

            return disciplinasDto;
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
                    disciplinasDto = MapearParaDto(disciplinas, turmaPrograma);

                    await repositorioCache.SalvarAsync(chaveCache, JsonConvert.SerializeObject(disciplinasDto));
                }
            }
            return disciplinasDto;
        }

        public IEnumerable<DisciplinaResposta> MapearComponentes(IEnumerable<ComponenteCurricularEol> componentesCurriculares)
        {
            foreach (var componenteCurricular in componentesCurriculares)
                yield return new DisciplinaResposta()
                {
                    CodigoComponenteCurricularPai = componenteCurricular.CodigoComponenteCurricularPai,
                    CodigoComponenteCurricular = componenteCurricular.Codigo,
                    Nome = componenteCurricular.Descricao,
                    Regencia = componenteCurricular.Regencia,
                    TerritorioSaber = componenteCurricular.TerritorioSaber,
                    Compartilhada = componenteCurricular.Compartilhada,
                    LancaNota = componenteCurricular.LancaNota,
                };
        }

        private DisciplinaResposta MapearDisciplinaResposta(DisciplinaDto disciplinaEol) => new DisciplinaResposta()
        {
            CodigoComponenteCurricular = disciplinaEol.CodigoComponenteCurricular,
            CodigoComponenteCurricularPai = disciplinaEol.CdComponenteCurricularPai,
            Nome = disciplinaEol.Nome,
            Regencia = disciplinaEol.Regencia,
            Compartilhada = disciplinaEol.Compartilhada,
            RegistroFrequencia = disciplinaEol.RegistraFrequencia,
            LancaNota = disciplinaEol.LancaNota,
        };

        private List<DisciplinaDto> MapearParaDto(IEnumerable<DisciplinaResposta> disciplinas, bool turmaPrograma = false)
        {
            var retorno = new List<DisciplinaDto>();

            if (disciplinas != null)
            {
                foreach (var disciplina in disciplinas)
                {
                    retorno.Add(MapearParaDto(disciplina, turmaPrograma));
                }
            }
            return retorno;
        }

        private DisciplinaDto MapearParaDto(DisciplinaResposta disciplina, bool turmaPrograma = false) => new DisciplinaDto()
        {
            CdComponenteCurricularPai = disciplina.CodigoComponenteCurricularPai,
            CodigoComponenteCurricular = disciplina.CodigoComponenteCurricular,
            Nome = disciplina.Nome,
            Regencia = disciplina.Regencia,
            TerritorioSaber = disciplina.TerritorioSaber,
            Compartilhada = disciplina.Compartilhada,
            RegistraFrequencia = disciplina.RegistroFrequencia,
            LancaNota = disciplina.LancaNota,
            PossuiObjetivos = !turmaPrograma && consultasObjetivoAprendizagem
            .DisciplinaPossuiObjetivosDeAprendizagem(disciplina.CodigoComponenteCurricular)
        };

        private IEnumerable<DisciplinaResposta> TransformarListaDisciplinaEolParaRetornoDto(IEnumerable<DisciplinaDto> disciplinasEol)
        {
            foreach (var disciplinaEol in disciplinasEol)
            {
                yield return MapearDisciplinaResposta(disciplinaEol);
            }
        }

        private IEnumerable<DisciplinaDto> TratarRetornoDisciplinasPlanejamento(IEnumerable<DisciplinaDto> disciplinas, long codigoDisciplina, bool regencia)
        {
            if (codigoDisciplina == 0)
                return disciplinas;

            if (regencia)
                return disciplinas.Where(x => x.Regencia);

            return disciplinas.Where(x => x.CodigoComponenteCurricular == codigoDisciplina);
        }
    }
}