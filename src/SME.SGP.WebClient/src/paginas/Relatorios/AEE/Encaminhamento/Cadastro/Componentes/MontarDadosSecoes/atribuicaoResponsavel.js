import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import CollapseAtribuicaoResponsavel from '~/componentes-sgp/CollapseAtribuicaoResponsavel/collapseAtribuicaoResponsavel';
import { erros } from '~/servicos';
import ServicoEncaminhamentoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoEncaminhamentoAEE';

const AtribuicaoResponsavel = props => {
  const { match } = props;

  const dadosEncaminhamento = useSelector(
    store => store.encaminhamentoAEE.dadosEncaminhamento
  );

  const params = {
    url: 'v1/encaminhamento-aee/responsavel/pesquisa',
    codigoTurma: dadosEncaminhamento?.turma?.codigo,
    validarAntesProximoPasso: async funcionario => {
      const encaminhamentoId = match?.params?.id;
      const resposta = await ServicoEncaminhamentoAEE.atribuirResponsavelEncaminhamento(
        funcionario.codigoRF,
        encaminhamentoId
      ).catch(e => erros(e));

      if (resposta?.data) {
        return true;
      }
      return false;
    },
  };

  return dadosEncaminhamento?.podeAtribuirResponsavel ? (
    <CollapseAtribuicaoResponsavel {...params} />
  ) : (
    ''
  );
};

AtribuicaoResponsavel.propTypes = {
  match: PropTypes.oneOfType([PropTypes.object]),
};

AtribuicaoResponsavel.defaultProps = {
  match: {},
};

export default AtribuicaoResponsavel;
