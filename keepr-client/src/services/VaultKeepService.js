// import { AppState } from '../AppState'
import { logger } from '../utils/Logger'
import { api } from './AxiosService'
import { keepService } from './KeepService'

class VaultKeepService {
  async create(newVaultKeep) {
    try {
      await api.post('api/vaultkeeps', newVaultKeep)
    } catch (error) {
      logger.log(error)
    }
  }

  async deleteKeepFromVault(vaultKeepId, vaultId) {
    try {
      await api.delete('api/vaultkeeps/' + vaultKeepId)
      keepService.getKeepsByVaultId(vaultId)
    } catch (error) {
      logger.error(error)
    }
  }
}
export const vaultKeepService = new VaultKeepService()
