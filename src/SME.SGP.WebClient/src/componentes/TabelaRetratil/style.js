import styled from 'styled-components';

// Componentes
import { Base } from '~/componentes';

export const TabelaEstilo = styled.div`
  display: flex;
  width: 100%;
`;

export const Tabela = styled.table`
  thead {
    background: ${Base.CinzaFundo};

    tr {
      th {
        padding: 0.7rem;
        border: 1px solid ${Base.CinzaDesabilitado};

        &:first-child {
          border-top-left-radius: 4px;
        }
      }
    }
  }

  tbody {
    tr {
      td {
        padding: 0.7rem;
        border: 1px solid ${Base.CinzaDesabilitado};
      }
    }
  }
`;

export const LinhaTabela = styled.tr`

	${props =>
    !props.ativo &&
    `
		background: ${Base.CinzaDesabilitado};
		color: ${Base.CinzaMako} !important;
		cursor: not-allowed !important;

		&:hover {
			background: ${Base.CinzaDesabilitado} !important;
		}
	`}

  &:hover {
    background: ${Base.Roxo};
    color: white;
		cursor: pointer;
		
		::after {
			content: 'italo',
			color: black;
			display: block;
		}
  }
`;

export const DetalhesAluno = styled.div`
  flex: 1;
  background: cyan;
`;

export const CabeçalhoDetalhes = styled.div``;
