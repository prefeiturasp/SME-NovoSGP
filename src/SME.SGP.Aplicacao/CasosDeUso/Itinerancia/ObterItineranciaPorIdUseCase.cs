using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterItineranciaPorIdUseCase : AbstractUseCase, IObterItineranciaPorIdUseCase
    {
        public ObterItineranciaPorIdUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ItineranciaDto> Executar(long id)
        {            
            var itinerancia = await mediator.Send(new ObterItineranciaPorIdQuery(id));            
            

            if (itinerancia == null)
                throw new NegocioException($"Não foi possível localizar a itinerância de Id {id}");            

            var CodigosAluno = itinerancia.Alunos.Select(a => a.CodigoAluno).ToArray();           

            var alunosEol = await mediator.Send(new ObterAlunosEolPorCodigosEAnoQuery(CodigosAluno.Select(long.Parse).ToArray() , DateTime.Now.Year));

            var turmas = await mediator.Send(new ObterTurmasPorCodigosQuery(alunosEol.Select(al => al.CodigoTurma.ToString()).ToArray()));

            var ues = await mediator.Send(new ObterUesPorIdsQuery(itinerancia.Ues.Select(u => u.UeId).ToArray()));

            var questoesBase = await mediator.Send(new ObterQuestoesBaseItineranciaEAlunoQuery());

            var itineranciaDto = new ItineranciaDto()
            {
                DataVisita = itinerancia.DataVisita,
                DataRetornoVerificacao = itinerancia.DataRetornoVerificacao,
                Alunos = itinerancia.Alunos.Select(aluno => 
                {
                    return new ItineranciaAlunoDto
                    {
                        Id = aluno.Id,
                        CodigoAluno = aluno.CodigoAluno,
                        Nome = @$"{alunosEol
                                       .Where(a => a.CodigoAluno == int.Parse(aluno.CodigoAluno))
                                       .Select(a => a.NomeAluno)
                                       .FirstOrDefault()}{OberterNomeTurmaFormatado(turmas
                                                                                      .Where(t => t.CodigoTurma == alunosEol
                                                                                                                    .Where(a => a.CodigoAluno == int.Parse(aluno.CodigoAluno))
                                                                                                                    .Select(a => a.CodigoTurma.ToString())
                                                                                                                    .FirstOrDefault())
                                                                 .FirstOrDefault())}",
                        Questoes = aluno.AlunosQuestoes.Select(questao => 
                        {
                            return new ItineranciaAlunoQuestaoDto
                            {
                                Id = questao.Id,
                                QuestaoId = questao.QuestaoId,
                                Descricao = questoesBase.ItineranciaAlunoQuestao
                                                .Where(q => q.Id == questao.QuestaoId)
                                                .Select(q => q.Descricao)
                                                .FirstOrDefault(),
                                ItineranciaAlunoId = questao.ItineranciaAlunoId,
                                Resposta = questao.Resposta,
                                Obrigatorio = questoesBase.ItineranciaAlunoQuestao
                                                .Where(q => q.Id == questao.QuestaoId)
                                                .Select(q => q.Obrigatorio)
                                                .FirstOrDefault(),
                            };
                        })

                    };
                }),

                ObjetivosVisita = itinerancia.ObjetivosVisita.Select(o => 
                {
                    return new ItineranciaObjetivoDto
                    {
                        Id = o.Id,
                        ItineranciaObjetivoId = 0,
                        Nome = itinerancia.ObjetivosBase
                                  .Where(ob => ob.Id == o.ItineranciaObjetivosBaseId)
                                  .Select(ob => ob.Nome)
                                  .FirstOrDefault(),
                        TemDescricao = itinerancia.ObjetivosBase
                                         .Where(ob => ob.Id == o.ItineranciaObjetivosBaseId)
                                         .Select(ob => ob.TemDescricao)
                                         .FirstOrDefault(),
                        PermiteVariasUes = itinerancia.ObjetivosBase
                                              .Where(ob => ob.Id == o.ItineranciaObjetivosBaseId)
                                              .Select(ob => ob.PermiteVariasUes)
                                              .FirstOrDefault(),
                        Descricao = string.IsNullOrEmpty(o.Descricao) ? "" : o.Descricao
                    };
                }),

                Questoes = itinerancia.Questoes.Select(questao => 
                {
                    return new ItineranciaQuestaoDto
                    {
                        Id = questao.Id,
                        QuestaoId = questao.QuestaoId,
                        Descricao = questoesBase.ItineranciaQuestao
                                        .Where(q => q.Id == questao.QuestaoId)
                                        .Select(q => q.Descricao)
                                        .FirstOrDefault(),
                        ItineranciaId = questao.ItineranciaId,
                        Resposta = questao.Resposta,
                        Obrigatorio = questoesBase.ItineranciaQuestao
                                        .Where(q => q.Id == questao.QuestaoId)
                                        .Select(q => q.Obrigatorio)
                                        .FirstOrDefault(),
                    };
                }),

                Ues = ues.Select(ue =>
                {
                    return new ItineranciaUeDto
                    {
                        Id = itinerancia.Ues
                                .Where(i => i.UeId == ue.Id)
                                .Select(i => i.Id)
                                .FirstOrDefault(),
                        UeId = ue.Id,
                        Descricao = ues
                                     .Where(u => u.Id == ue.Id)
                                     .Select(u => u.Nome)
                                     .FirstOrDefault()
                    };
                })         
            };

            return itineranciaDto;
        }

         private string OberterNomeTurmaFormatado(Turma turma)
        {
            var turmaNome = "";

            if (turma != null)
                turmaNome = $" - {turma.ModalidadeCodigo.ShortName()} - {turma.Nome}";

            return turmaNome;
        }       

    }
}
