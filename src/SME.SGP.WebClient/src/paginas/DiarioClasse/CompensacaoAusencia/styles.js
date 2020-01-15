import styled from 'styled-components';
import { Base } from '~/componentes';

export const AlunosCompensacao = styled.div`
  span {
    padding: 3px 9px 3px 9px;
    margin: 6px;
    border-radius: 10px;
    box-shadow: 0 0 2px 0 rgba(12, 35, 48, 0.3);
    font-size: 11px;
    color: rgba(0, 0, 0, 0.65);
    background-color: #ffffff;
  }
`;

export const Badge = styled.button`
  &:last-child {
    margin-right: 10 !important;
  }

  &[aria-pressed='true'] {
    background: ${Base.Roxo} !important;
    color: white !important;
  }
`;
