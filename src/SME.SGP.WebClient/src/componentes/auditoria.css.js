import styled from 'styled-components';

export const Container = styled.div`
  object-fit: contain;
  font-family: Roboto;
  font-size: 9px;
  font-weight: bold;
  color: #42474a;
  margin-top: ${p => (p.ignorarMarginTop ? '0px' : '16px')};
  width: 100%;
`;
