import styled from 'styled-components';
import { Base } from '~/componentes/colors';

export const ContainerModal = styled.div`
  .ant-modal-footer {
    border-top: 0px !important;
  }
`;

export const VerticalCentered = styled.div`
  display: flex;
  align-items: center;
  color: ${Base.CinzaMako};
`;

export const IconMargin = styled.i`
  margin-right: 0.5rem;
`;

export const DefaultDropDownLink = styled.a`
  color: ${Base.CinzaMako};
`;
