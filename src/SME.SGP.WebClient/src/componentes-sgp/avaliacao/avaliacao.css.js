import styled from 'styled-components';

import { Base } from '../../componentes/colors';

export const Container = styled.div``;

export const Lista = styled.div`
  max-width: 100%;
  min-width: 100%;

  .desabilitar-nota {
    opacity: 0.4 !important;
    cursor: unset !important;
  }

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
    min-width: 150px;
  }

  .width-70 {
    width: 70px;
    max-width: 70px;
    min-width: 70px;
  }

  .width-50 {
    width: 50px;
    max-width: 50px;
    min-width: 50px;
  }

  .width-60 {
    width: 60px;
    max-width: 60px;
    min-width: 60px;
  }

  .width-460 {
    /* width: 460px;
    max-width: 460px; */
    min-width: 460px;
    width: 100%;
  }

  .width-400 {
    /* width: 400px;
    max-width: 400px; */
    min-width: 400px;
    width: 100%;
  }

  .cursor-pointer {
    cursor: pointer;
  }

  .botao-ordenacao-avaliacao {
    float: left;
    margin-left: -12px !important;
  }

  .texto-header-avaliacao {
    text-overflow: ellipsis;
    overflow: hidden;
    white-space: nowrap;
    max-width: 140px;
    width: 140px;
  }

  .select-conceitos {
    border-radius: 4px;
    margin-bottom: -3px;
  }

  .aluno-conceitos {
    border-radius: 7px;
    padding-left: 2px;
    margin-left: 13px;
    padding-right: 15px;
  }

  .aluno-ausente-conceitos {
    border-top: solid 2px #d06d12 !important;
    background-color: #d06d12 !important;
    border-radius: 7px !important;
    border-right: solid 22px #d06d12 !important;
    padding-left: 2px !important;
    margin-left: 13px !important;
  }

  .aluno-notas {
    border-radius: 7px !important;
    padding-left: 3px !important;
  }

  .aluno-ausente-notas {
    border-top: solid 3px #d06d12 !important;
    background-color: #d06d12 !important;
    border-radius: 7px !important;
    border-right: solid 28px #d06d12 !important;
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
    overflow-x: hidden;
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

  .linha-expandida {
    color: ${Base.Roxo};
    background: ${Base.CinzaFundo};
    text-align: left;
    i {
      transform: rotate(-90deg);
    }
  }

  .fa-minus-linha-expandida {
    border: 1.6px solid #6933ff !important;
    border-radius: 20px !important;
    display: inline;
    font-size: 13px;
  }

  .ant-input-number-handler-wrap {
    display: none !important;
  }

  .border-registro-alterado {
    border: solid 2px ${Base.Roxo} !important;
  }
`;

export const CaixaMarcadores = styled.span`
  border: 1.6px solid ${Base.Roxo};
  border-radius: 9px;
  padding-left: 10px;
  padding-right: 10px;
  margin-left: 8px;
  font-weight: bold;
  color: ${Base.Roxo};
`;

export const IconePlusMarcadores = styled.i`
  color: ${Base.Roxo};
  font-size: 16px;
  margin-left: 5px;
  cursor: pointer;
`;

export const CabecalhoNotaConceitoFinal = styled.th`
  box-shadow: -8px -3px 8px -4px #8080804d;
  border-bottom: 0 !important;
`;

export const LinhaNotaConceitoFinal = styled.td`
  box-shadow: -8px 0px 8px -4px #8080804d;
  background: ${Base.CinzaFundo};
`;
