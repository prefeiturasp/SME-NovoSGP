import styled from 'styled-components';
import { Base } from '~/componentes';

export const Lista = styled.div`
  .tabela-conselho-thead {
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

  .tabela-conselho-tbody {
    text-align: center;
    border-left: solid 1px ${Base.CinzaDesabilitado};

    tr td {
      border-right: solid 1px ${Base.CinzaDesabilitado};
      border-bottom: 1px solid ${Base.CinzaDesabilitado};
    }
  }
`;

export const CampoDesabilitado = styled.div`
  justify-content: center;
  align-items: center;
  display: flex;

  span {
    justify-content: center;
    align-items: center;
    display: flex;
    background-color: ${Base.Branco} !important;
    border-radius: 4px;
    border: 1px solid ${Base.CinzaDesabilitado};
    color: ${Base.CinzaDesabilitado} !important;
    width: 65px;
    height: 32px;
  }
`;

export const BarraLateral = styled.td`
  background-color: ${Base.VerdeBorda};
  border: 1px solid ${Base.VerdeBorda} !important;
`;
