using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorUeLoginPerfilQuery : IRequest<IEnumerable<AbrangenciaTurmaRetorno>>
    {
        public ObterTurmasPorUeLoginPerfilQuery(string codigoUe, string login, Guid perfil, Modalidade modalidade, int periodo, bool consideraHistorico, int anoLetivo, int[] tipos = null)
        {
            CodigoUe = codigoUe;
            Login = login;
            Perfil = perfil;
            Modalidade = modalidade;
            Periodo = periodo;
            ConsideraHistorico = consideraHistorico;
            AnoLetivo = anoLetivo;
            Tipos = tipos;
        }

        public string CodigoUe { get; }
        public string Login { get; }
        public Guid Perfil { get; }
        public Modalidade Modalidade { get; }
        public int Periodo { get; }
        public bool ConsideraHistorico { get; }
        public int AnoLetivo { get; }
        public int[] Tipos { get; }
    }
}
