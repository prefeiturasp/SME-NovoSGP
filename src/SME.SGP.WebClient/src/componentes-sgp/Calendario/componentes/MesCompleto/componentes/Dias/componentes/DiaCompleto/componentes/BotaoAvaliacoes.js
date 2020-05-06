import React, { useCallback } from 'react';
import shortid from 'shortid';
import t from 'prop-types';

// Componentes
import { SelectComponent, Base, Colors } from '~/componentes';

// Estilos
import { Botao } from '../styles';

// DTOs
import RotasDTO from '~/dtos/rotasDto';

// Serviços
import history from '~/servicos/history';

function BotaoAvaliacoes({ atividadesAvaliativas, permissaoTela }) {
  const onClickAvaliacaoHandler = useCallback(
    avaliacao => {
      if (permissaoTela && permissaoTela.podeConsultar) {
        history.push(`${RotasDTO.CADASTRO_DE_AVALIACAO}/editar/${avaliacao}`);
      }
    },
    [permissaoTela]
  );

  return (
    <div className="pr-0 d-flex align-items-center px-2 p-x-md-3">
      {atividadesAvaliativas.length > 1 ? (
        <SelectComponent
          lista={atividadesAvaliativas}
          classNameContainer="w-100"
          className="fonte-14"
          onChange={avaliacaoAtual => onClickAvaliacaoHandler(avaliacaoAtual)}
          valueSelect={atividadesAvaliativas[0].id}
          valueOption="id"
          valueText="descricao"
          placeholder="Avaliação"
          size="small"
          border={Base.Roxo}
          color={Base.Roxo}
        />
      ) : (
        atividadesAvaliativas &&
        atividadesAvaliativas.length === 1 && (
          <Botao
            id={shortid.generate()}
            label={`${atividadesAvaliativas[0].descricao}`}
            color={Colors.Roxo}
            className="w-100 position-relative btn-sm zIndex"
            onClick={() => onClickAvaliacaoHandler(atividadesAvaliativas[0].id)}
            height="24px"
            padding="0 1rem"
            border
          />
        )
      )}
    </div>
  );
}

BotaoAvaliacoes.propTypes = {
  atividadesAvaliativas: t.oneOfType([t.any]),
  permissaoTela: t.oneOfType([t.any]),
};

BotaoAvaliacoes.defaultProps = {
  atividadesAvaliativas: [],
  permissaoTela: {},
};

export default BotaoAvaliacoes;
