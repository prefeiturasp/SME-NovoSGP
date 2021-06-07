using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CarregarRegistroFrequenciaAlunosUseCase : AbstractUseCase, ICarregarRegistroFrequenciaAlunosUseCase
    {
        public CarregarRegistroFrequenciaAlunosUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var dadosAula = mensagemRabbit.ObterObjetoMensagem<MigracaoFrequenciaTurmaAulaDto>();
            var contadorAula = 1;
            while (contadorAula <= dadosAula.QuantidadeAula)
            {
                var codigosAlunosComRegistro = await mediator.Send(new CodigosAlunosComRegistroFrequenciaAlunoQuery(dadosAula.RegistroFrequenciaId, dadosAula.CodigosAlunos, contadorAula));
                foreach (var codigoAluno in dadosAula.CodigosAlunos)
                {
                    var existeRegistro = false;
                    if (codigosAlunosComRegistro != null && codigosAlunosComRegistro.Any())
                    {
                        var codigoAlunoExistente = codigosAlunosComRegistro.FirstOrDefault(c => c == codigoAluno);
                        existeRegistro = codigoAlunoExistente != null;
                    }
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
                        await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.SincronizarDadosAlunosFrequenciaMigracao, registro, Guid.NewGuid(), null));                        
                    }
                }
                contadorAula++;
            }
            return true;
        }
    }
}
