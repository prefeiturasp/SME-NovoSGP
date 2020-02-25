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
`
