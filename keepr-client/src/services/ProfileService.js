import { AppState } from '../AppState'
import { api } from './AxiosService'

class ProfileService {
  async getProfileById(profId) {
    const res = await api.get('api/profiles/' + profId)
    console.log(res.data)
    AppState.activeProfile = res.data
  }

  async getKeepsByProfileId(profId) {
    const res = await api.get('api/profiles/' + profId + '/keeps')
    console.log(res.data)
    AppState.keeps = res.data
  }

  async getVaultsByProfileId(profId) {
    const res = await api.get('api/profiles/' + profId + '/vaults')
    AppState.vaults = res.data
  }
}

export const profileService = new ProfileService()
