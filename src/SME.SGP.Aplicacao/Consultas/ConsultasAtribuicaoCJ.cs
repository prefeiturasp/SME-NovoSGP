using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasAtribuicaoCJ : IConsultasAtribuicaoCJ
    {
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;

        private readonly IServicoEOL servicoEOL;

        public ConsultasAtribuicaoCJ(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ, IServicoEOL servicoEOL)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<IEnumerable<AtribuicaoCJListaRetornoDto>> Listar(AtribuicaoCJListaFiltroDto filtroDto)
        {
            var listaRetorno = await repositorioAtribuicaoCJ.ObterPorFiltros(null, null, filtroDto.UeId, 0,
                filtroDto.UsuarioRf, filtroDto.UsuarioNome, true, anoLetivo: filtroDto.AnoLetivo);

            if (listaRetorno.Any())
                return TransformaEntidadesEmDtosListaRetorno(listaRetorno);
            else return null;
        }

        public async Task<AtribuicaoCJTitularesRetornoDto> ObterProfessoresTitularesECjs(string ueId, string turmaId,
            string professorRf, Modalidade modalidadeId)
        {
            IEnumerable<ProfessorTitularDisciplinaEol> professoresTitularesDisciplinasEol = await servicoEOL.ObterProfessoresTitularesDisciplinas(turmaId, professorRf);

            var listaAtribuicoes = await repositorioAtribuicaoCJ.ObterPorFiltros(modalidadeId, turmaId, ueId, 0,
                professorRf, string.Empty, null);

            if (professoresTitularesDisciplinasEol.Any())
                return TransformaEntidadesEmDtosAtribuicoesProfessoresRetorno(listaAtribuicoes, professoresTitularesDisciplinasEol);
            else return null;
        }

        private AtribuicaoCJTitularesRetornoDto TransformaEntidadesEmDtosAtribuicoesProfessoresRetorno(IEnumerable<AtribuicaoCJ> listaAtribuicoes, IEnumerable<ProfessorTitularDisciplinaEol> professoresTitularesDisciplinasEol)
        {
            var listaRetorno = new AtribuicaoCJTitularesRetornoDto();

            foreach (var disciplinaProfessorTitular in professoresTitularesDisciplinasEol)
            {
                var atribuicao = listaAtribuicoes.FirstOrDefault(b => b.DisciplinaId == disciplinaProfessorTitular.DisciplinaId);

                listaRetorno.Itens.Add(new AtribuicaoCJTitularesRetornoItemDto()
                {
                    Disciplina = disciplinaProfessorTitular.DisciplinaNome,
                    DisciplinaId = disciplinaProfessorTitular.DisciplinaId,
                    ProfessorTitular = disciplinaProfessorTitular.ProfessorNome,
                    ProfessorTitularRf = disciplinaProfessorTitular.ProfessorRf,
                    Substituir = atribuicao == null ? false : atribuicao.Substituir
                });
            }

            if (listaAtribuicoes.Any())
            {
                var ultimoRegistroAlterado = listaAtribuicoes
                    .OrderBy(b => b.AlteradoEm)
                    .ThenBy(b => b.CriadoEm).FirstOrDefault();

                listaRetorno.CriadoEm = ultimoRegistroAlterado.CriadoEm;
                listaRetorno.CriadoPor = ultimoRegistroAlterado.CriadoPor;
                listaRetorno.AlteradoEm = ultimoRegistroAlterado.AlteradoEm;
                listaRetorno.AlteradoPor = ultimoRegistroAlterado.AlteradoPor;
            }

            return listaRetorno;
        }

        private IEnumerable<AtribuicaoCJListaRetornoDto> TransformaEntidadesEmDtosListaRetorno(IEnumerable<AtribuicaoCJ> listaDto)
        {
            var idsDisciplinas = listaDto
                .Select(a => a.DisciplinaId)
                .Distinct<long>()
                .ToArray();

            var disciplinasEol = servicoEOL.ObterDisciplinasPorIds(idsDisciplinas);

            if (!disciplinasEol.Any())
                throw new NegocioException("Não foi possível obter as descrições das disciplinas no Eol.");

            var professoresDisciplinas = listaDto
                .GroupBy(a => new { a.ProfessorRf, a.Modalidade, a.TurmaId, a.UeId }).ToList();

            var listRetorno = new List<AtribuicaoCJListaRetornoDto>();

            professoresDisciplinas.ForEach(a =>
            {
                var disciplinasIds = a.Select(b => b.DisciplinaId);

                var disciplinasDescricoes = disciplinasEol
                            .Where(c => disciplinasIds.Contains(c.CodigoComponenteCurricular))
                            .ToList();

                var professorDisciplina = a.FirstOrDefault();

                var atribuicaoDto = new AtribuicaoCJListaRetornoDto()
                {
                    Modalidade = a.Key.Modalidade.GetAttribute<DisplayAttribute>().Name,
                    ModalidadeId = (int)a.Key.Modalidade,
                    Turma = professorDisciplina.Turma.Nome,
                    TurmaId = professorDisciplina.TurmaId,
                    Disciplinas = disciplinasDescricoes.Select(d => d.Nome).ToArray()
                };

                listRetorno.Add(atribuicaoDto);
            });

            return listRetorno;
        }
    }
}