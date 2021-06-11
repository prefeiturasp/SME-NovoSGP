import * as moment from 'moment';
import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Loader } from '~/componentes';
import { RegistrosAnterioresConteudo } from '~/componentes-sgp/RegistroIndividual/registrosAnteriores/registrosAnterioresConteudo';
import CardCollapse from '~/componentes/cardCollapse';
import { RotasDto } from '~/dtos';
import { limparDadosRegistroIndividual } from '~/redux/modulos/registroIndividual/actions';

const RegistrosIndividuais = props => {
  const { semestreSelecionado } = props;

  const exibirLoaderGeralRegistroAnteriores = useSelector(
    store => store.registroIndividual.exibirLoaderGeralRegistroAnteriores
  );

  const dadosAcompanhamentoAprendizagem = useSelector(
    store => store.acompanhamentoAprendizagem.dadosAcompanhamentoAprendizagem
  );

  const desabilitarCamposAcompanhamentoAprendizagem = useSelector(
    store =>
      store.acompanhamentoAprendizagem
        .desabilitarCamposAcompanhamentoAprendizagem
  );

  const dispatch = useDispatch();

  const usuario = useSelector(store => store.usuario);
  const permissoesTela = usuario.permissoes[RotasDto.REGISTRO_INDIVIDUAL];

  const [exibirRegistroIndividual, setExibirRegistroIndividual] = useState(
    false
  );

  useEffect(() => {
    if (dadosAcompanhamentoAprendizagem?.periodoInicio) {
      const hoje = moment();
      const periodoInicio = moment(
        dadosAcompanhamentoAprendizagem.periodoInicio
      );
      const exibir = hoje.isSameOrAfter(periodoInicio);
      setExibirRegistroIndividual(exibir);
    } else {
      setExibirRegistroIndividual(false);
    }
  }, [dadosAcompanhamentoAprendizagem]);

  useEffect(() => {
    return () => {
      dispatch(limparDadosRegistroIndividual());
    };
  }, [dispatch]);

  return (
    <Loader
      ignorarTip
      loading={exibirLoaderGeralRegistroAnteriores}
      className="w-100"
    >
      <div className="col-md-12 mb-2">
        <CardCollapse
          key="registros-individuais-collapse"
          titulo="Registros individuais"
          indice="registros-individuais"
          alt="registros-individuais"
        >
          {exibirRegistroIndividual ? (
            <RegistrosAnterioresConteudo
              permissoesTela={permissoesTela}
              periodoInicio={dadosAcompanhamentoAprendizagem?.periodoInicio}
              periodoFim={dadosAcompanhamentoAprendizagem?.periodoFim}
              podeEditar={!desabilitarCamposAcompanhamentoAprendizagem}
            />
          ) : (
            <div className="text-center">
              {`O período do ${semestreSelecionado}º semestre não foi iniciado`}
            </div>
          )}
        </CardCollapse>
      </div>
    </Loader>
  );
};

RegistrosIndividuais.propTypes = {
  semestreSelecionado: PropTypes.string,
};

RegistrosIndividuais.defaultProps = {
  semestreSelecionado: '',
};

export default RegistrosIndividuais;
