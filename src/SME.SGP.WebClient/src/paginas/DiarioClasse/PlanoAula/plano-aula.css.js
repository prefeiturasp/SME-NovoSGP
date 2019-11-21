import styled from 'styled-components';
import { Base } from '~/componentes';

export const QuantidadeBotoes = styled.div`
padding: 0 0 20px 0;
`;

export const ObjetivosList = styled.div`
max-height: 300px !important;
`;

export const ListItem = styled.li`
border-color: ${Base.AzulAnakiwa} !important;
`;

export const ListItemButton = styled.li`
border-color: ${Base.AzulAnakiwa} !important;
cursor: pointer;
`;

export const Corpo = styled.div`
.selecionado{
  background: ${Base.AzulAnakiwa} !important;
}
`;

export const Descritivo = styled.h6`
padding-top: 10px;
color: ${Base.CinzaBotao} !important;
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