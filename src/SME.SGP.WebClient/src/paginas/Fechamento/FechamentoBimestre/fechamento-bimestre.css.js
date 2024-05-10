import styled from 'styled-components';
import { Base } from '~/componentes';

export const Fechamento = styled.div`
  .ant-tabs-nav {
    width: 20% !important;
  }

  .ant-tabs-tab {
    &:last-child{
      background: ${Base.CinzaDesabilitado} !important;
    }
  }

  .btn-com-anotacao {
    color: #ffffff !important;
    border: solid 1px #297805 !important;
    background-color: #297805 !important;
  }
`

export const BtbAnotacao = styled.div`
  font-size: 10px;
  border-radius: 3px;
  padding: 5px;
  cursor: pointer;
  color: #a4a4a4;
  border: solid 1px #f5f6f8;
  background-color: #f5f6f8;
`;

