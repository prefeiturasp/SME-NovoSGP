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

  .input-notas-conceitos {
    justify-content: center;
    align-items: center;
    display: flex;
    span {
      justify-content: center;
      align-items: center;
      display: flex;
      background-color: #f5f6f8;
      border-radius: 4px;
      border: 1px solid #ced4da;
      color: ${Base.CinzaDesabilitado} !important;
      width: 43.8px;
      height: 35.6px;
    }
  }

  .input-notas-conceitos-final {
    width: 43.8px;
    height: 35.6px;
    border-radius: 3px;
    border: solid 1px #ced4da;
    background-color: #f5f6f8;
    margin-right: 5px;
    margin-left: 5px;
    text-align: center;
    padding-top: 9px;
    margin-bottom: 9px;
    color: #a4a4a4;
  }

  .col-nota-conceito {
    width: 250px;
    max-width: 250px;
    min-width: 250px;
  }

  .borda-abaixo-media {
    border: solid 2px #b22222 !important;
    border-radius: 7px;
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

export const BarraLateralLista = styled.td`
  background-color: ${props => props.cor || ''};
  border: 1px solid ${props => props.cor || ''}!important;
  width: 7px !important;
  margin: 0 !important;
  padding: 0 !important;
`;
