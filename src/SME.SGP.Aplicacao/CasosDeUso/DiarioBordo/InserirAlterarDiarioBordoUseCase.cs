using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirAlterarDiarioBordoUseCase : AbstractUseCase, IInserirAlterarDiarioBordoUseCase
    {
        public InserirAlterarDiarioBordoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<AuditoriaDiarioBordoDto>> Executar(IEnumerable<InserirAlterarDiarioBordoDto> dados)
        {
            var auditoria = new List<AuditoriaDiarioBordoDto>();
            AuditoriaDto dadosAuditoria = new AuditoriaDto();

            foreach (var param in dados)
            {
                if(param.Id == 0)    
                {
                    dadosAuditoria = await mediator.Send(new InserirDiarioBordoCommand(param.AulaId, param.Planejamento, param.ReflexoesReplanejamento, param.ComponenteCurricularId));
                    await mediator.Send(new ExcluirPendenciaAulaCommand(param.AulaId, Dominio.TipoPendencia.DiarioBordo));
                }
                else
                {
                    dadosAuditoria = await mediator.Send(new AlterarDiarioBordoCommand(param.Id, param.AulaId, param.Planejamento, param.ReflexoesReplanejamento, param.ComponenteCurricularId));       
                }

                var valorAuditoria = new AuditoriaDiarioBordoDto(dadosAuditoria, param.AulaId);
                auditoria.Add(valorAuditoria);
            }

            return auditoria;
        }
    }
}
