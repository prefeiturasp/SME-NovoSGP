﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarAnotacaoFrequenciaAlunoUseCase : AbstractUseCase, IAlterarAnotacaoFrequenciaAlunoUseCase
    {
        public AlterarAnotacaoFrequenciaAlunoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(AlterarAnotacaoFrequenciaAlunoDto param)
        {
            var anotacao = await ObterAnotacao(param.Id);

            var aula = await mediator.Send(new ObterAulaPorIdQuery(anotacao.AulaId));
            await ValidarAtribuicaoUsuario(long.Parse(aula.DisciplinaId), aula.TurmaId, aula.DataAula);

            return await AtualizarAnotacaoFrequenciaAluno(anotacao, param);
        }

        private async Task<bool> AtualizarAnotacaoFrequenciaAluno(AnotacaoFrequenciaAluno anotacao, AlterarAnotacaoFrequenciaAlunoDto param)
        {
            anotacao.MotivoAusenciaId = param.MotivoAusenciaId;
            anotacao.Anotacao = param.Anotacao;

            return await mediator.Send(new AlterarAnotacaoFrequenciaAlunoCommand(anotacao));
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
