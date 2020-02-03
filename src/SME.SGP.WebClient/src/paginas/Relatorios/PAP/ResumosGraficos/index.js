import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import { Cabecalho } from '~/componentes-sgp';
import { Card, ButtonGroup } from '~/componentes';
import RotasDto from '~/dtos/rotasDto';

import Filtro from './componentes/Filtro';

const ResumosGraficosPAP = () => {
  const permissoesTela = useSelector(store => store.usuario.permissoes);
  const [somenteConsulta] = useState(false);

  const onClickVoltar = () => {};

  return (
    <>
      <Cabecalho pagina="Resumos e gráficos PAP" />
      <Card>
        <ButtonGroup
          somenteConsulta={somenteConsulta}
          permissoesTela={permissoesTela[RotasDto.PAP]}
          onClickVoltar={onClickVoltar}
          desabilitarBotaoPrincipal
        />
        <Filtro />
      </Card>
    </>
  );
};

export default ResumosGraficosPAP;
