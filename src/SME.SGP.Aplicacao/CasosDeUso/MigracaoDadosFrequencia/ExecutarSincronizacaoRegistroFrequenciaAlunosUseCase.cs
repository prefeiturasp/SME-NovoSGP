using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarSincronizacaoRegistroFrequenciaAlunosUseCase : AbstractUseCase, IExecutarSincronizacaoRegistroFrequenciaAlunosUseCase
    {
        public ExecutarSincronizacaoRegistroFrequenciaAlunosUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var dadosAula = mensagemRabbit.ObterObjetoMensagem<MigracaoFrequenciaTurmaAulaDto>();
            var contadorAula = 1;
            while(contadorAula <= dadosAula.QuantidadeAula)
            {
                foreach(var codigoAluno in dadosAula.CodigosAlunos)
                {
                    var existeRegistro = await mediator.Send(new ExisteRegistroFrequenciaAlunoQuery(dadosAula.RegistroFrequenciaId, codigoAluno, contadorAula));
                    if (!existeRegistro)
                    {
                        var registro = new RegistroFrequenciaAluno()
                        {
                            CodigoAluno = codigoAluno,
                            NumeroAula = contadorAula,
                            RegistroFrequenciaId = dadosAula.RegistroFrequenciaId,
                            CriadoEm = DateTime.Today,
                            CriadoPor = "Sistema",
                            CriadoRF = "Sistema",
                            Valor = 1
                        };
                        await mediator.Send(new SalvarRegistroFrequenciaAlunoCommand(registro));
                    }
                }
                contadorAula++;
            }
            return true;
        }
    }
}
