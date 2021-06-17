import PropTypes from 'prop-types';
import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import CollapseAtribuicaoResponsavel from '~/componentes-sgp/CollapseAtribuicaoResponsavel/collapseAtribuicaoResponsavel';
import { setExibirLoaderEncaminhamentoAEE } from '~/redux/modulos/encaminhamentoAEE/actions';
import { erros } from '~/servicos';
import ServicoEncaminhamentoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoEncaminhamentoAEE';

const AtribuicaoResponsavel = props => {
  const { match } = props;

  const dispatch = useDispatch();

  const dadosEncaminhamento = useSelector(
    store => store.encaminhamentoAEE.dadosEncaminhamento
  );

  const limparAtualizarDados = () => {
    const encaminhamentoId = match?.params?.id;
    ServicoEncaminhamentoAEE.obterEncaminhamentoPorId(encaminhamentoId);
  };

  const params = {
    url: 'v1/encaminhamento-aee/responsavel/pesquisa',
    codigoTurma: dadosEncaminhamento?.turma?.codigo,
    validarAntesAtribuirResponsavel: async funcionario => {
      const encaminhamentoId = match?.params?.id;
      const resposta = await ServicoEncaminhamentoAEE.atribuirResponsavelEncaminhamento(
        funcionario.codigoRF,
        encaminhamentoId
      ).catch(e => erros(e));

      if (resposta?.data) {
        limparAtualizarDados();
        return true;
      }
      return false;
    },
    clickRemoverResponsavel: async () => {
      const encaminhamentoId = match?.params?.id;
      if (encaminhamentoId) {
        dispatch(setExibirLoaderEncaminhamentoAEE(true));
        const retorno = await ServicoEncaminhamentoAEE.removerResponsavel(
          encaminhamentoId
        )
          .catch(e => erros(e))
          .finally(() => dispatch(setExibirLoaderEncaminhamentoAEE(false)));
        if (retorno?.status === 200) {
          limparAtualizarDados();
        }
      }
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
