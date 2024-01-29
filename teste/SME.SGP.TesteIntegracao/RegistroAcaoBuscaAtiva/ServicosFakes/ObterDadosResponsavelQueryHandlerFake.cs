using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.RegistroAcaoBuscaAtiva.ServicosFakes
{
    public class ObterDadosResponsavelQueryHandlerFake : IRequestHandler<ObterDadosResponsavelQuery, IEnumerable<DadosResponsavelAlunoEolDto>>
    {
        private const string ALUNO_1 = "1";
        private const string ALUNO_2 = "2";

        public Task<IEnumerable<DadosResponsavelAlunoEolDto>> Handle(ObterDadosResponsavelQuery request, CancellationToken cancellationToken)
        {
            var dadosResponsaveis = new List<DadosResponsavelAlunoEolDto>()
            {
                new DadosResponsavelAlunoEolDto()
                {
                    CodigoAluno = ALUNO_1,
                    Cpf = "1",
                    DDDCelular = "62",
                    DDDTelefoneComercial = "62",
                    DDDTelefoneFixo = "62",
                    NumeroCelular = "9999-9999",
                    NumeroTelefoneComercial = "3232-3232",
                    NumeroTelefoneFixo = "3333-3333",
                    Email = "teste@teste.com",
                },
                new DadosResponsavelAlunoEolDto()
                {
                    CodigoAluno = ALUNO_2,
                    Cpf = "2",
                    DDDCelular = "62",
                    DDDTelefoneComercial = "62",
                    DDDTelefoneFixo = "62",
                    NumeroCelular = "9999-9999",
                    NumeroTelefoneComercial = "3232-3232",
                    NumeroTelefoneFixo = "3333-3333",
                    Email = "teste@teste.com",
                },
            };

            return Task.FromResult<IEnumerable<DadosResponsavelAlunoEolDto>>(dadosResponsaveis);
        }
    }
}
