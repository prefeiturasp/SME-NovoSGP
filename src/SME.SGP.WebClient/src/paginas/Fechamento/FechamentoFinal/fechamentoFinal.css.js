import styled from 'styled-components';
import { Base } from '~/componentes/colors';

export const Lista = styled.div`
  max-width: 100%;
  min-width: 100%;

  .desabilitar-nota {
    opacity: 0.4 !important;
    cursor: unset !important;
  }
  .btn-disciplina {
    height: 38px !important;
    border-radius: 4px;
    border: solid 1px rgba(0, 0, 0, 0.15);
    background-color: #f3f3f3;
    font-family: Roboto;
    font-size: 12px;
    font-weight: normal;
    font-stretch: normal;
    font-style: normal;
    line-height: 1.58;
    letter-spacing: normal;
    color: #42474a;
    padding: 10px;
    margin: 5px;

    &.ativa {
      color: #ffffff;
      border: solid 1px #490cf5;
      background-color: #490cf5;
    }
  }

  .botao-ordenacao-avaliacao {
    display: flex;
    justify-content: flex-end;
    .btn-ordenacao {
      margin-right: 31% !important;
      margin-top: -7px;
    }
    span {
      cursor: pointer;
    }
  }

  .tabela-fechamento-final-thead {
    background: ${Base.CinzaFundo} !important;
    text-align: center;
    th {
      border-right: solid 1px ${Base.CinzaDesabilitado};
      border-bottom: 1px solid ${Base.CinzaDesabilitado};
      vertical-align: middle;
      padding: 4px;
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
    .col-nome-aluno {
      text-align: left !important;
      padding-left: 30px;
    }
    .head-conceito {
      width: 75px;
      min-width: 75px;
      max-width: 75px;
    }
  }

  .tabela-fechamento-final-tbody {
    text-align: center;
    border-left: solid 1px ${Base.CinzaDesabilitado};

    tr td {
      border-right: solid 1px ${Base.CinzaDesabilitado};
      border-bottom: 1px solid ${Base.CinzaDesabilitado};
      padding-bottom: 0px;
      vertical-align: middle;
    }

    tr.linha-conceito-regencia td {
      vertical-align: top !important;
    }

    .border-right-none {
      border-right: none !important;
    }
    .col-nome-aluno {
      width: 250px;
      max-width: 250px;
      min-width: 250px;
      text-align: left !important;
    }

    /* .col-conceito-regencia {
      float: left;
      margin-right: 5px;
      width: 79px;
    } */
    .ant-input-number {
      width: 79px;
    }

    .linha-conceito-regencia {
      background-color: #f3f3f3;
      padding-top: 0px;
      max-height: 120px;
      min-height: 100px;
      height: 110px;
    }

    .elevation-3 {
      box-shadow: 0 0 28px rgba(0, 0, 0, 0.25),
        2px -6px 10px rgba(0, 0, 0, 0.22);
    }

    .col-teste {
      display: flex;
      flex-wrap: wrap;
    }

    .input-teste {
      flex: 1 0 auto; /* explanation below */
      margin-right: 5px;
      margin-bottom: 5px;
      height: 70px;
      background-color: white;
      border-radius: 3px;
      border: solid 1px #ced4da;
      padding: 3px;
    }

    .input-reg {
      float: left;
      padding-left: 5px;
    }
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
    padding-top: 9px;
    margin-bottom: 9px;
    color: #a4a4a4;
    float: left;
  }

  .width-120 {
    width: 120px;
    max-width: 120px;
    min-width: 120px;
  }
`;

export const MaisMenos = styled.i`
  color: ${Base.Roxo} !important;
  font-size: 18px;
  cursor: pointer;
`;

export const CampoNumerico = styled.div`
  /* span {
    color: ${Base.Vermelho};
  }
  .campo {
    margin-bottom: 5px;
  }
  .ant-input-number {
    height: 38px;
  }

  .tb-conceito-regencia {
    table {
      width: 100%;
    }
    thead th,
    tbody td {
      border: none;
    }
  }
  float: left;
  margin-right: 5px;
  width: 79px;
  height: 45px; */
  /* display:flex; */
`;
