import { combineReducers } from 'redux';

import navegacao from './navegacao/reducers';
import alertas from './alertas/reducers';
import usuario from './usuario/reducers';
import notificacoes from './notificacoes/reducers';
import perfil from './perfil/reducers';
import bimestres from './planoAnual/reducers';
import filtro from './filtro/reducers';

export default combineReducers({
  navegacao,
  alertas,
  usuario,
  notificacoes,
  perfil,
  bimestres,
  filtro,
});
