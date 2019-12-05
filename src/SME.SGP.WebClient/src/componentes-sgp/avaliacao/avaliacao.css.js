import styled from 'styled-components';

import { Base } from '../../componentes/colors';

export const Lista = styled.div`
  width: 100%;

  .tabela-avaliacao-thead {
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

  .tabela-avaliacao-tbody {
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

  .width-170 {
    width: 170px;
  }

  .width-70 {
    width: 70px;
  }

  .width-50 {
    width: 50px;
  }

  .width-60 {
    width: 60px;
  }

  .cursor-pointer {
    cursor: pointer;
  }

  .botao-ordenacao-avaliacao  {
    float: left;
    margin-left: -12px;
  }

  .scroll-tabela-avaliacao-thead {
    overflow-y: scroll;
    ::-webkit-scrollbar {
      width: 9px !important;
      background-color: rgba(229, 237, 244, 0.71) !important;
    }
  }

  .scroll-tabela-avaliacao-tbody {
    max-height: 500px;
    overflow-y: scroll;
    border-bottom: solid 1px ${Base.CinzaDesabilitado};

    ::-webkit-scrollbar-track {
      background-color: #f4f4f4 !important;
    }

    ::-webkit-scrollbar {
      width: 9px !important;
      background-color: rgba(229, 237, 244, 0.71) !important;
      border-radius: 2.5px !important;
    }

    ::-webkit-scrollbar-thumb {
      background: #a8a8a8 !important;
      border-radius: 3px !important;
    }
  }
`;
