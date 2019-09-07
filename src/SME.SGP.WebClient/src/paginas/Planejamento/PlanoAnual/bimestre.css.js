import { Base } from '../../../componentes/colors';
import styled from 'styled-components';

export const Badge = styled.button`
 &:last-child {
   margin-right: 10 !important;
 }

 &[aria-pressed='true'] {
   background: ${Base.CinzaBadge} !important;
   border-color: ${Base.CinzaBadge} !important;
 }
`;

export const ObjetivosList = styled.div`
 
 max-height: 300px !important;

`;

export const ListItem = styled.li`
 border-color: ${Base.AzulAnakiwa} !important;
`;

export const ListItemButton = styled(ListItem)`
 cursor: pointer;

 &[aria-pressed='true'] {
   background: ${Base.AzulAnakiwa} !important;
 }
`;
