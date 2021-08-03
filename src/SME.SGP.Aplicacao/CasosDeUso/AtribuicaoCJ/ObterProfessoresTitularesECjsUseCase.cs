using MediatR;
using Sentry;
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
        public ObterProfessoresTitularesECjsUseCase(IMediator mediator, IServicoEol servicoEOL) : base(mediator)
        {
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<AtribuicaoCJTitularesRetornoDto> Executar(string ueId, string turmaId,
                    string professorRf, Modalidade modalidadeId, int anoLetivo)
        {
            IEnumerable<ProfessorTitularDisciplinaEol> professoresTitularesDisciplinasEol = await servicoEOL.ObterProfessoresTitularesDisciplinas(turmaId, professorRf);

            var listaAtribuicoes = await mediator.Send(new ObterAtribuicoesPorTurmaEProfessorQuery(modalidadeId, turmaId, ueId, 0, professorRf, string.Empty, null,"",null, anoLetivo));

            if (professoresTitularesDisciplinasEol != null && professoresTitularesDisciplinasEol.Any())
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
