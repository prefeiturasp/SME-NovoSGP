import styled from 'styled-components';
import { Base } from '~/componentes/colors';

export const ContainerTabsComponentesCorriculares = styled.div`
  .ant-tabs-nav {
    width: ${props => props.widthAntTabsNav} !important;
  }
`;

export const DescricaoNomeTabComponenteCurricular = styled.span`
  .desc-nome {
    color: ${props =>
      props.tabSelecionada ? `${Base.Roxo}` : `${Base.Verde}`};
  }

  i {
    color: ${Base.Verde} !important;
  }
`;
