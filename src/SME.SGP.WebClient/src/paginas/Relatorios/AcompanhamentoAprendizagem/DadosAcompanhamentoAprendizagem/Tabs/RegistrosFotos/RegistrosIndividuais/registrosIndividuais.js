import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import { Loader } from '~/componentes';
import { RegistrosAnterioresConteudo } from '~/componentes-sgp/RegistroIndividual/registrosAnteriores/registrosAnterioresConteudo';
import CardCollapse from '~/componentes/cardCollapse';
import { RotasDto } from '~/dtos';

const RegistrosIndividuais = props => {
  const exibirLoaderGeralRegistroAnteriores = useSelector(
    store => store.registroIndividual.exibirLoaderGeralRegistroAnteriores
  );

  const usuario = useSelector(store => store.usuario);
  const permissoesTela =
    usuario.permissoes[RotasDto.ACOMPANHAMENTO_APRENDIZAGEM];

  const { semestreSelecionado } = props;

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
          <RegistrosAnterioresConteudo permissoesTela={permissoesTela} />
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
