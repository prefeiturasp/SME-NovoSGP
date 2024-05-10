using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosPendenciaFechamento : IComandosPendenciaFechamento
    {
        private readonly IServicoPendenciaFechamento servicoPendenciaFechamento;

        public ComandosPendenciaFechamento(IServicoPendenciaFechamento servicoPendenciaFechamento)
        {
            this.servicoPendenciaFechamento = servicoPendenciaFechamento ?? throw new ArgumentNullException(nameof(servicoPendenciaFechamento));
        }

        public async Task<IEnumerable<AuditoriaPersistenciaDto>> Aprovar(IEnumerable<long> pendenciasIds)
        {
            var auditoriasDtos = new List<AuditoriaPersistenciaDto>();

            foreach (var pendenciaId in pendenciasIds)
            {
                try
                {
                    auditoriasDtos.Add(await servicoPendenciaFechamento.Aprovar(pendenciaId));
                }
                catch (Exception e)
                {
                    auditoriasDtos.Add(new AuditoriaPersistenciaDto()
                    {
                        Sucesso = false,
                        MensagemConsistencia = $"Erro ao aprovar pendencia [{pendenciaId}]: {e.Message}"
                    });
                }
            }

            return auditoriasDtos;
        }
    }
}
