import styled from 'styled-components';

import { Base } from '../../componentes/colors';

export const Container = styled.div``;

export const TabelaColunasFixas = styled.div`
  table {
    border-collapse: separate;
    border-spacing: 0;
    margin-bottom: 0px !important ;
  }

  .wrapper {
    position: relative;
    overflow: auto;
    white-space: nowrap;
    height: 400px;

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

  .sticky-col {
    position: sticky;
    position: -webkit-sticky;
    background-color: white;
  }

  .col-numero-chamada {
    width: 55px;
    min-width: 55px;
    max-width: 55px;
    left: 0px;
    z-index: 2;
    border-left: solid 1px ${Base.CinzaDesabilitado};
  }

  .col-nome-aluno {
    width: 100%;
    min-width: 250px;
    max-width: 250px;
    left: 55px;
    text-overflow: ellipsis;
    overflow: hidden;
    white-space: nowrap;
    text-align: left;
    z-index: 2;
    box-shadow: 8px 0px 8px -4px #8080804d;
    vertical-align: middle;
  }

  .col-frequencia {
    width: 100px;
    min-width: 100px;
    max-width: 100px;
    right: 0px;
    z-index: 2;
  }

  .col-nota-final {
    width: 125px;
    min-width: 125px;
    max-width: 125px;
    right: 100px;
    z-index: 2;
  }

  .header-fixo {
    position: sticky;
    top: 0;
    z-index: 5;
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

  .cabecalho-nota-conceito-final {
    box-shadow: -8px 0px 8px -4px #8080804d;
    border-bottom: 0 !important;
    vertical-align: middle;
    background: ${Base.CinzaFundo};
  }

  .cabecalho-frequencia {
    border-bottom: 0 !important;
    vertical-align: middle;
    background: ${Base.CinzaFundo};
  }

  .linha-nota-conceito-final {
    box-shadow: -8px 0px 8px -4px #8080804d;
    background: ${Base.CinzaFundo};
  }

  .linha-nota-conceito-final-clicada {
    box-shadow: -8px 0px 8px -4px #8080804d;
    background: ${Base.Roxo};
    color: white;
  }

  .linha-frequencia {
    background: ${Base.CinzaFundo};
  }

  .cinza-fundo {
    background: ${Base.CinzaFundo};
  }

  .aluno-ausente-notas {
    border-top: solid 3px #d06d12 !important;
    background-color: #d06d12 !important;
    border-radius: 7px !important;
    margin-right: 3px !important;
    margin-left: 10px !important;
    padding-right: 11px !important;
  }

  .icon-aluno-ausente {
    color: white;
    float: right;
    margin-top: -27px;
    font-size: 10px;
    margin-right: 5px;
  }

  .desabilitar-nota {
    opacity: 0.4 !important;
    cursor: unset !important;
  }

  .botao-ordenacao-avaliacao {
    float: left;
    position: absolute !important;
    z-index: 6;
  }

  .texto-header-avaliacao {
    text-overflow: ellipsis;
    overflow: hidden;
    white-space: nowrap;
    max-width: 140px;
    width: 140px;
  }

  .border-registro-alterado {
    border: solid 2px ${Base.Roxo} !important;
    border-radius: 7px;
  }

  .border-abaixo-media {
    border: solid 2px #b22222 !important;
    border-radius: 7px;
  }

  .select-conceitos {
    border-radius: 4px;
    margin-bottom: -3px;
  }

  .aluno-ausente-conceitos {
    border-top: solid 2px #d06d12 !important;
    background-color: #d06d12 !important;
    border-radius: 7px !important;
    border-right: solid 22px #d06d12 !important;
    padding-left: 2px !important;
  }

  .tamanho-conceito-final {
    width: 100px;
    max-width: 100px;
    min-width: 100px;
  }

  .linha-conceito-final {
    border-left: solid 1px ${Base.CinzaDesabilitado};
    box-shadow: 0px 0px 7px 3px ${Base.CinzaDesabilitado};
    border-radius: 3px;
    width: 400px;
    overflow: auto;
    position: relative;
    z-index: 3;

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

  .desc-linha-conceito-final {
    margin-left: 20px;
    margin-top: 25px;
    margin-right: 37px;
  }
`;

export const CaixaMarcadores = styled.span`
  border: 1.6px solid ${Base.Roxo};
  border-radius: 9px;
  padding-left: 10px;
  padding-right: 10px;
  font-weight: bold;
  color: ${Base.Roxo};
`;

export const IconePlusMarcadores = styled.i`
  color: ${Base.Roxo};
  font-size: 16px;
  margin-left: 5px;
  cursor: pointer;
  font-size: 18px;
  margin-top: 10px;
`;

export const InfoMarcador = styled.i`
  color: ${Base.Roxo} !important;
  font-size: 15px;
  margin-left: 2px;
  position: absolute;
  padding-top: 5px;
`;
