import React from 'react';
import t from 'prop-types';
import shortid from 'shortid';

// Componentes
import { Colors } from '~/componentes';

// Estilos
import { BotoesAuxiliaresEstilo, Botao } from '../styles';

const BotoesAuxiliares = ({
  temAula,
  podeCadastrarAvaliacao,
  podeCadastrarAula,
  onClickNovaAula,
  onClickNovaAvaliacao,
  permissaoTela,
  dentroPeriodo,
  desabilitado,
}) => {
  return (
    <BotoesAuxiliaresEstilo>
      <Botao
        id={shortid.generate()}
        key={shortid.generate()}
        onClick={onClickNovaAvaliacao}
        label="Nova Avaliação"
        color={Colors.Roxo}
        disabled={
          !permissaoTela?.podeIncluir ||
          !dentroPeriodo ||
          !temAula ||
          !podeCadastrarAvaliacao ||
          desabilitado
        }
        className="mr-3"
      />
      <Botao
        id={shortid.generate()}
        key={shortid.generate()}
        onClick={onClickNovaAula}
        label="Nova Aula"
        color={Colors.Roxo}
        disabled={
          !permissaoTela?.podeIncluir ||
          !podeCadastrarAula ||
          !dentroPeriodo ||
          desabilitado
        }
      />
    </BotoesAuxiliaresEstilo>
  );
};

BotoesAuxiliares.propTypes = {
  temAula: t.bool.isRequired,
  podeCadastrarAvaliacao: t.oneOfType([t.bool, t.number]).isRequired,
  podeCadastrarAula: t.oneOfType([t.bool, t.number]).isRequired,
  onClickNovaAula: t.func.isRequired,
  onClickNovaAvaliacao: t.func.isRequired,
  permissaoTela: t.oneOfType([t.any]).isRequired,
  dentroPeriodo: t.bool.isRequired,
  desabilitado: t.bool.isRequired,
};

export default BotoesAuxiliares;
