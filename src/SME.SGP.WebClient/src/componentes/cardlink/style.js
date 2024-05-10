import styled from 'styled-components';

export const CardLine = styled.div`
  border-bottom: 5.8px solid ${props => props.color} !important;
  color: ${props => props.color} !important;
  background: ${props => props.background} !important;

  &:hover {
    color: ${props => props.colorActive} !important;
    background: ${props => props.backgroundActive} !important;

    h2,
    i {
      color: ${props => props.colorActive};
    }
  }

  &:not(:hover) {
    color: ${props => props.color} !important;
    background: ${props => props.background} !important;

    h2,
    i {
      color: ${props => props.color};
    }
  }
`;

export const LabelCardLink = styled.h2`
  font-size: 16px;
  margin-top: 20px;
  font-family: Roboto;
  font-size: 16px;
  font-weight: 500;
`;

export const DivCardLink = styled.div`
  min-height: ${props => props.minHeight};
  height: 100%;
  .card-body {
    min-height: ${props => props.minHeight} !important;
  }
  .altura-100 {
    height: 100%;
  }
`;
