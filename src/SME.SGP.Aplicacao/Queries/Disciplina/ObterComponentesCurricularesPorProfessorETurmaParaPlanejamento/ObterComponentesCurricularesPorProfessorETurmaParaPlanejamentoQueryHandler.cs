using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorProfessorETurmaParaPlanejamentoQueryHandler : IRequestHandler<ObterComponentesCurricularesPorProfessorETurmaParaPlanejamentoQuery, IEnumerable<DisciplinaDto>>
    {
        private static readonly long[] IDS_COMPONENTES_REGENCIA = { 2, 7, 8, 89, 138 };
        private readonly IMediator mediator;
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;
        private readonly IRepositorioComponenteCurricularJurema repositorioComponenteCurricularJurema;
        private readonly IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem;

        public ObterComponentesCurricularesPorProfessorETurmaParaPlanejamentoQueryHandler(
                                                           IMediator mediator,
                                                           IRepositorioCache repositorioCache,
                                                           IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ,
                                                           IRepositorioComponenteCurricularJurema repositorioComponenteCurricularJurema,
                                                           IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
            this.repositorioComponenteCurricularJurema = repositorioComponenteCurricularJurema ?? throw new System.ArgumentNullException(nameof(repositorioComponenteCurricularJurema));
            this.consultasObjetivoAprendizagem = consultasObjetivoAprendizagem ?? throw new System.ArgumentNullException(nameof(consultasObjetivoAprendizagem));
        }

        public async Task<IEnumerable<DisciplinaDto>> Handle(ObterComponentesCurricularesPorProfessorETurmaParaPlanejamentoQuery request, CancellationToken cancellationToken)
        {
            List<DisciplinaDto> disciplinasDto;
            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            var dataInicioNovoSGP = await mediator.Send(new ObterParametroSistemaPorTipoQuery(TipoParametroSistema.DataInicioSGP));

            var chaveCache = string.Format(NomeChaveCache.COMPONENTES_PLANEJAMENTO_TURMA_COMPONENTE_PERFIL, request.CodigoTurma, request.CodigoDisciplina, usuario.PerfilAtual);
            if (!usuario.EhProfessor() && !usuario.EhProfessorCj() && !usuario.EhProfessorPoa())
            {
                var disciplinasCacheString = await repositorioCache.ObterAsync(chaveCache);

                if (!string.IsNullOrWhiteSpace(disciplinasCacheString))
                {
                    disciplinasDto = JsonConvert.DeserializeObject<List<DisciplinaDto>>(disciplinasCacheString);
                    var disciplinas = await TratarRetornoDisciplinasPlanejamento(disciplinasDto, request.CodigoDisciplina, request.Regencia, request.CodigoTurma);
                    return disciplinas?.OrderBy(c => c.Nome)?.ToList();
                }
            }

            var componentesCurricularesJurema = await repositorioCache.ObterAsync(NomeChaveCache.COMPONENTES_JUREMA, () => Task.FromResult(repositorioComponenteCurricularJurema.Listar()));
            if (componentesCurricularesJurema.EhNulo())
            {
                throw new NegocioException("Não foi possível recuperar a lista de componentes   curriculares.");
            }

            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(request.CodigoTurma));
            if (turma.EhNulo())
                throw new NegocioException("Não foi possível encontrar a turma");

            if (usuario.EhProfessorCj())
            {
                var componentesCJ = await ObterComponentesCJ(null, request.CodigoTurma,
                    string.Empty,
                    request.CodigoDisciplina,
                    usuario.Login);
                disciplinasDto = (MapearParaDto(componentesCJ))?.OrderBy(c => c.Nome)?.ToList();
            }
            else
            {
                var componentesCurriculares = await mediator.Send(new ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery(request.CodigoTurma, usuario.Login, usuario.PerfilAtual));

                if (turma.ModalidadeCodigo == Modalidade.EJA)
                    componentesCurriculares = RemoverEdFisicaEJA(componentesCurriculares);

                disciplinasDto = (await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(componentesCurriculares.Select(a => a.Codigo).ToArray())))?.OrderBy(c => c.Nome)?.ToList();
                    
                disciplinasDto.ForEach(d =>
                {
                    var componenteEOL = componentesCurriculares.FirstOrDefault(a => a.Codigo == d.CodigoComponenteCurricular);
                    d.PossuiObjetivos = turma.AnoLetivo < Convert.ToInt32(dataInicioNovoSGP) ? false : componenteEOL.PossuiObjetivosDeAprendizagem(componentesCurricularesJurema, turma.ModalidadeCodigo);
                    d.Regencia = componenteEOL.Regencia;
                    d.ObjetivosAprendizagemOpcionais = componenteEOL.PossuiObjetivosDeAprendizagemOpcionais(componentesCurricularesJurema, turma.EnsinoEspecial);
                });
            }

            if (!usuario.EhProfessor() && !usuario.EhProfessorCj() && !usuario.EhProfessorPoa())
                await repositorioCache.SalvarAsync(chaveCache, JsonConvert.SerializeObject(disciplinasDto));

            return await TratarRetornoDisciplinasPlanejamento(disciplinasDto, request.CodigoDisciplina, request.Regencia, request.CodigoTurma, usuario.EhProfessorCj());
        }

        private async Task<IEnumerable<DisciplinaDto>> TratarRetornoDisciplinasPlanejamento(IEnumerable<DisciplinaDto> disciplinas, long codigoDisciplina, bool regencia, string codigoTurma = "", bool CJ = false)
        {
            if (codigoDisciplina == 0 && !regencia)
                return disciplinas;

            if (regencia)
            {
                if (!codigoTurma.Equals(""))
                {
                    var regencias = await mediator.Send(new ObterComponentesCurricularesRegenciaPorTurmaCodigoQuery(codigoTurma));
                    return CJ ? disciplinas.Where(x => regencias.Any(c => c.CodigoComponenteCurricular == x.CodigoComponenteCurricular))
                        : disciplinas.Where(x => x.Regencia && regencias.Any(c => c.CodigoComponenteCurricular == x.CodigoComponenteCurricular));
                }
                return CJ ? disciplinas : disciplinas.Where(x => x.Regencia);
            }

            return disciplinas.Where(x => x.CodigoComponenteCurricular == codigoDisciplina);
        }

        private async Task<IEnumerable<DisciplinaResposta>> ObterComponentesCJ(Modalidade? modalidade, string codigoTurma, string ueId, long codigoDisciplina, string rf, bool ignorarDeParaRegencia = false)
        {
            IEnumerable<DisciplinaResposta> componentes = null;
            var atribuicoes = await repositorioAtribuicaoCJ.ObterPorFiltros(modalidade,
                codigoTurma,
                ueId,
                codigoDisciplina,
                rf,
                string.Empty,
                true);
            if (atribuicoes.EhNulo() || !atribuicoes.Any())
                return null;

            var disciplinasEol = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(atribuicoes.Select(a => a.DisciplinaId).Distinct().ToArray()));
                
            var componenteRegencia = disciplinasEol?.FirstOrDefault(c => c.Regencia);
            if (componenteRegencia.EhNulo() || ignorarDeParaRegencia)
                return TransformarListaDisciplinaEolParaRetornoDto(disciplinasEol);

            var componentesRegencia = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(IDS_COMPONENTES_REGENCIA));
            if (componentesRegencia.NaoEhNulo())
                return TransformarListaDisciplinaEolParaRetornoDto(componentesRegencia);

            return componentes;
        }

        private List<DisciplinaDto> MapearParaDto(IEnumerable<DisciplinaResposta> disciplinas, bool ensinoEspecial = false)
        {
            var retorno = new List<DisciplinaDto>();

            if (disciplinas.NaoEhNulo())
            {
                foreach (var disciplina in disciplinas)
                {
                    retorno.Add(MapearParaDto(disciplina, ensinoEspecial));
                }
            }
            return retorno;
        }

        private DisciplinaDto MapearParaDto(DisciplinaResposta disciplina, bool ensinoEspecial = false) => new DisciplinaDto()
        {
            Id = disciplina.Id,
            CdComponenteCurricularPai = disciplina.CodigoComponenteCurricularPai,
            CodigoComponenteCurricular = disciplina.CodigoComponenteCurricular,
            Nome = disciplina.Nome,
            NomeComponenteInfantil = disciplina.NomeComponenteInfantil,
            Regencia = disciplina.Regencia,
            TerritorioSaber = disciplina.TerritorioSaber,
            Compartilhada = disciplina.Compartilhada,
            RegistraFrequencia = disciplina.RegistroFrequencia,
            LancaNota = disciplina.LancaNota,
            PossuiObjetivos = consultasObjetivoAprendizagem.DisciplinaPossuiObjetivosDeAprendizagem(disciplina.CodigoComponenteCurricular),
            ObjetivosAprendizagemOpcionais = consultasObjetivoAprendizagem.ComponentePossuiObjetivosOpcionais(disciplina.CodigoComponenteCurricular, disciplina.Regencia, ensinoEspecial)
        };

        private IEnumerable<DisciplinaResposta> TransformarListaDisciplinaEolParaRetornoDto(IEnumerable<DisciplinaDto> disciplinasEol)
        {
            foreach (var disciplinaEol in disciplinasEol)
            {
                yield return MapearDisciplinaResposta(disciplinaEol);
            }
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
            NomeComponenteInfantil = disciplinaEol.NomeComponenteInfantil,
            Id = disciplinaEol.Id,
            TerritorioSaber = disciplinaEol.TerritorioSaber
        };

        private IEnumerable<ComponenteCurricularEol> RemoverEdFisicaEJA(IEnumerable<ComponenteCurricularEol> componentesCurriculares)
            => componentesCurriculares.Where(c => c.Codigo != MensagemNegocioComponentesCurriculares.COMPONENTE_CURRICULAR_CODIGO_ED_FISICA);
    }
}
