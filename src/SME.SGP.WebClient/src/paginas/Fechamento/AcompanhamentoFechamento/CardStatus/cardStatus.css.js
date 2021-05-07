import styled from 'styled-components';

export const Container = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0 16px;

  width: 320px;
  height: 56px;

  background: #ffffff;
  border: 1px solid #bfbfbf;
  border-left: ${({ corStatus }) => corStatus && `4px solid ${corStatus}`};
  color: ${({ corStatus }) => corStatus};
  box-sizing: border-box;
  box-shadow: 0px 1px 4px rgba(8, 35, 48, 0.1);
  border-radius: 4px;

  .descricao {
    font-weight: 500;
    font-size: 14px;
    line-height: 24px;
  }

  .quantidade {
    font-weight: 700;
    font-size: 16px;
    line-height: 24px;
  }
`;
