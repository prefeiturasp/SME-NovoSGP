import PropTypes from 'prop-types';
import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import CollapseAtribuicaoResponsavel from '~/componentes-sgp/CollapseAtribuicaoResponsavel/collapseAtribuicaoResponsavel';
import { setLimparDadosAtribuicaoResponsavel } from '~/redux/modulos/collapseAtribuicaoResponsavel/actions';
import { setLimparDadosLocalizarEstudante } from '~/redux/modulos/collapseLocalizarEstudante/actions';
import { setLimparDadosEncaminhamento } from '~/redux/modulos/encaminhamentoAEE/actions';
import { erros } from '~/servicos';
import ServicoEncaminhamentoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoEncaminhamentoAEE';

const AtribuicaoResponsavel = props => {
  const { match } = props;

  const dispatch = useDispatch();

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
        dispatch(setLimparDadosAtribuicaoResponsavel());
        dispatch(setLimparDadosLocalizarEstudante());
        dispatch(setLimparDadosEncaminhamento());
        ServicoEncaminhamentoAEE.obterEncaminhamentoPorId(encaminhamentoId);
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
