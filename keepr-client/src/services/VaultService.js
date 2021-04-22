import { AppState } from '../AppState'
import { api } from './AxiosService'

class VaultService {
  async getVaults() {
    try {
      const res = await api.get('api/vaults')
      console.log(res)
      AppState.vaults = res.data
    } catch (error) {
      console.error(error)
    }
  }

  async getVaultById(id) {
    try {
      const res = await api.get('api/vaults/' + id)
      AppState.activeVault = res.data
    } catch (error) {
      console.error(error)
    }
  }

  async createVault(vault) {
    await api.post('api/vaults', vault)
    this.getVaults()
  }

  async deleteVault(id) {
    await api.delete('api/vaults/' + id)
  }
}

export const vaultService = new VaultService()
