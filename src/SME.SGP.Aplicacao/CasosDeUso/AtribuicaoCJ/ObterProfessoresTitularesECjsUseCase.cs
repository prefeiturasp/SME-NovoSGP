﻿using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
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
            var dataReferencia = anoLetivo == DateTimeExtension.HorarioBrasilia().Year
                ? DateTimeExtension.HorarioBrasilia().Date
                : new DateTime(anoLetivo, 12, 31, 0, 0, 0, DateTimeKind.Utc);

            var professoresTitularesDisciplinasEol = (await mediator.Send(new ObterProfessoresTitularesDisciplinasEolQuery(turmaId, dataReferencia, professorRf, false))).ToList();

            var listaAtribuicoes = (await mediator.Send(new ObterAtribuicoesPorTurmaEProfessorQuery(modalidadeId, turmaId, ueId, 0, professorRf, string.Empty, null, "", null, anoLetivo))).ToList();

            if (professoresTitularesDisciplinasEol.Any())
                return TransformaEntidadesEmDtosAtribuicoesProfessoresRetorno(listaAtribuicoes, professoresTitularesDisciplinasEol);
            else return null;
        }

        public List<ProfessorTitularDisciplinaEol> VerificaTitularidadeDisciplinaInfantil(IEnumerable<ProfessorTitularDisciplinaEol> professoresTitularesEol, List<DisciplinaDto> disciplinas)
        {
            var listaProfessorDisciplina = new List<ProfessorTitularDisciplinaEol>();
            foreach (var disciplina in disciplinas)
            {
                if (professoresTitularesEol.Any(p => p.DisciplinasId.Contains(disciplina.CodigoComponenteCurricular)))
                {
                    var dadosProfessorTitular = professoresTitularesEol.FirstOrDefault(p => p.DisciplinasId.Contains(disciplina.CodigoComponenteCurricular));
                    listaProfessorDisciplina.Add(new ProfessorTitularDisciplinaEol()
                    {
                        DisciplinasId = new long[] { disciplina.Id },
                        DisciplinaNome = disciplina.NomeComponenteInfantil,
                        ProfessorNome = dadosProfessorTitular.ProfessorNome,
                        ProfessorRf = dadosProfessorTitular.ProfessorRf
                    });
                }
                else
                {
                    listaProfessorDisciplina.Add(new ProfessorTitularDisciplinaEol()
                    {
                        DisciplinasId = new long[] { disciplina.Id },
                        DisciplinaNome = disciplina.NomeComponenteInfantil,
                        ProfessorNome = "Não há professor titular",
                        ProfessorRf = ""
                    });
                }
            }

            return listaProfessorDisciplina;
        }

        private AtribuicaoCJTitularesRetornoDto TransformaEntidadesEmDtosAtribuicoesProfessoresRetorno(List<AtribuicaoCJ> listaAtribuicoes, IEnumerable<ProfessorTitularDisciplinaEol> professoresTitularesDisciplinasEol)
        {
            var listaRetorno = new AtribuicaoCJTitularesRetornoDto();

            foreach (var disciplinaProfessorTitular in professoresTitularesDisciplinasEol)
            {
                var atribuicao = listaAtribuicoes.FirstOrDefault(b => disciplinaProfessorTitular.DisciplinasId.Contains(b.DisciplinaId));

                listaRetorno.Itens.Add(new AtribuicaoCJTitularesRetornoItemDto()
                {
                    Disciplina = disciplinaProfessorTitular.DisciplinaNome,
                    DisciplinaId = disciplinaProfessorTitular.DisciplinasId.First(),
                    ProfessorTitular = disciplinaProfessorTitular.ProfessorNome,
                    ProfessorTitularRf = disciplinaProfessorTitular.ProfessorRf,
                    Substituir = atribuicao is {Substituir: true}
                });
            }

            if (listaAtribuicoes.Any())
            {
                var ultimoRegistroAlterado = listaAtribuicoes
                    .OrderBy(b => b.AlteradoEm)
                    .ThenBy(b => b.CriadoEm).FirstOrDefault();

                if (ultimoRegistroAlterado != null)
                {
                    listaRetorno.CriadoEm = ultimoRegistroAlterado.CriadoEm;
                    listaRetorno.CriadoPor = ultimoRegistroAlterado.CriadoPor;
                    listaRetorno.AlteradoEm = ultimoRegistroAlterado.AlteradoEm;
                    listaRetorno.AlteradoPor = ultimoRegistroAlterado.AlteradoPor;
                }
            }

            return listaRetorno;
        }
    }
}
