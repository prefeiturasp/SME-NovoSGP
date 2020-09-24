using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorTurmaIdUseCase : IObterComponentesCurricularesPorTurmaIdUseCase
    {
        private readonly IMediator mediator;

        public ObterComponentesCurricularesPorTurmaIdUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        //public async Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasPerfilCJ(string codigoTurma, string login)
        //{
        //    var atribuicoes = await repositorioAtribuicaoCJ.ObterPorFiltros(null, codigoTurma, string.Empty, 0, login, string.Empty, true);

        //    if (atribuicoes == null || !atribuicoes.Any())
        //        return null;

        //    var disciplinasEol = servicoEOL.ObterDisciplinasPorIds(atribuicoes.Select(a => a.DisciplinaId).Distinct().ToArray());

        //    return TransformarListaDisciplinaEolParaRetornoDto(disciplinasEol);
        //}
        //TODO: FEITO COM CODIGO E ID POIS PRECISAMOS ACESSAR O EOL PARA OBTER OS COMPONENTES CURRICULARES;
        // APÓS AS ATRIBUIÇÕES ESTAREM NO SGP, REMOVER O CÓDIGO E VIRAR TUDO PARA ID
        public async Task<IEnumerable<DisciplinaDto>> Executar(long turmaId, bool turmaPrograma)
        {
            var usuarioLogadoPerfil = await mediator.Send(new ObterUsuarioLogadoPerfilAtualQuery());
            var usuarioRF = await mediator.Send(new ObterUsuarioLogadoRFQuery());


            //TODO: SE FORNECER MODALIDADE/ANO/CODIGO DA TURMA, NÃO SERA NECESSARIO BUSCAR TURMA;
            var turmas = await mediator.Send(new ObterTurmaPorIdsQuery(new long[] { turmaId }));
            if (turmas == null || !turmas.Any())
                throw new NegocioException("Não foi possível obter a turma");


            var turma = turmas.FirstOrDefault();

            List<DisciplinaDto> disciplinasDto = new List<DisciplinaDto>();


            var usuario = new Usuario();
            usuario.PerfilAtual = usuarioLogadoPerfil;

            //if (!usuario.EhProfessor())
            //{
            //    //var disciplinasCacheString = await repositorioCache.ObterAsync(chaveCache);

            //    //if (!string.IsNullOrWhiteSpace(disciplinasCacheString))
            //    //    return JsonConvert.DeserializeObject<List<DisciplinaDto>>(disciplinasCacheString);
            //}

            //var turma = await repositorioTurma.ObterPorCodigo(codigoTurma);
            //if (turma == null)
            //    throw new NegocioException("Não foi possível encontrar a turma");

            if (usuario.EhProfessorCj())
            {
                //var disciplinas = await ObterDisciplinasPerfilCJ(codigoTurma, usuarioLogado.Login);
                //disciplinasDto = MapearParaDto(disciplinas, turmaPrograma, turma.EnsinoEspecial);
            }
            else
            {
                var componentesCurricularesIds = await mediator.Send(new ObterComponentesCurricularesIdsDoProfessorNaTurmaQuery(usuarioRF, long.Parse(turma.CodigoTurma), usuarioLogadoPerfil));

                var componentesCurriculares = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(componentesCurricularesIds));

                disciplinasDto = componentesCurriculares?.Select(componenteCurricular => new DisciplinaDto()
                {
                    //CdComponenteCurricularPai = disciplina.ComponenteCurricularPaiId,
                    CodigoComponenteCurricular = componenteCurricular.Id,
                    Nome = componenteCurricular.Descricao,
                    Regencia = componenteCurricular.EhRegenciaClasse,
                    TerritorioSaber = componenteCurricular.EhTerritorio,
                    Compartilhada = componenteCurricular.EhCompatilhado,
                    LancaNota = componenteCurricular.PermiteLancamentoNota,
                    PossuiObjetivos = componenteCurricular.PossuiObjetivosDeAprendizagem(turmaPrograma, turma.ModalidadeCodigo, turma.Ano),
                    ObjetivosAprendizagemOpcionais = componenteCurricular.PossuiObjetivosDeAprendizagemOpcionais(turma.EnsinoEspecial),
                    RegistraFrequencia = componenteCurricular.PermiteRegistroFrequencia
                })?.ToList();

                //if (!usuarioLogado.EhProfessor())
                //    await repositorioCache.SalvarAsync(chaveCache, JsonConvert.SerializeObject(disciplinasDto));
            }

            return disciplinasDto;
        }
    }
}
