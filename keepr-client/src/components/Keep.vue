<template>
  <div class="card bg-dark text-white hover" @click="incrementViews">
    <img class="card-img" :src="keepProp.img" alt="Card image">
    <div class="card-img-overlay" data-toggle="modal" :data-target="'#keepModal' + keepProp.id">
      <router-link :to="{ name: 'Profile', params: { id: keepProp.creatorId } }">
        <i class="fa fa-user text-white card-text z" style="position:absolute;bottom:14px;right:20px;" aria-hidden="true"></i>
      </router-link>
      <h5 class="card-body text-center" style="position:absolute;bottom:-18px;left:-3px">
        {{ keepProp.name }}
      </h5>
    </div>
  </div>
  <div class="modal"
       :id="'keepModal' + keepProp.id"
       tabindex="-1"
       role="dialog"
       aria-labelledby="keepModalLabel"
       aria-hidden="true"
  >
    <div class="modal-dialog-md" role="document">
      <div class="modal-content">
        <div class="modal-body">
          <div class="container-fluid">
            <div class="row">
              <button
                style="position:absolute;top:0px;right:5px;"
                class="close"
                data-toggle="modal"
                :data-target="'#keepModal' + keepProp.id"
                aria-label="Close"
              >
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div class="row">
              <img class="img-fluid col-6" :src="keepProp.img" alt="">
              <div class="col-6 d-flex flex-column text-center">
                <small class="mb-2">Keeps: {{ keepProp.keeps }} <i class="fa fa-eye" aria-hidden="true"></i> {{ keepProp.views }} <i class="fa fa-share-alt" aria-hidden="true"></i> {{ keepProp.shares }}</small>
                <h1 class="mt-3">
                  {{ keepProp.name }}
                </h1>
                <hr>
                <p>{{ keepProp.description }}</p>
                <div class="row sticky-bottom text-center mt-auto align-items-baseline justify-content-around">
                  <div class="dropdown">
                    <button class="btn btn-secondary dropdown-toggle m-auto"
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
                              data-toggle="modal"
                              :data-target="'#keepModal' + keepProp.id"
                      >
                        {{ v.name }}
                      </button>
                    </div>
                  </div>
                  <router-link data-toggle="modal" :data-target="'#keepModal' + keepProp.id" :to="{ name: 'Profile', params: { id: keepProp.creatorId } }">
                    <p class="text-dark"><img class="pic" :src="keepProp.creator.picture" alt="">{{ keepProp.creator.name }}</p>
                  </router-link>
                  <i class="fa fa-trash text-danger hover" data-dismiss="modal" aria-hidden="true" v-if="keepProp.creator.email == state.user.email || state.vault.creatorId == state.account.id" @click="deleteKeep"></i>
                </div>
              </div>
            </div>
          </div>
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
import { accountService } from '../services/AccountService'
import $ from 'jquery'
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
      account: computed(() => AppState.account),
      user: computed(() => AppState.user),
      vaults: computed(() => AppState.vaults),
      vault: computed(() => AppState.activeVault),
      newVaultKeep: {
      }
    })
    return {
      route,
      router,
      state,
      getVaultsByAccountId() {
        accountService.getVaultsByAccountId(state.account.id)
      },
      async addToVault(id) {
        state.newVaultKeep.KeepId = props.keepProp.id
        state.newVaultKeep.VaultId = id
        await vaultKeepService.create(state.newVaultKeep)
        this.getVaultsByProfileId(state.account.id)
        this.incrementKeeps()
      },
      async deleteKeep() {
        if (route.name === 'Vault') {
          if (window.confirm('Are You Sure You Want To Remove From Vault?')) {
            await vaultKeepService.deleteKeepFromVault(props.keepProp.vaultKeepId, route.params.id)
            this.getKeepsByAccountId(route.params.id)
          }
        } else if (route.name === 'Profile') {
          if (window.confirm('Are You Sure?')) {
            await keepService.deleteKeep(props.keepProp.id)
            profileService.getKeepsByProfileId(route.params.id)
          }
        } else if (route.name === 'Account') {
          if (window.confirm('Are You Sure?')) {
            await keepService.deleteKeep(props.keepProp.id)
            accountService.getKeepsByAccountId(state.account.id)
          }
        }
      },
      async incrementViews() {
        const tempKeep = props.keepProp
        tempKeep.views++
        await keepService.incrementViews(props.keepProp.id, tempKeep)
      },
      async incrementKeeps() {
        const tempKeep = props.keepProp
        tempKeep.keeps++
        await keepService.incrementKeeps(props.keepProp.id, tempKeep)
      },
      closeModal(id) {
        console.log('closingmodal', id)
        $('#keepModal' + id).modal('hide')
      }
    }
  }
}
</script>

<style scoped>
.hover{
  cursor: pointer;
}
.z{
  z-index: 1000;
}
.pic{
  display: inline-block;
  padding: 8px;
  object-fit: cover;
  margin: auto;
  border-radius: 50%;
  width: 80px;
  height:80px;
  box-shadow: 6px,6px,12px,16px black;
}
</style>
