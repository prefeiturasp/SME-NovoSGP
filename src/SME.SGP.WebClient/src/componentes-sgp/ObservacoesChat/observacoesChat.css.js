import { Input } from 'antd';
import styled from 'styled-components';
import { Base } from '~/componentes/colors';

const { TextArea } = Input;

export const ContainerObservacoesChat = styled.div`
  background: #ffffff;
  border: 1px solid #dadada;
  box-sizing: border-box;
  box-shadow: 0px 1px 4px rgba(8, 35, 48, 0.1);
  border-radius: 4px;

  .btn-acao {
    background-color: ${Base.Azul} !important;
    color: ${Base.Branco} !important;
    i {
      margin-right: 0px !important;
      font-size: 11px;
    }
  }
`;

export const CampoObservacao = styled(TextArea)`
  font-family: Roboto;
  font-size: 14px !important;
  font-weight: normal !important;
  font-stretch: normal !important;
  font-style: normal !important;
  line-height: 1.3 !important;
  letter-spacing: normal !important;
  color: #42474a;
`;

export const LinhaObservacao = styled.div`
  font-family: Roboto;
  font-style: normal;
  font-weight: normal;
  font-size: 14px;
  color: #000000;

  border: 1px solid #d9d9d9;
  border-radius: 4px;
  padding: 4px 11px;
  min-height: 64px;
  line-height: 1.3 !important;
`;
