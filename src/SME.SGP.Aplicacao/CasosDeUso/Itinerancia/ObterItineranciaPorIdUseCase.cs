using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
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

            var ues = await mediator.Send(new ObterUesPorIdsQuery(itinerancia.Ues.Select(u => u.UeId).ToArray()));

            var questoesBase = await mediator.Send(new ObterQuestoesBaseItineranciaEAlunoQuery());

            var itineranciaDto = new ItineranciaDto()
            {
                DataVisita = itinerancia.DataVisita,
                DataRetornoVerificacao = itinerancia.DataRetornoVerificacao,
                ObjetivosVisita = MontarObjetivosItinerancia(itinerancia),
                Questoes = MontarQuestoesItinerancia(itinerancia, questoesBase),
                Ues = MontarUes(ues, itinerancia)
            };

            if(itinerancia.Alunos != null && itinerancia.Alunos.Any())
            {
                var CodigosAluno = itinerancia.Alunos.Select(a => a.CodigoAluno).ToArray();

                var alunosEol = await mediator.Send(new ObterAlunosEolPorCodigosEAnoQuery(CodigosAluno.Select(long.Parse).ToArray(), DateTime.Now.Year));

                var turmas = await mediator.Send(new ObterTurmasPorCodigosQuery(alunosEol.Select(al => al.CodigoTurma.ToString()).ToArray()));

                itineranciaDto.Alunos = MontarAlunosItinerancia(itinerancia, alunosEol, questoesBase, turmas);
            }

            return itineranciaDto;
        }


        private IEnumerable<ItineranciaUeDto> MontarUes(IEnumerable<Ue> ues, Itinerancia itinerancia)
        {
            return ues.Select(ue =>
             {
                 var objUe = ues.FirstOrDefault(u => u.Id == ue.Id);
                 return new ItineranciaUeDto
                 {
                     Id = itinerancia.Ues.FirstOrDefault(i => i.UeId == ue.Id).Id,                              
                     UeId = ue.Id,
                     Descricao = $"{objUe.TipoEscola.ShortName()} - {objUe.Nome}" 
                 };
             });
        }

        private IEnumerable<ItineranciaQuestaoDto> MontarQuestoesItinerancia(Itinerancia itinerancia, ItineranciaQuestoesBaseDto questoesBase)
        {
            return itinerancia.Questoes.Select(questao =>
            {
                var descricao = questoesBase.ItineranciaQuestao.FirstOrDefault(q => q.QuestaoId == questao.QuestaoId)?.Descricao;
                var obrigatorio = questoesBase.ItineranciaQuestao.FirstOrDefault(q => q.QuestaoId == questao.QuestaoId)?.Obrigatorio;
                return new ItineranciaQuestaoDto
                {
                    Id = questao.Id,
                    QuestaoId = questao.QuestaoId,
                    Descricao = descricao,
                    ItineranciaId = questao.ItineranciaId,
                    Resposta = questao.Resposta,
                    Obrigatorio = obrigatorio

                };
            });
        }

        private IEnumerable<ItineranciaObjetivoDto> MontarObjetivosItinerancia(Itinerancia itinerancia)
        {
            return itinerancia.ObjetivosVisita.Select(o =>
            {
                return new ItineranciaObjetivoDto
                {
                    Id = o.Id,
                    ItineranciaObjetivoBaseId = o.ItineranciaObjetivosBaseId,
                    Nome = itinerancia.ObjetivosBase.FirstOrDefault(ob => ob.Id == o.ItineranciaObjetivosBaseId).Nome,
                    TemDescricao = itinerancia.ObjetivosBase.FirstOrDefault(ob => ob.Id == o.ItineranciaObjetivosBaseId).TemDescricao,
                    PermiteVariasUes = itinerancia.ObjetivosBase.FirstOrDefault(ob => ob.Id == o.ItineranciaObjetivosBaseId).PermiteVariasUes,
                    Descricao = string.IsNullOrEmpty(o.Descricao) ? "" : o.Descricao
                };
            });
        }

        private IEnumerable<ItineranciaAlunoDto> MontarAlunosItinerancia(Itinerancia itinerancia, IEnumerable<TurmasDoAlunoDto> alunosEol, ItineranciaQuestoesBaseDto questoesBase, IEnumerable<Turma> turmas)
        {
            return itinerancia.Alunos.Select(aluno =>
            {
                return new ItineranciaAlunoDto
                {
                    Id = aluno.Id,
                    AlunoCodigo = aluno.CodigoAluno,                    
                    Nome = @$"{alunosEol
                                 .FirstOrDefault(a => a.CodigoAluno == int.Parse(aluno.CodigoAluno)).NomeAluno} - {OberterNomeTurmaFormatado(turmas.FirstOrDefault(t => t.CodigoTurma == alunosEol.FirstOrDefault(a => a.CodigoAluno == int.Parse(aluno.CodigoAluno)).CodigoTurma.ToString()))}",
                    Questoes = MontarQuestoesItineranciaAluno(aluno, questoesBase)
                };
            });
        }

        private IEnumerable<ItineranciaAlunoQuestaoDto> MontarQuestoesItineranciaAluno(ItineranciaAluno aluno, ItineranciaQuestoesBaseDto questoesBase)
        {
            return aluno.AlunosQuestoes.Select(questao =>
            {
                return new ItineranciaAlunoQuestaoDto
                {
                    QuestaoId = questao.QuestaoId,
                    Descricao = questoesBase.ItineranciaAlunoQuestao.FirstOrDefault(q => q.QuestaoId == questao.QuestaoId).Descricao,
                    ItineranciaAlunoId = questao.ItineranciaAlunoId,
                    Resposta = questao.Resposta,
                    Obrigatorio = questoesBase.ItineranciaAlunoQuestao.FirstOrDefault(q => q.QuestaoId == questao.QuestaoId).Obrigatorio
                };
            });
        }

        private string OberterNomeTurmaFormatado(Turma turma)
        {
            var turmaNome = "";

            if (turma != null)
                turmaNome = $"{turma.ModalidadeCodigo.ShortName()} - {turma.Nome}";

            return turmaNome;
        }

    }
}
