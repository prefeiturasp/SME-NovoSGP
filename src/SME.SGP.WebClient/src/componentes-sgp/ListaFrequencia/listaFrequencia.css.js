import styled from 'styled-components';

import { Base } from '../../componentes/colors';

export const Rectangle = styled.div`
  width: 66px;
  height: 16px;
  border-radius: 4px;
  background-color: var(--white);
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

  .scroll-tabela-frequencia {
    max-height: 240px;
    overflow-y: scroll;
  }
`;
