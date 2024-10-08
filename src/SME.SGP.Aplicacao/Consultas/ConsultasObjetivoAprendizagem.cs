﻿using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
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
        private readonly IConfiguration configuration;
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioComponenteCurricularJurema repositorioComponenteCurricular;
        private readonly IRepositorioObjetivoAprendizagem repositorioObjetivoAprendizagem;
        private readonly IRepositorioObjetivoAprendizagemPlano repositorioObjetivosPlano;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IConsultasTurma consultasTurma;
        
        public ConsultasObjetivoAprendizagem(IRepositorioCache repositorioCache,
                                                     IRepositorioComponenteCurricularJurema repositorioComponenteCurricular,
                                                     IRepositorioObjetivoAprendizagemPlano repositorioObjetivosPlano,
                                                     IConfiguration configuration,
                                                     IConsultasPeriodoEscolar consultasPeriodoEscolar,
                                                     IConsultasTurma consultasTurma,
                                                     IRepositorioObjetivoAprendizagem repositorioObjetivoAprendizagem)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.consultasTurma = consultasTurma ?? throw new ArgumentNullException(nameof(consultasTurma));
            this.repositorioObjetivoAprendizagem = repositorioObjetivoAprendizagem ?? throw new ArgumentNullException(nameof(repositorioObjetivoAprendizagem));
            this.repositorioObjetivosPlano = repositorioObjetivosPlano ?? throw new ArgumentNullException(nameof(repositorioObjetivosPlano));
        }

        public bool DisciplinaPossuiObjetivosDeAprendizagem(long codigoDisciplina)
        {
            IEnumerable<ComponenteCurricularJurema> componentesCurriculares = ObterComponentesCurriculares();

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

            return await repositorioCache.ObterAsync(NomeChaveCache.OBJETIVOS_APRENDIZAGEM, () => ListarSemCache(), tempoExpiracao, true);
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

        public bool ComponentePossuiObjetivosOpcionais(long componenteCurricularCodigo, bool regencia, bool turmaEspecial)
        {
            return turmaEspecial && (regencia || new long[] { 218, 138, 1116 }.Contains(componenteCurricularCodigo));
        }

        private async Task<int> ObterBimestreAtual(DateTime dataReferencia, string turmaId)
        {
            Turma turma = await consultasTurma.ObterComUeDrePorCodigo(turmaId);

            if (turma.EhNulo())
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

        private IEnumerable<ComponenteCurricularJurema> ObterComponentesCurriculares()
        {
            IEnumerable<ComponenteCurricularJurema> componentesCurriculares = repositorioComponenteCurricular.Listar();
            if (componentesCurriculares.EhNulo())
            {
                throw new NegocioException("Não foi possível recuperar a lista de componentes curriculares.");
            }

            return componentesCurriculares;
        }

        private IEnumerable<long> ObterComponentesJuremaPorIdEOL(IEnumerable<long> componentesCurricularesIds)
        {
            IEnumerable<ComponenteCurricularJurema> componentesCurriculares = ObterComponentesCurriculares();

            IEnumerable<ComponenteCurricularJurema> componentesFiltro = componentesCurriculares.Where(c => componentesCurricularesIds.Contains(c.CodigoEOL));
            IEnumerable<long> componentesJurema = componentesFiltro.Select(c => c.CodigoJurema);
            return componentesJurema;
        }
    }
}