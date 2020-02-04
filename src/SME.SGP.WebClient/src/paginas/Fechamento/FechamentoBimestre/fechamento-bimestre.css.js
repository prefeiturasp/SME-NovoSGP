import styled from 'styled-components';
import { Base } from '~/componentes';

export const Marcadores = styled.div`
  vertical-align: bottom;
`;

export const MarcadorAulas = styled.div`
  background-color: ${Base.Roxo};
  color:${Base.Branco};
  height: 22px;
  min-width: 123px;
  display: 'flex';
  align-items: 'center';
  justify-content: 'center';
  font-size:12px;
  text-align: center;
  border-radius: 4px;
  margin-top: 15px;

  .numero{
    font-weight: bold;
  }
`;
