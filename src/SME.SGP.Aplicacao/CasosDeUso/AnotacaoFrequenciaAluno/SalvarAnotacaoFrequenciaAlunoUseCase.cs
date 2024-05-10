using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarAnotacaoFrequenciaAlunoUseCase : ISalvarAnotacaoFrequenciaAlunoUseCase
    {
        private readonly IMediator mediator;

        public SalvarAnotacaoFrequenciaAlunoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<AuditoriaDto> Executar(SalvarAnotacaoFrequenciaAlunoDto dto)
        {
            var aula = await mediator.Send(new ObterAulaPorIdQuery(dto.AulaId));
            if (aula.EhNulo())
                throw new NegocioException("Aula não encontrada.");

            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(aula.TurmaId));

            var verificarTipoTurma = !turma.EhTurmaPrograma();
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            if (aula.DataAula.Date <= DateTime.Today && !usuarioLogado.EhProfessorCj() && !usuarioLogado.EhGestorEscolar())
            {
                var usuarioPossuiAtribuicaoNaTurmaNaData = await mediator.Send(new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery(dto.ComponenteCurricularId, aula.TurmaId, aula.DataAula, usuarioLogado));
                if (!usuarioPossuiAtribuicaoNaTurmaNaData)
                    throw new NegocioException("Você não pode fazer alterações ou inclusões nesta turma, componente e data.");
            }

            var aluno = await mediator.Send(new ObterAlunoPorTurmaAlunoCodigoQuery(aula.TurmaId, dto.CodigoAluno, true));
            if (aluno.EhNulo())
                throw new NegocioException($"{(dto.EhInfantil ? "Criança não encontrada" : "Aluno não encontrado")}.");

            if (aluno.EstaInativo(aula.DataAula))
                throw new NegocioException($"{(dto.EhInfantil ? "Criança não ativa na turma" : "Aluno não ativo na turma")}.");
            await MoverArquivos(dto);
            return await mediator.Send(new SalvarAnotacaoFrequenciaAlunoCommand(dto));
        }
        private async Task MoverArquivos(SalvarAnotacaoFrequenciaAlunoDto anotacaoAluno)
        {
            if (!string.IsNullOrEmpty(anotacaoAluno.Anotacao))
            {
                var moverArquivo = await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.FrequenciaAnotacaoEstudante, string.Empty, anotacaoAluno.Anotacao));
                anotacaoAluno.Anotacao = moverArquivo;
            }
        }
    }
}
