import styled from 'styled-components';

import { Base } from '../../componentes/colors';

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

export const Lista = styled.div`
  .presenca {
    .ant-switch::after {
      content: 'C' !important;
      background-color: #297805;
      color: white;
    }
  }

  .falta {
    .ant-switch::after {
      content: 'F' !important;
      background-color: #b40c02;
      color: white;
    }
  }

  .ant-switch-checked {
    background-color: white;
  }

  .ant-switch {
    border: solid 1px ${Base.CinzaDesabilitado};
    background-color: white;
  }

  .ant-switch-inner {
    color: grey;
  }

  width: 100%;

  .tabela-frequencia-thead {
    background: ${Base.CinzaFundo} !important;
    text-align: center;
    border-left: solid 1px ${Base.CinzaDesabilitado};

    th {
      border-right: solid 1px ${Base.CinzaDesabilitado};
      border-bottom: 1px solid ${Base.CinzaDesabilitado};
    }

    .border-right-none {
      border-right: none !important;
    }
  }

  .tabela-frequencia-tbody {
    text-align: center;
    border-left: solid 1px ${Base.CinzaDesabilitado};

    tr td {
      border-right: solid 1px ${Base.CinzaDesabilitado};
      border-bottom: 1px solid ${Base.CinzaDesabilitado};
    }

    .border-right-none {
      border-right: none !important;
    }

    .btn-falta-presenca {
      color: white;
      background-color: ${Base.CinzaDesabilitado};
    }

    .btn-falta {
      color: white;
      background-color: #b40c02;
    }

    .btn-compareceu {
      color: white;
      background-color: #297805;
    }

    .ant-btn-sm {
      width: 20px;
      height: 20px;
      font-size: 12px;
    }

    .ant-btn:hover,
    .ant-btn:focus {
      border-color: transparent;
    }
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

  .desabilitar-aluno {
    opacity: 0.4 !important;
    cursor: unset !important;
  }

  .scroll-tabela-frequencia-thead {
    overflow-y: scroll;
    ::-webkit-scrollbar {
      width: 9px !important;
      background-color: rgba(229, 237, 244, 0.71) !important;
    }
  }

  .scroll-tabela-frequencia-tbody {
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

  .marcar-todas-frequencia {
    cursor: unset !important;
    position: absolute;
    margin-left: -13px;
    margin-top: -17px;
    font-size: 10px;
    width: 100px;
    height: 15px;
    background-color: ${Base.CinzaDesabilitado};
  }

  .margin-marcar-todos {
    margin-bottom: -6px;
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

  .indicativo-alerta {
    background-color: #ffff30;
    color: black;
    border-radius: 8px;
    border-right: solid 5px #ffff30;
    border-left: solid 5px #ffff30;
  }

  .indicativo-critico {
    background-color: #b40c02;
    color: white;
    border-radius: 8px;
    border-right: solid 5px #b40c02;
    border-left: solid 5px #b40c02;
  }

  .btn-com-anotacao {
    color: #ffffff !important;
    border: solid 1px #297805 !important;
    background-color: #297805 !important;
  }
`;

export const BtbAnotacao = styled.div`
  font-size: 10px;
  border-radius: 3px;
  padding: 5px;
  color: #a4a4a4;
  border: solid 1px #f5f6f8;
  background-color: #f5f6f8;
  cursor: ${props => (props.podeAbrirModal ? 'pointer' : 'not-allowed')};
`;

export const MarcadorSituacao = styled.i`
  color: ${Base.Roxo} !important;
  font-size: 10px;
  margin-left: 2px;
  padding-top: 5px;
`;
