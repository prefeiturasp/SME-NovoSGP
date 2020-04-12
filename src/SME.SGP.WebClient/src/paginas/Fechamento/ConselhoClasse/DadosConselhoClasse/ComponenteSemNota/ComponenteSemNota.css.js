import styled from 'styled-components';
import { Base } from '~/componentes';

export const Tabela = styled.div`
  .tabela-componente-sem-nota-thead {
    background: ${Base.CinzaFundo} !important;
    text-align: center;
    th {
      border-right: solid 1px ${Base.CinzaDesabilitado};
      border-bottom: 1px solid ${Base.CinzaDesabilitado};
      vertical-align: middle;
      padding: 2px;
    }
    tr {
      border-left: solid 1px ${Base.CinzaDesabilitado};
      height: 45px;
    }
  }

  .tabela-componente-sem-nota-tbody {
    text-align: center;
    border-left: solid 1px ${Base.CinzaDesabilitado};

    tr td {
      border-right: solid 1px ${Base.CinzaDesabilitado};
      border-bottom: 1px solid ${Base.CinzaDesabilitado};
    }
  }

  .coluna-componente {
    text-align: left;
  }

  .sombra-direita {
    box-shadow: 7px 1px 15px 0px rgba(0, 0, 0, 0.095);
  }
`;

export const BarraLateralAzul = styled.td`
  background-color: ${props => props.corBorda};
  border: 1px solid ${props => props.corBorda} !important;
  width: 7px !important;
  margin: 0 !important;
  padding: 0 !important;
`;
