using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasObjetivoAprendizagem : IConsultasObjetivoAprendizagem
    {
        private readonly Dictionary<int, string> Anos = new Dictionary<int, string>
        {
           {1,"first"},{2,"second"},{3,"third"},{4,"fourth"},{5,"fifth"},{6,"sixth"},{7,"seventh"},{8,"eighth"},{9,"nineth"}
        };

        private readonly IConfiguration configuration;
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioComponenteCurricular repositorioComponenteCurricular;
        private readonly IRepositorioObjetivoAprendizagem repositorioObjetivoAprendizagem;
        private readonly IRepositorioObjetivoAprendizagemPlano repositorioObjetivosPlano;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IConsultasTurma consultasTurma;
        private readonly IServicoJurema servicoJurema;
        private readonly IServicoEol servicoEol;
        private readonly IServicoUsuario servicoUsuario;

        public ConsultasObjetivoAprendizagem(IServicoJurema servicoJurema,
                                                     IRepositorioCache repositorioCache,
                                                     IRepositorioComponenteCurricular repositorioComponenteCurricular,
                                                     IRepositorioObjetivoAprendizagemPlano repositorioObjetivosPlano,
                                                     IConfiguration configuration,
                                                     IServicoUsuario servicoUsuario,
                                                     IConsultasPeriodoEscolar consultasPeriodoEscolar,
                                                     IConsultasTurma consultasTurma,
                                                     IRepositorioObjetivoAprendizagem repositorioObjetivoAprendizagem,
                                                     IServicoEol servicoEol)
        {
            this.servicoJurema = servicoJurema ?? throw new ArgumentNullException(nameof(servicoJurema));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.consultasTurma = consultasTurma ?? throw new ArgumentNullException(nameof(consultasTurma));
            this.repositorioObjetivoAprendizagem = repositorioObjetivoAprendizagem ?? throw new ArgumentNullException(nameof(repositorioObjetivoAprendizagem));
            this.repositorioObjetivosPlano = repositorioObjetivosPlano ?? throw new ArgumentNullException(nameof(repositorioObjetivosPlano));
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
        }

        public bool DisciplinaPossuiObjetivosDeAprendizagem(long codigoDisciplina)
        {
            IEnumerable<ComponenteCurricular> componentesCurriculares = ObterComponentesCurriculares();

            return componentesCurriculares.Any(x => x.CodigoEOL == codigoDisciplina);
        }

        public async Task<IEnumerable<ObjetivoAprendizagemDto>> Filtrar(FiltroObjetivosAprendizagemDto filtroObjetivosAprendizagemDto)
        {
            IEnumerable<ObjetivoAprendizagemDto> objetivos = await Listar();
            IEnumerable<long> componentesJurema = ObterComponentesJuremaPorIdEOL(filtroObjetivosAprendizagemDto.ComponentesCurricularesIds);

            IEnumerable<ObjetivoAprendizagemDto> result = null;

            if (filtroObjetivosAprendizagemDto.ComponentesCurricularesIds.Contains(138))
            {
                result = objetivos?.Where(c => c.IdComponenteCurricular == (filtroObjetivosAprendizagemDto.EnsinoEspecial ? 11 : 6));
            }
            else
            {
                result = objetivos?.Where(c => componentesJurema.Contains(c.IdComponenteCurricular));
            }

            var listResult = result.ToList();
            foreach (var item in listResult)
            {
                item.ComponenteCurricularEolId = filtroObjetivosAprendizagemDto.ComponentesCurricularesIds.FirstOrDefault();
            }

            IEnumerable<int> anos = Enumerable.Range(1, 9);
            if (filtroObjetivosAprendizagemDto.EnsinoEspecial && !anos.Select(a => a.ToString()).Contains(filtroObjetivosAprendizagemDto.Ano))
            {
                return listResult.OrderBy(o => o.Ano).ThenBy(x => x.Codigo);
            }

            listResult = listResult.Where(x => x.Ano == filtroObjetivosAprendizagemDto.Ano).ToList();
            return listResult.OrderBy(o => o.Codigo);
        }

        public async Task<IEnumerable<ObjetivoAprendizagemDto>> Listar()
        {
            int tempoExpiracao = int.Parse(configuration.GetSection("ExpiracaoCache").GetSection("ObjetivosAprendizagem").Value);

            return await repositorioCache.ObterAsync("ObjetivosAprendizagem", () => ListarSemCache(), tempoExpiracao, true);
        }

        public async Task<ObjetivoAprendizagemSimplificadoDto> ObterAprendizagemSimplificadaPorId(long id)
        {
            IEnumerable<ObjetivoAprendizagemDto> lstObjAprendizagemDtos = await Listar();

            ObjetivoAprendizagemDto objetivoDto = lstObjAprendizagemDtos.FirstOrDefault(obj => obj.Id == id);

            return new ObjetivoAprendizagemSimplificadoDto()
            {
                Id = objetivoDto.Id,
                IdComponenteCurricular = objetivoDto.IdComponenteCurricular
            };
        }

        public async Task<IEnumerable<ComponenteCurricularSimplificadoDto>> ObterDisciplinasDoBimestrePlanoAnual(DateTime dataReferencia, long turmaId, long componenteCurricularId)
        {
            int bimestre = await ObterBimestreAtual(dataReferencia, turmaId.ToString());

            return repositorioObjetivosPlano.ObterDisciplinasDoBimestrePlanoAula(dataReferencia.Year, bimestre, turmaId, componenteCurricularId);
        }

        public long ObterIdPorObjetivoAprendizagemJurema(long planoId, long objetivoAprendizagemJuremaId)
        {
            return repositorioObjetivosPlano.ObterIdPorObjetivoAprendizagemJurema(planoId, objetivoAprendizagemJuremaId);
        }


        public async Task<IEnumerable<ObjetivoAprendizagemDto>> ObterObjetivosPlanoDisciplina(DateTime dataReferencia, long turmaId, long componenteCurricularId, long disciplinaId, bool regencia = false)
        {
            Usuario usuarioLogado = await servicoUsuario.ObterUsuarioLogado();

            int bimestre = await ObterBimestreAtual(dataReferencia, turmaId.ToString());

            bool filtrarSomenteRegencia = regencia && !usuarioLogado.EhProfessorCj();
            IEnumerable<ObjetivoAprendizagemPlano> objetivosPlano = repositorioObjetivosPlano.ObterObjetivosPlanoDisciplina(dataReferencia.Year,
                                                                                         bimestre,
                                                                                         turmaId,
                                                                                         componenteCurricularId,
                                                                                         disciplinaId,
                                                                                         filtrarSomenteRegencia);

            IEnumerable<ObjetivoAprendizagemDto> objetivosJurema = await Listar();

            // filtra objetivos do jurema com os objetivos cadastrados no plano anual nesse bimestre
            return objetivosJurema.
                Where(c => objetivosPlano.Any(o => o.ObjetivoAprendizagemJuremaId == c.Id)).OrderBy(c => c.Codigo);
        }


        private async Task<int> ObterBimestreAtual(DateTime dataReferencia, string turmaId)
        {
            Turma turma = await consultasTurma.ObterComUeDrePorCodigo(turmaId);

            if (turma == null)
                throw new NegocioException("Turma não encontrada para consulta de objetivos de aprendizagem");

            return await consultasPeriodoEscolar.ObterBimestre(dataReferencia, turma.ModalidadeCodigo, turma.Semestre);
        }

        private async Task<List<ObjetivoAprendizagemDto>> ListarSemCache()
        {
            IEnumerable<ObjetivoAprendizagem> objetivosJuremaDto = await repositorioObjetivoAprendizagem.ListarAsync();
            return MapearParaDto(objetivosJuremaDto).ToList();
        }

        private IEnumerable<ObjetivoAprendizagemDto> MapearParaDto(IEnumerable<ObjetivoAprendizagem> objetivos)
        {
            foreach (ObjetivoAprendizagem objetivoBase in objetivos)
            {
                if (objetivoBase.Ano != 0)
                {
                    yield return new ObjetivoAprendizagemDto()
                    {
                        Descricao = objetivoBase.Descricao,
                        Id = objetivoBase.Id,
                        Ano = objetivoBase.Ano.ToString(),
                        Codigo = objetivoBase.Codigo,
                        IdComponenteCurricular = objetivoBase.ComponenteCurricularId
                    };
                }
            }
        }

        private IEnumerable<ComponenteCurricular> ObterComponentesCurriculares()
        {
            IEnumerable<ComponenteCurricular> componentesCurriculares = repositorioComponenteCurricular.Listar();
            if (componentesCurriculares == null)
            {
                throw new NegocioException("Não foi possível recuperar a lista de componentes curriculares.");
            }

            return componentesCurriculares;
        }

        private IEnumerable<long> ObterComponentesJuremaPorIdEOL(IEnumerable<long> componentesCurricularesIds)
        {
            IEnumerable<ComponenteCurricular> componentesCurriculares = ObterComponentesCurriculares();

            IEnumerable<ComponenteCurricular> componentesFiltro = componentesCurriculares.Where(c => componentesCurricularesIds.Contains(c.CodigoEOL));
            IEnumerable<long> componentesJurema = componentesFiltro.Select(c => c.CodigoJurema);
            return componentesJurema;
        }

        public async Task<bool> ComponentePossuiObjetivosOpcionais(long componenteCurricularCodigo, bool regencia, bool turmaEspecial)
        {
            return turmaEspecial && (regencia || new long[] { 218, 138, 1116 }.Contains(componenteCurricularCodigo));
        }
    }
}