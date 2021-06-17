using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
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
            var frequenciasAula = await mediator.Send(new ObterRegistrosFrequenciasAlunosSimplificadoPorAulaIdQuery(dadosAula.AulaId));

            for (var numeroAula = 1; numeroAula <= dadosAula.QuantidadeAula; numeroAula++)
            {
                foreach (var codigoAluno in dadosAula.CodigosAlunos)
                {
                    if (!ExisteRegistroFrequenciaAluno(frequenciasAula, codigoAluno, numeroAula))
                    {
                        var registro = new RegistroFrequenciaAluno()
                        {
                            CodigoAluno = codigoAluno,
                            NumeroAula = numeroAula,
                            RegistroFrequenciaId = dadosAula.RegistroFrequenciaId,
                            CriadoEm = DateTime.Today,
                            CriadoPor = "Sistema",
                            CriadoRF = "Sistema",
                            Valor = (int)TipoFrequencia.C
                        };
                        await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.SincronizarDadosAlunosFrequenciaMigracao, registro, Guid.NewGuid(), null));                        
                    }
                }
            }
            return true;
        }

        private bool ExisteRegistroFrequenciaAluno(IEnumerable<FrequenciaAlunoSimplificadoDto> frequenciasAula, string codigoAluno, int numeroAula)
            => frequenciasAula != null &&
               frequenciasAula.Any(a => a.CodigoAluno == codigoAluno &&
                                        a.NumeroAula == numeroAula);
    }
}
