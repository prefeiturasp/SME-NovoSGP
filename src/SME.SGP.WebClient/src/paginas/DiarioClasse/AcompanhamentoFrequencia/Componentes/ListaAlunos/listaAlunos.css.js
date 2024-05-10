import styled from 'styled-components';

import { Base } from '~/componentes/colors';

export const Container = styled.div``;

export const Marcadores = styled.div`
  vertical-align: bottom;
`;

export const MarcadorAulas = styled.div`
  background-color: ${Base.Roxo};
  color: ${Base.Branco};
  height: 22px;
  min-width: 123px;
  display: 'flex';
  align-items: 'center';
  justify-content: 'center';
  font-size: 12px;
  text-align: center;
  border-radius: 4px;
  margin-top: 15px;

  .numero {
    font-weight: bold;
  }
`;

export const TabelaColunasFixas = styled.div`
  font-size: 14px !important;
  table {
    border-collapse: separate;
    border-spacing: 0;
    margin-bottom: 0px !important ;
    border: solid 1px ${Base.CinzaDesabilitado};
  }
  .wrapper {
    position: relative;
    overflow: auto;
    white-space: nowrap;
    margin-bottom: 10px;
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
  .col-linha-um {
    width: 30%;
    left: 0px;
    z-index: 2;
  }
  .col-valor-linha-um {
    width: 30%;
    z-index: 2;
  }

  .col-linha-dois {
    text-align: center;
    width: 25%;
    left: 0px;
    z-index: 2;
  }
  .col-linha-quatro {
    text-align: left;
    width: 40%;
    left: 0px;
    z-index: 2;
  }

  .col-valor-linha-dois {
    width: 25%;
    z-index: 2;
  }
  .col-valor-linha-tres {
    text-align: center;
    width: 5%;
    z-index: 2;
    padding-top: 18px;
  }

  .divIconeSituacao {
    display: inline-flex;
    position: absolute;
    margin-top: 5px;
  }

  .iconeSituacao {
    background: ${Base.Roxo};
    width: 9px;
    height: 9px;
    border-radius: 50%;
    display: inline-block;
    margin-left: 0.2rem;
  }

  .col-valor-linha-quatro {
    text-align: left;
    width: 35%;
    z-index: 2;
  }

  .header-fixo {
    position: sticky;
    top: 0;
    z-index: 5;
  }

  .tabela-um-thead {
    background: ${Base.CinzaFundo} !important;
    text-align: center;
    th {
      border-right: solid 1px ${Base.CinzaDesabilitado};
      border-bottom: 1px solid ${Base.CinzaDesabilitado};
    }
    tr {
      border-left: solid 1px ${Base.CinzaDesabilitado};
    }
  }
  .tabela-um-tbody {
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
  .tabela-dois-thead {
    background: ${Base.CinzaFundo} !important;
    text-align: left;
    th {
      border-right: solid 1px ${Base.CinzaDesabilitado};
      border-bottom: 1px solid ${Base.CinzaDesabilitado};
    }
    tr {
      border-left: solid 1px ${Base.CinzaDesabilitado};
    }
  }
  .tabela-dois-tbody {
    text-align: left;
    border-left: solid 1px ${Base.CinzaDesabilitado};
    tr td {
      border-right: solid 1px ${Base.CinzaDesabilitado};
      border-bottom: 1px solid ${Base.CinzaDesabilitado};

      .iconeSituacao {
        background: ${Base.Roxo};
        width: 9px;
        height: 9px;
        border-radius: 50%;
        display: inline-block;
        margin-left: 0.2rem;
      }
    }
    .border-right-none {
      border-right: none !important;
    }
  }
  .btn-com-anotacao {
    color: #ffffff !important;
    border: solid 1px #297805 !important;
    background-color: #297805 !important;
  }
`;

export const ContainerColunaMotivoAusencia = styled.i`
  font-size: 19px;
  cursor: pointer;
  margin-left: 8px;
`;

export const BtnVisualizarAnotacao = styled.div`
  font-size: 12px;
  border-radius: 3px;
  color: #a4a4a4;
  border: solid 1px #f5f6f8;
  background-color: #f5f6f8;
  cursor: pointer;
  height: 32px;
  width: 32px;
  text-align: center;
`;
