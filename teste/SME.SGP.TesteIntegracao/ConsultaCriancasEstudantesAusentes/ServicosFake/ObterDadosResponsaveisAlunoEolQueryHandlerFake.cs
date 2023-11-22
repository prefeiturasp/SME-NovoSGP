using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra.Dtos.Aluno;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ConsultaCriancasEstudantesAusentes.ServicosFake
{
    public class ObterDadosResponsaveisAlunoEolQueryHandlerFake : IRequestHandler<ObterDadosResponsaveisAlunoEolQuery, IEnumerable<DadosResponsavelAlunoDto>>
    {
        private const string CODIGO_ALUNO_1 = "1";
        private const string CODIGO_ALUNO_2 = "2";
        public async Task<IEnumerable<DadosResponsavelAlunoDto>> Handle(ObterDadosResponsaveisAlunoEolQuery request, CancellationToken cancellationToken)
        {
            var responsaveis = new List<DadosResponsavelAlunoDto>()
            {
                new DadosResponsavelAlunoDto()
                {
                    CodigoAluno = CODIGO_ALUNO_1,
                    DDDCelular = "62",
                    NumeroCelular = "99999999",
                    DDDResidencial = "62",
                    NumeroResidencial = "32323232",
                    TipoResponsavel = Dominio.TipoResponsavel.Filiacao1,
                    NomeResponsavel = "Nome responsável 1",
                    Endereco = new Dto.EnderecoRespostaDto()
                    {
                        Bairro = "Bairro",
                        CEP = 74000000,
                        Complemento = "Complemento",
                        Logradouro = "Logradouro",
                        NomeMunicipio = "NomeMunicipio",
                        Nro = "0",
                        SiglaUF = "UF",
                        Tipologradouro = "Rua"
                    }
                },
                new DadosResponsavelAlunoDto()
                {
                    CodigoAluno = CODIGO_ALUNO_2,
                    DDDCelular = "62",
                    NumeroCelular = "99999999",
                    DDDResidencial = "62",
                    NumeroResidencial = "32323232",
                    TipoResponsavel = Dominio.TipoResponsavel.Filiacao1,
                    NomeResponsavel = "Nome responsável 1",
                    Endereco = new Dto.EnderecoRespostaDto()
                    {
                        Bairro = "Bairro",
                        CEP = 74000000,
                        Complemento = "Complemento",
                        Logradouro = "Logradouro",
                        NomeMunicipio = "NomeMunicipio",
                        Nro = "0",
                        SiglaUF = "UF",
                        Tipologradouro = "Rua"
                    }
                },
                new DadosResponsavelAlunoDto()
                {
                    CodigoAluno = CODIGO_ALUNO_2,
                    DDDCelular = "62",
                    NumeroCelular = "99999999",
                    DDDResidencial = "62",
                    NumeroResidencial = "32323232",
                    TipoResponsavel = Dominio.TipoResponsavel.Filiacao2,
                    NomeResponsavel = "Nome responsável 2"
                }
            };

            return responsaveis.FindAll(responsavel => responsavel.CodigoAluno == request.CodigoAluno);
        }
    }
}
