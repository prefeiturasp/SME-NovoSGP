import styled from 'styled-components';
import { Base } from '~/componentes/colors';

export const Lista = styled.div`
  max-width: 100%;
  min-width: 100%;

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
    cursor: pointer;

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


    .coluna-ordenacao-tr {
      border-left: none;
    }

    .col-nome-aluno {
      text-align: left !important;
      padding-left: 30px;
    }

    .head-conceito {
      width: 100px;
      min-width: 100px;
      max-width: 100px;
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
      width: 100%;
      max-width: 250px;
      min-width: 250px;
      text-align: left !important;
    }

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

    .coluna-regencia {
      display: flex;
      .select-conceito {
        margin: 10px;
        width: 80px;
      }
    }

    .linha-numero-chamada {
      display: inline !important;
      margin-left: 17px;
    }
  }

  .col-nota-conceito {
    width: 250px;
    max-width: 250px;
    min-width: 250px;
  }

  .col-numero-chamada {
    min-width: 54px;
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
    i {
      padding-top: 0;
    }
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

  .tamanho-conceito-final {
    width: 100px;
    max-width: 100px;
    min-width: 100px;
  }

  .border-abaixo-media {
    border: solid 2px #b22222 !important;
    border-radius: 7px;
  }

  .border-registro-alterado {
    border: solid 2px ${Base.Roxo} !important;
    border-radius: 7px;
  }

  .lista-disciplinas {
    display: flex;
    flex-wrap: wrap;
  }

  .indicativo-alerta {
    background-color: #ffff30;
    color: black;
    border-radius: 8px;
    border-right: solid 5px #ffff30;
    border-left: solid 5px #ffff30;
  }
`;

export const MaisMenos = styled.i`
  color: ${Base.Roxo} !important;
  font-size: 19px;
  cursor: pointer;
`;

export const Info = styled.i`
  color: ${Base.Roxo} !important;
  font-size: 10px;
  margin-left: 2px;
  margin-right: 5px;
  padding-top: 5px;
  display: inline !important;
`;

export const ContainerAuditoria = styled.div`
  object-fit: contain;
  font-family: Roboto;
  font-size: 9px;
  font-weight: bold;
  color: #42474a;
  float: left;

  p {
    margin-top: 0px;
    margin-bottom: 0px;
  }
`;
