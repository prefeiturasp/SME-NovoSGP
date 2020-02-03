import styled from 'styled-components';

export const Corpo = styled.div`
  display: flex;
  justify-content: center;
  flex-direction: column;
  align-items: center;
  min-height: 400px;
  font-size: 16px;
  span {
    padding-bottom: 10px !important;
  }

  .msg-principal {
    font-size: 24px;
  }

  .not-found {
    font-size: 70px;
    padding-bottom: 10px;
  }
`;
