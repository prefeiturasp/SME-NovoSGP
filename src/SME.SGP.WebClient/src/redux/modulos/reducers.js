import { combineReducers } from 'redux';

import notificacoes from './alertas/reducers';
import usuario from './usuario/reducers';

export default combineReducers({
  notificacoes,
  usuario,
});
