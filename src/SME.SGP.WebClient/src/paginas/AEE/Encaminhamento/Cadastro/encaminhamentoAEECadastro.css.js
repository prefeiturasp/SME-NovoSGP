import { Steps } from 'antd';
import styled from 'styled-components';
import { Base } from '~/componentes/colors';

export const ContainerStepsEncaminhamento = styled(Steps)`
  margin-top: 1rem !important;
  margin-bottom: 1rem !important;

  .card-header {
    a {
      display: flex;
    }
  }

  .ant-steps-item-title {
    width: 100%;
    padding-right: 0px;
    line-height: normal !important;
  }

  .ant-steps-item-container {
    display: flex;
  }

  .ant-steps-item-tail {
    top: 16px !important;
  }

  .ant-steps-item-content {
    display: flex;
    width: 100%;
  }

  .ant-steps-item-process .ant-steps-item-icon {
    background: ${Base.Roxo};
    border-color: ${Base.Roxo};
    margin-top: 18px;
  }

  .ant-steps-item-finish .ant-steps-item-icon {
    border-color: ${Base.Roxo};
    line-height: 26px;
    margin-top: 18px;

    .ant-steps-icon {
      color: ${Base.Roxo};
    }
  }

  .ant-steps-item-finish
    > .ant-steps-item-container
    > .ant-steps-item-tail::after {
    background-color: ${Base.Roxo};
  }

  .btn-excluir-atendimento-clinico {
    background-color: ${Base.Azul} !important;
    color: ${Base.Branco} !important;
    i {
      margin-right: 0px !important;
      font-size: 11px;
    }
  }

  .tabela-invalida {
    border: 1px solid #dc3545;
  }
`;
