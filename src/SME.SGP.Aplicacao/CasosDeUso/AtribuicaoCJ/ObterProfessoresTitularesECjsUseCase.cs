using MediatR;
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
    public class ObterProfessoresTitularesECjsUseCase : AbstractUseCase, IObterProfessoresTitularesECjsUseCase
    {

        private readonly IServicoEol servicoEOL;
        private readonly IConsultasDisciplina consultasDisciplina;
        public ObterProfessoresTitularesECjsUseCase(IMediator mediator, IServicoEol servicoEOL, IConsultasDisciplina consultasDisciplina) : base(mediator)
        {
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.consultasDisciplina = consultasDisciplina ?? throw new ArgumentNullException(nameof(consultasDisciplina));
        }

        public async Task<AtribuicaoCJTitularesRetornoDto> Executar(string ueId, string turmaId,
                    string professorRf, Modalidade modalidadeId, int anoLetivo)
        {
            IEnumerable<ProfessorTitularDisciplinaEol> professoresTitularesDisciplinasEol = await servicoEOL.ObterProfessoresTitularesDisciplinas(turmaId, professorRf);
            List<AtribuicaoCJ> atribuicaoCJ = new List<AtribuicaoCJ>();

            if (anoLetivo >= 2022 && modalidadeId == Modalidade.EducacaoInfantil)
            {
                var listaDisciplinas = await consultasDisciplina.ObterComponentesCurricularesPorProfessorETurma(turmaId, false);
                var listaDisciplinaComProfessor = VerificaTitularidadeDisciplinaInfantil(professoresTitularesDisciplinasEol, listaDisciplinas);

                foreach (var disciplinas in listaDisciplinas)
                    atribuicaoCJ.AddRange(await mediator.Send(new ObterAtribuicoesPorTurmaEProfessorQuery(modalidadeId, turmaId, ueId, disciplinas.CodigoComponenteCurricular, professorRf, string.Empty, null, "", null, anoLetivo)));

                if (professoresTitularesDisciplinasEol != null && professoresTitularesDisciplinasEol.Any())
                    return TransformaEntidadesEmDtosAtribuicoesProfessoresRetorno(atribuicaoCJ, listaDisciplinaComProfessor);
                else
                    return null;
            }
            else
            {
                var listaAtribuicoes = await mediator.Send(new ObterAtribuicoesPorTurmaEProfessorQuery(modalidadeId, turmaId, ueId, 0, professorRf, string.Empty, null, "", null, anoLetivo));

                if (professoresTitularesDisciplinasEol != null && professoresTitularesDisciplinasEol.Any())
                    return TransformaEntidadesEmDtosAtribuicoesProfessoresRetorno(listaAtribuicoes, professoresTitularesDisciplinasEol);
                else return null;
            }
        }

        public List<ProfessorTitularDisciplinaEol> VerificaTitularidadeDisciplinaInfantil(IEnumerable<ProfessorTitularDisciplinaEol> professoresTitularesEol, List<DisciplinaDto> disciplinas)
        {
            var listaProfessorDisciplina = new List<ProfessorTitularDisciplinaEol>();
            foreach(var disciplina in disciplinas)
            {
                if(!professoresTitularesEol.Any(p => p.DisciplinaId == disciplina.CodigoComponenteCurricular))
                {
                    listaProfessorDisciplina.Add(new ProfessorTitularDisciplinaEol()
                    {
                        DisciplinaId = disciplina.Id,
                        DisciplinaNome = disciplina.Nome,
                        ProfessorNome = "Não há professor titular",
                        ProfessorRf = ""
                    });
                }
                else
                {
                    listaProfessorDisciplina.Add(professoresTitularesEol.FirstOrDefault(p => p.DisciplinaId == disciplina.CodigoComponenteCurricular));
                }
            }

            return listaProfessorDisciplina;
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
                    Substituir = atribuicao != null && atribuicao.Substituir
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
    }
}
