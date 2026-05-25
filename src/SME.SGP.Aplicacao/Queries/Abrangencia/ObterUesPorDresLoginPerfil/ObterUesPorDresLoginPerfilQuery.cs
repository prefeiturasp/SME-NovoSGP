using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterUesPorDresLoginPerfilQuery : IRequest<IEnumerable<AbrangenciaUeComDreRetorno>>
    {
        public ObterUesPorDresLoginPerfilQuery(string[] codigosDres, string login, Guid perfil, Modalidade modalidade, int periodo, bool consideraHistorico, int anoLetivo)
        {
            CodigosDres = codigosDres;
            Login = login;
            Perfil = perfil;
            Modalidade = modalidade;
            Periodo = periodo;
            ConsideraHistorico = consideraHistorico;
            AnoLetivo = anoLetivo;
        }

        public string[] CodigosDres { get; }
        public string Login { get; }
        public Guid Perfil { get; }
        public Modalidade Modalidade { get; }
        public int Periodo { get; }
        public bool ConsideraHistorico { get; }
        public int AnoLetivo { get; }
    }
}
