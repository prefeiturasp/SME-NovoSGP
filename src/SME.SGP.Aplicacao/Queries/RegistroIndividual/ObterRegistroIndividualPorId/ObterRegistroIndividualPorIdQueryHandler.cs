using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistroIndividualPorIdQueryHandler : IRequestHandler<ObterRegistroIndividualPorIdQuery, RegistroIndividualDto>
    {
        private readonly IRepositorioRegistroIndividual repositorioRegistroIndividual;

        public ObterRegistroIndividualPorIdQueryHandler(IRepositorioRegistroIndividual repositorioRegistroIndividual)
        {
            this.repositorioRegistroIndividual = repositorioRegistroIndividual ?? throw new System.ArgumentNullException(nameof(repositorioRegistroIndividual));
        }

        public async Task<RegistroIndividualDto> Handle(ObterRegistroIndividualPorIdQuery request, CancellationToken cancellationToken)
        {
            var registroIndividual = await repositorioRegistroIndividual.ObterPorIdAsync(request.Id);

            if (registroIndividual == null)
                throw new NegocioException("Registro não foi encontrado!");

            return MapearParaDto(registroIndividual);
        }

        private RegistroIndividualDto MapearParaDto(RegistroIndividual registro)
        {
            return new RegistroIndividualDto()
            {
                Id = registro.Id,
                AlunoCodigo = registro.AlunoCodigo,
                Auditoria = (AuditoriaDto)registro,
                ComponenteCurricularId = registro.ComponenteCurricularId,
                Data = registro.DataRegistro,
                Excluido = registro.Excluido,
                Migrado = registro.Migrado,
                Registro = registro.Registro,
                TurmaId = registro.TurmaId
            };
        }
    }
}
