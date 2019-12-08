import styled from 'styled-components';

import { Base } from '../../componentes/colors';

export const Container = styled.div`
`

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

  .width-150 {
    width: 150px;
    max-width: 150px;
  }

  .width-70 {
    width: 70px;
    max-width: 70px;
  }

  .width-50 {
    width: 50px;
    max-width: 50px;
  }

  .width-60 {
    width: 60px;
    max-width: 60px;
  }

  .cursor-pointer {
    cursor: pointer;
  }

  .botao-ordenacao-avaliacao  {
    float: left;
    margin-left: -12px !important;
  }

  .texto-header-avaliacao {
    text-overflow: ellipsis;
    overflow: hidden;
    white-space: nowrap;
    max-width: 140px;
  }

  .select-conceitos {
    border-radius: 4px;
    margin-bottom: -3px;
  }

  .aluno-conceitos {
    border-top: solid 2px white;
    background-color: white;
    border-radius: 7px;
    border-right: solid 22px white;
    padding-left: 2px;
    margin-left: 13px;
  }

  .aluno-ausente-conceitos {
    border-top: solid 2px #d06d12 !important;
    background-color: #d06d12 !important;
    border-radius: 7px !important;
    border-right: solid 22px #D06D12 !important;
    padding-left: 2px !important;
    margin-left: 13px !important;
  }

  .aluno-notas {
    border-top: solid 3px white !important;
    background-color: white !important;
    border-radius: 7px !important;
    border-right: solid 20px white !important;
    padding-left: 3px !important;
    margin-left: 23px !important;
  }

  .aluno-ausente-notas {
    border-top: solid 3px #d06d12 !important;
    background-color: #d06d12 !important;
    border-radius: 7px !important;
    border-right: solid 20px #D06D12 !important;
    padding-left: 3px !important;
    margin-left: 23px !important;
  }

  .icon-aluno-ausente {
    color: white;
    float: right;
    margin-top: -27px;
    font-size: 10px;
    margin-right: 5px;
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
