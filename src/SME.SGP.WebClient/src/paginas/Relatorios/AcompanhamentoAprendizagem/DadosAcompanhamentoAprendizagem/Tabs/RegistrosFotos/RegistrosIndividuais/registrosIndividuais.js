import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Loader } from '~/componentes';
import { RegistrosAnterioresConteudo } from '~/componentes-sgp/RegistroIndividual/registrosAnteriores/registrosAnterioresConteudo';
import CardCollapse from '~/componentes/cardCollapse';
import { RotasDto } from '~/dtos';
import { limparDadosRegistroIndividual } from '~/redux/modulos/registroIndividual/actions';

const RegistrosIndividuais = () => {
  const exibirLoaderGeralRegistroAnteriores = useSelector(
    store => store.registroIndividual.exibirLoaderGeralRegistroAnteriores
  );

  const dadosAcompanhamentoAprendizagem = useSelector(
    store => store.acompanhamentoAprendizagem.dadosAcompanhamentoAprendizagem
  );

  const dispatch = useDispatch();

  const usuario = useSelector(store => store.usuario);
  const permissoesTela =
    usuario.permissoes[RotasDto.ACOMPANHAMENTO_APRENDIZAGEM];

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
          <RegistrosAnterioresConteudo
            permissoesTela={permissoesTela}
            periodoInicio={dadosAcompanhamentoAprendizagem?.periodoInicio}
            periodoFim={dadosAcompanhamentoAprendizagem?.periodoFim}
            podeEditar={dadosAcompanhamentoAprendizagem?.podeEditar}
          />
        </CardCollapse>
      </div>
    </Loader>
  );
};

export default RegistrosIndividuais;
