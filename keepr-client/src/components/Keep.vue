<template>
  <div class="col-3 card bg-dark text-white hover" data-toggle="modal" :data-target="'#keepModal' + keepProp.id">
    <img class="card-img" :src="keepProp.img" alt="Card image">
    <div class="card-img-overlay">
      <i class="fa fa-user text-white card-text" style="position:absolute;bottom:14px;right:20px;" aria-hidden="true"></i>
      <h5 class="card-title text-center" style="position:absolute;bottom:0">
        {{ keepProp.name }}
      </h5>
    </div>
  </div>
  <div class="modal fade"
       :id="'keepModal' + keepProp.id"
       tabindex="-1"
       role="dialog"
       aria-labelledby="keepModalLabel"
       aria-hidden="true"
  >
    <div class="modal-dialog" role="document">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="keepModalLabel">
            {{ keepProp.name }}
          </h5>
          <button type="button" class="close" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">&times;</span>
          </button>
        </div>
        <div class="modal-body">
          <img class="card-img" :src="keepProp.img" alt="">
          <p class="mt-1">
            {{ keepProp.description }}
            <router-link :to="{ name: 'Profile', params: { id: keepProp.creatorId } }">
              <i class="fa fa-user text-dark card-text float-right" aria-hidden="true" data-dismiss="modal"></i>
            </router-link>
          </p>
        </div>
        <div class="modal-footer justify-content-between" v-if="keepProp.creator">
          <div class="dropdown">
            <button class="btn btn-secondary dropdown-toggle"
                    type="button"
                    id="dropdownMenuButton"
                    data-toggle="dropdown"
                    aria-haspopup="true"
                    aria-expanded="false"
                    @click="getVaultsByProfileId"
            >
              Add to Vault
            </button>
            <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">
              <button class="dropdown-item"
                      @click="addToVault(v.id)"
                      v-for="v in state.vaults"
                      :key="v.id"
                      :vault-prop="v"
                      href="#"
                      data-dismiss="modal"
              >
                {{ v.name }}
              </button>
            </div>
          </div>
          <img :src="keepProp.creator.picture" alt="">
          <p>{{ keepProp.creator.name }}</p>
          <i class="fa fa-trash text-danger" data-dismiss="modal" aria-hidden="true" v-if="keepProp.creator.email == state.user.email" @click="deleteKeep"></i>
          <button type="button" class="btn btn-secondary" data-dismiss="modal">
            Close
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { reactive, computed } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { AppState } from '../AppState'
import { vaultKeepService } from '../services/VaultKeepService'
import { profileService } from '../services/ProfileService'
import { keepService } from '../services/KeepService'
export default {
  name: 'KeepComponent',
  props: {
    keepProp: {
      type: Object,
      required: true
    }
  },
  setup(props) {
    const route = useRoute()
    const router = useRouter()
    const state = reactive({
      user: computed(() => AppState.user),
      vaults: computed(() => AppState.vaults),
      newVaultKeep: {
      }
    })
    return {
      route,
      router,
      state,
      getVaultsByProfileId() {
        profileService.getVaultsByProfileId(props.keepProp.creator.id)
      },
      async addToVault(id) {
        state.newVaultKeep.KeepId = props.keepProp.id
        state.newVaultKeep.VaultId = id
        await vaultKeepService.create(state.newVaultKeep)
      },
      deleteKeep() {
        if (route.name === 'Vault') {
          if (window.confirm('Are You Sure You Want To Remove From Vault?')) {
            vaultKeepService.deleteKeepFromVault(props.keepProp.vaultKeepId, route.params.id)
          }
        } else {
          if (window.confirm('Are You Sure?')) {
            console.log(route)
            keepService.deleteKeep(props.keepProp.id)
          }
        }
      }
    }
  }
}
</script>

<style scoped>
.hover{
  cursor: pointer;
}
</style>
