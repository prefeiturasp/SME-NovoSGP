import styled from 'styled-components';
import { Base } from '~/componentes';

export const Corpo = styled.div`
  .negrito {
    font-weight: bold;
  }

  th,
  td {
    border: solid 1px ${Base.CinzaDesabilitado};
  }

  .bimestre-selecionado {
    td {
      background-color: ${Base.Roxo};
    }
  }

  .fundo-cinza {
    background-color: ${Base.CinzaFundo};
    vertical-align: middle !important;
  }

  .fundo-cinza-i {
    background-color: ${Base.CinzaFundo} !important;
  }

  .p-l-16 {
    padding-left: 16px !important;
  }

  .bc-w-i {
    background-color: ${Base.Branco} !important;
  }

  .ant-input-number-disabled {
    background-color: ${Base.Branco};
  }

  .ant-input-number-input {
    text-align: center !important;
  }
`;

export const CampoDesabilitado = styled.div`
  justify-content: center;
  align-items: center;
  display: flex;

  span {
    justify-content: center;
    align-items: center;
    display: flex;
    background-color: ${Base.Branco} !important;
    border-radius: 4px;
    border: 1px solid ${Base.CinzaDesabilitado};
    color: ${Base.CinzaDesabilitado} !important;
    width: 65px;
    height: 32px;
  }
`;

export const CampoEditavel = styled.div`
  justify-content: center;
  align-items: center;
  display: flex;
  padding-left: 1px;
  .ant-input-number {
    width: 67px !important;
  }
  div {
    height: 32px !important;
  }
  input {
    justify-content: center;
    align-items: center;
    display: flex;
    width: 65px;
    height: 32px !important;
  }
`;

export const CampoCentralizado = styled.div`
  justify-content: center;
  align-items: center;
  display: flex;
`;

export const CampoAlerta = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: ${Base.LaranjaAlerta};
  border-radius: 4px;
  width: 87px;
  height: 34px;
  padding-left: 1px;
  .ant-input-number {
    width: 67px !important;
  }
  div {
    height: 32px !important;
  }
  .icone {
    display: flex;
    justify-content: center;
    align-items: center;
    width: 20px;
    i {
      font-size: 8px;
      color: ${Base.Branco};
    }
  }
`;
