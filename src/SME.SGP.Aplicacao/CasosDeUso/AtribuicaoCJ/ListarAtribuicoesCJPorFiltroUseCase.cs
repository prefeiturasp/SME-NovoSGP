using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarAtribuicoesCJPorFiltroUseCase : AbstractUseCase, IListarAtribuicoesCJPorFiltroUseCase
    {
        public ListarAtribuicoesCJPorFiltroUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<IEnumerable<AtribuicaoCJListaRetornoDto>> Executar(AtribuicaoCJListaFiltroDto filtroDto)
        {
            var listaRetorno = (await mediator.Send(new ObterAtribuicoesPorTurmaEProfessorQuery(null, null, filtroDto.UeId, 0,
                filtroDto.UsuarioRf, filtroDto.UsuarioNome, true, anoLetivo: filtroDto.AnoLetivo, historico: filtroDto.Historico))).ToList();

            if (listaRetorno.Any())
                return await TransformaEntidadesEmDtosListaRetorno(listaRetorno);
            else
                return Enumerable.Empty<AtribuicaoCJListaRetornoDto>();
        }

        private async Task<IEnumerable<AtribuicaoCJListaRetornoDto>> TransformaEntidadesEmDtosListaRetorno(IEnumerable<AtribuicaoCJ> listaDto)
        {
            var idsDisciplinas = listaDto
                .Select(a => a.DisciplinaId)
                .Distinct<long>()
                .ToArray();

            var professoresDisciplinas = listaDto
                .GroupBy(a => new { a.ProfessorRf, a.Modalidade, a.TurmaId, a.UeId }).ToList();

            var listRetorno = new List<AtribuicaoCJListaRetornoDto>();

            foreach (var profDisciplina in professoresDisciplinas)
            {
                var disciplinasIds = profDisciplina.Select(b => b.DisciplinaId);
                var disciplinasEol = await mediator.Send(new ObterDisciplinasPorCodigoTurmaQuery(profDisciplina.Key.TurmaId));

                if (!disciplinasEol.Any())
                    throw new NegocioException("Não foi possível obter as descrições das disciplinas no Eol.");

                var disciplinasDescricoes = disciplinasEol
                    .Where(c => disciplinasIds.Contains(c.CodigoComponenteCurricular) ||
                               (c.TerritorioSaber && (disciplinasIds.Contains(c.CodigoComponenteTerritorioSaber.Value) || disciplinasIds.Intersect(c.CodigosTerritoriosAgrupamento).Any())))
                    .ToList();

                var professorDisciplina = profDisciplina.FirstOrDefault();

                var exibeNomeTurmaNovoInfantil = professorDisciplina != null && professorDisciplina.Turma.ModalidadeCodigo == Modalidade.EducacaoInfantil && professorDisciplina.Turma.AnoLetivo >= DateTime.Now.Year;

                var atribuicaoDto = new AtribuicaoCJListaRetornoDto()
                {
                    Modalidade = profDisciplina.Key.Modalidade.GetAttribute<DisplayAttribute>().Name,
                    ModalidadeId = (int)profDisciplina.Key.Modalidade,
                    Turma = professorDisciplina?.Turma.Nome,
                    TurmaId = professorDisciplina?.TurmaId,
                    Disciplinas = exibeNomeTurmaNovoInfantil
                    ? disciplinasDescricoes.Select(d => d.NomeComponenteInfantil ?? d.Nome).Distinct().ToArray()
                    : disciplinasDescricoes.Select(d => d.Nome).Distinct().ToArray(),
                    ProfessorRf = professorDisciplina.ProfessorRf
                };

                listRetorno.Add(atribuicaoDto);
            }
            return listRetorno;
        }
    }
}
