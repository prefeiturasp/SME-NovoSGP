using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
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
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

            if (!usuario.EhProfessorCj() && !usuario.EhGestorEscolar())
                await ValidarAtribuicaoUsuario(long.Parse(aula.DisciplinaId), aula.TurmaId, aula.DataAula, usuario);

            await mediator.Send(new ExcluirAnotacaoFrequenciaAlunoCommand(anotacao));
            if (anotacao?.Anotacao != null)
            {
                await mediator.Send(new DeletarArquivoDeRegistroExcluidoCommand(anotacao.Anotacao, TipoArquivo.FrequenciaAnotacaoEstudante.Name()));
            }
            return true;
        }

        private async Task ValidarAtribuicaoUsuario(long componenteCurricularId, string turmaId, DateTime dataAula, Usuario usuarioLogado)
        {
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
