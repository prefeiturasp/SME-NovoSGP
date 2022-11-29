using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterEstudanteLocalAtividadeUseCase : AbstractUseCase, IObterEstudanteLocalAtividadeUseCase
    {
        public ObterEstudanteLocalAtividadeUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<AlunoLocalAtividadeDto>> Executar(string codigoAluno, int? anoLetivo,bool filtrarSituacaoMatricula, bool tipoTurma)
        {
            var estudantes = await mediator.Send(new ObterTurmasAlunoPorFiltroQuery(codigoAluno, anoLetivo, filtrarSituacaoMatricula, tipoTurma));

            var alunosLocaisAtividades = new List<AlunoLocalAtividadeDto>();

            var ue = default(Ue);
            var turma = default(Turma);
            
            foreach (var estudante in estudantes)
            {
                if (!estudante.CodigoEscola.Equals(ue.CodigoUe))
                    ue = await mediator.Send(new ObterUeComDrePorCodigoQuery(estudante.CodigoEscola));
                
                if (!estudante.CodigoTurma.ToString().Equals(turma.CodigoTurma))
                    turma = await mediator.Send(new ObterTurmaPorCodigoQuery(estudante.CodigoTurma.ToString()));
                
                var local = ue.TipoEscola != TipoEscola.Nenhum ? $"{ue.TipoEscola.ObterNomeCurto()} {ue.Nome}" : $"{ue.Nome}";
                
                alunosLocaisAtividades.Add(new AlunoLocalAtividadeDto()
                {
                    Local = $"{ue.Dre.Abreviacao} {local}",
                    Atividade = $"{turma.ModalidadeCodigo.ShortName()} - {turma.Nome}"
                });
            }

            return alunosLocaisAtividades;
        }
    }
}
