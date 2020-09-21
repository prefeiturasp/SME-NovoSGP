using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirAnotacaoFrequenciaAlunoUseCase : AbstractUseCase, IExcluirAnotacaoFrequenciaAlunoUseCase
    {
        public ExcluirAnotacaoFrequenciaAlunoUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(long anotacaoId)
        {
            var anotacao = await ObterAnotacao(anotacaoId);

            var aula = await mediator.Send(new ObterAulaPorIdQuery(anotacao.AulaId));
            await ValidarAtribuicaoUsuario(long.Parse(aula.DisciplinaId), aula.TurmaId, aula.DataAula);

            await mediator.Send(new ExcluirAnotacaoFrequenciaAlunoCommand(anotacao));;
            return true;
        }

        private async Task ValidarAtribuicaoUsuario(long componenteCurricularId, string turmaId, DateTime dataAula)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            var usuarioPossuiAtribuicaoNaTurmaNaData = await mediator.Send(new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery(componenteCurricularId, turmaId, dataAula, usuarioLogado));
            if (!usuarioPossuiAtribuicaoNaTurmaNaData)
                throw new NegocioException("Você não pode fazer alterações ou inclusões nesta turma, componente e data.");
        }

        private async Task<AnotacaoFrequenciaAluno> ObterAnotacao(long id)
        {
            var anotacao = await mediator.Send(new ObterAnotacaoFrequenciaAlunoPorIdQuery(id));
            if (anotacao == null)
                throw new NegocioException("Anotação não localizada com o Id informado");

            return anotacao;
        }
    }
}
