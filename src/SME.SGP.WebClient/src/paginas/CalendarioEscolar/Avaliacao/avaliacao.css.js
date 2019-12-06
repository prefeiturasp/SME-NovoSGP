import styled from 'styled-components';
import { Base } from '~/componentes';

export const Div = styled.div``;

export const Titulo = styled(Div)`
  color: ${Base.CinzaMako};
  font-size: 24px;
`;

export const Badge = styled.button`
  &:last-child {
    margin-right: 10 !important;
  }

  &[aria-pressed='true'] {
    background: ${Base.CinzaBadge} !important;
    border-color: ${Base.CinzaBadge} !important;
  }
`;
