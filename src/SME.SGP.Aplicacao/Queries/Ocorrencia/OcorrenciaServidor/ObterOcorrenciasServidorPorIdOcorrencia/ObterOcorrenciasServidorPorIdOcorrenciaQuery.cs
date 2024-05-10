using System.Collections.Generic;
using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterOcorrenciasServidorPorIdOcorrenciaQuery : IRequest<IEnumerable<Dominio.OcorrenciaServidor>>
    {
        public  long IdOcorrencia { get; set; }

        public ObterOcorrenciasServidorPorIdOcorrenciaQuery(long idOcorrencia)
        {
            IdOcorrencia = idOcorrencia;
        }
        
        
        public class ObterOcorrenciasServidorPorIdOcorrenciaQueryValidator : AbstractValidator<ObterOcorrenciasServidorPorIdOcorrenciaQuery>
        {
            public ObterOcorrenciasServidorPorIdOcorrenciaQueryValidator()
            {
                RuleFor(x => x.IdOcorrencia).GreaterThan(0).WithMessage("Informe o código da ocorrência para realizar a consulta");
            }
        }
    }
}