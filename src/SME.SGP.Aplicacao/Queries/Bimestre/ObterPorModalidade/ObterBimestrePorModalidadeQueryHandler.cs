using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Bimestre.ObterPorModalidade
{
    public class ObterBimestrePorModalidadeQueryHandler : IRequestHandler<ObterBimestrePorModalidadeQuery, List<FiltroBimestreDto>>
    {

            public ObterBimestrePorModalidadeQueryHandler()
            {

            }
            public async Task<List<FiltroBimestreDto>> Handle(ObterBimestrePorModalidadeQuery request, CancellationToken cancellationToken)
            {
                var listaBimestres = new List<FiltroBimestreDto>();

                if (request.OpcaoTodos)
                {
                    var bimestre = new FiltroBimestreDto();
                    bimestre.Valor = (int)SME.SGP.Dominio.Bimestre.Todos;
                    bimestre.Descricao = SME.SGP.Dominio.Bimestre.Todos.ObterNome();
                    listaBimestres.Add(bimestre);
                }

                var primeiro = new FiltroBimestreDto()
                {
                    Valor = (int)SME.SGP.Dominio.Bimestre.Primeiro,
                    Descricao = SME.SGP.Dominio.Bimestre.Primeiro.ObterNome()
                };

                var segundoBimestre = new FiltroBimestreDto()
                {
                    Valor = (int)SME.SGP.Dominio.Bimestre.Segundo,
                    Descricao = SME.SGP.Dominio.Bimestre.Segundo.ObterNome()
                };

                listaBimestres.Add(primeiro);
                listaBimestres.Add(segundoBimestre);

                if (request.Modalidade != Modalidade.EJA)
                {
                    var terceiroBimestre = new FiltroBimestreDto()
                    {
                        Valor = (int)SME.SGP.Dominio.Bimestre.Terceiro,
                        Descricao = SME.SGP.Dominio.Bimestre.Terceiro.ObterNome()
                    };

                    var quartoBimestre = new FiltroBimestreDto()
                    {
                        Valor = (int)SME.SGP.Dominio.Bimestre.Quarto,
                        Descricao = SME.SGP.Dominio.Bimestre.Quarto.ObterNome()
                    };

                    listaBimestres.Add(terceiroBimestre);
                    listaBimestres.Add(quartoBimestre);
                }

                if (request.OpcaoFinal)
                {
                    var bimestreFinal = new FiltroBimestreDto()
                    {
                        Valor = (int)SME.SGP.Dominio.Bimestre.Final,
                        Descricao = SME.SGP.Dominio.Bimestre.Final.ObterNome()
                    };

                    listaBimestres.Add(bimestreFinal);
                }

                return listaBimestres;

            }
        }
    }

