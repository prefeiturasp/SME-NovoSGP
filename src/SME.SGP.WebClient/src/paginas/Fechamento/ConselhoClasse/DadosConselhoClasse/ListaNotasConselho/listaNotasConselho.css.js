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
      vertical-align: middle;
    }
  }
  .coluna-disciplina {
    text-align: left;
  }

  .sombra-direita {
    box-shadow: 7px 1px 15px 0px rgba(0, 0, 0, 0.095);
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
    width: 88px;
    height: 38px;
  }
`;

export const BarraLateralVerde = styled.td`
  background-color: ${Base.VerdeBorda};
  border: 1px solid ${Base.VerdeBorda} !important;
  width: 7px !important;
  margin: 0 !important;
  padding: 0 !important;
`;

export const BarraLateralBordo = styled.td`
  background-color: ${Base.Bordo};
  border: 1px solid ${Base.Bordo} !important;
  width: 7px !important;
  margin: 0 !important;
  padding: 0 !important;
`;
