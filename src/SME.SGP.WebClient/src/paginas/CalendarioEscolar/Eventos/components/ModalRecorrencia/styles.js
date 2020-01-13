import styled from 'styled-components';
import { Base } from '~/componentes/colors';

export const ContainerModal = styled.div`
  .ant-modal-footer {
    border-top: 0px !important;
  }
`;

export const VerticalCentralizado = styled.div`
  display: flex;
  align-items: center;
  color: ${Base.CinzaMako};

  .item {
    margin-right: 16px;
  }
`;

export const IconMargin = styled.i`
  margin-right: 0.5rem;
`;

export const DefaultDropDownLink = styled.a`
  color: ${Base.CinzaMako};
  display: flex;
  align-items: center;

  .anticon {
    font-size: 15px !important;
    margin-left: 5px;
  }
`;

export const TextoPequeno = styled.small`
  color: ${Base.RoxoFundo};
`;
