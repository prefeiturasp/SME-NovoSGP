import styled from 'styled-components';
import { Base } from '~/componentes/colors';

export const Lista = styled.div`
  max-width: 100%;
  min-width: 100%;

  .desabilitar-nota {
    opacity: 0.4 !important;
    cursor: unset !important;
  }
  td {
    padding-top: 25px !important;
  }

  .tabela-fechamento-final-thead {
    background: ${Base.CinzaFundo} !important;
    text-align: center;

    th {
      border-right: solid 1px ${Base.CinzaDesabilitado};
      border-bottom: 1px solid ${Base.CinzaDesabilitado};
    }

    tr {
      border-left: solid 1px ${Base.CinzaDesabilitado};
    }

    .border-right-none {
      border-right: none !important;
    }

    .coluna-ordenacao-th {
      border-top: none;
      background-color: white;
    }

    .coluna-ordenacao-tr {
      border-left: none;
    }
  }

  .tabela-fechamento-final-tbody {
    text-align: center;
    border-left: solid 1px ${Base.CinzaDesabilitado};

    tr td {
      border-right: solid 1px ${Base.CinzaDesabilitado};
      border-bottom: 1px solid ${Base.CinzaDesabilitado};
    }

    .border-right-none {
      border-right: none !important;
    }
  }

  .col-nome-aluno {
    width: 250px;
    max-width: 250px;
    min-width: 250px;
    text-align: left;
  }

  .col-nota-conceito {
    width: 250px;
    max-width: 250px;
    min-width: 250px;
  }

  .col-numero-chamada {
    width: 54px;
    height: 49px;
    font-family: Roboto;
    font-size: 12px;
    font-weight: bold;
    font-stretch: normal;
    font-style: normal;
    line-height: normal;
    letter-spacing: normal;
    color: #42474a;
  }

  .input-notas {
    width: 43.8px;
    height: 35.6px;
    border-radius: 3px;
    border: solid 1px #ced4da;
    background-color: #f5f6f8;
    margin-right: 5px;
    margin-left: 5px;
    text-align: center;
    padding-top: 7px;
    color: #a4a4a4;
  }
  .col-notas {
    display: flex;
  }

  .width-120 {
    width: 120px;
    max-width: 120px;
    min-width: 120px;
  }
`;
