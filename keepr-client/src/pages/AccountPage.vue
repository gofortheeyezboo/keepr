<template>
  <div class="about text-left">
    <div class="row mt-3" v-if="!state.loading && state.profile">
      <img class="rounded mt-3 ml-4" :src="state.profile.picture" alt="" />
      <div class="col">
        <h1>Welcome {{ state.profile.name }}</h1>
        <h1>Vaults: {{ state.vaults.length }} </h1>
        <h1>Keeps: {{ state.keeps.length }} </h1>
      </div>
    </div>
    <div class="row">
      <h1 class="ml-4 mt-4">
        Vaults <i class="fa fa-plus hover" aria-hidden="true" data-toggle="modal" data-target="#createVault"></i>
      </h1>
    </div>
    <div class="row" v-if="!state.loading && state.vaults">
      <div class="container-fluid">
        <div
          class="modal fade"
          id="createVault"
          tabindex="-1"
          role="dialog"
          aria-labelledby="modalTitleId"
          aria-hidden="true"
        >
          <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title">
                  New Vault
                </h5>
                <button
                  type="button"
                  class="close"
                  data-dismiss="modal"
                  aria-label="Close"
                >
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>
              <div class="modal-body">
                <form @submit.prevent="createVault">
                  <div class="form-group">
                    <label for="title"> Title </label>
                    <input
                      type="text"
                      class="form-control"
                      name="title"
                      id="title"
                      placeholder="Title..."
                      v-model="state.newVault.name"
                    />
                  </div>
                  <div class="form-group">
                    <label for="description"> Description </label>
                    <textarea
                      class="form-control"
                      name="description"
                      id="description"
                      placeholder="Description..."
                      v-model="state.newVault.description"
                      rows="2"
                    ></textarea>
                  </div>
                  <div class="form-check">
                    <input
                      class="form-check-input"
                      type="checkbox"
                      value=""
                      id="defaultCheck1"
                      v-model="state.newVault.isPrivate"
                    />
                    <label class="form-check-label" for="defaultCheck1">
                      Private?
                    </label>
                    <div class="row">
                      <div class="col d-flex justify-content-end">
                        <button type="submit" class="btn btn-primary mt-2">
                          Submit
                        </button>
                      </div>
                    </div>
                  </div>
                </form>
              </div>
            </div>
          </div>
        </div>
      </div>
      <VaultComponent v-for="v in state.vaults" :key="v.id" :vault-prop="v" />
    </div>
    <div class="row">
      <h1 class="ml-4">
        Keeps <i class="fa fa-plus hover" aria-hidden="true" data-toggle="modal" data-target="#createKeep"></i>
      </h1>
    </div>
    <div class="row" v-if="!state.loading && state.keeps">
      <div class="container-fluid">
        <div
          class="modal fade"
          id="createKeep"
          tabindex="-1"
          role="dialog"
          aria-labelledby="modalTitleId"
          aria-hidden="true"
        >
          <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title">
                  New Keep
                </h5>
                <button
                  type="button"
                  class="close"
                  data-dismiss="modal"
                  aria-label="Close"
                >
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>
              <div class="modal-body">
                <form @submit.prevent="createKeep">
                  <div class="form-group">
                    <label for="title"> Title </label>
                    <input
                      type="text"
                      class="form-control"
                      name="title"
                      id="title"
                      placeholder="Title..."
                      v-model="state.newKeep.name"
                    />
                  </div>
                  <div class="form-group">
                    <label for="imgurl"> Image URL </label>
                    <input
                      type="text"
                      class="form-control"
                      name="imgUrl"
                      id="imgUrl"
                      placeholder="Img URL..."
                      v-model="state.newKeep.img"
                    />
                  </div>
                  <div class="form-group">
                    <label for="description"> Description </label>
                    <textarea
                      class="form-control"
                      name="description"
                      id="description"
                      placeholder="Description..."
                      v-model="state.newKeep.description"
                      rows="2"
                    ></textarea>
                  </div>
                  <div class="row">
                    <div class="col d-flex justify-content-end">
                      <button type="submit" class="btn btn-primary mt-2">
                        Submit
                      </button>
                    </div>
                  </div>
                </form>
              </div>
            </div>
          </div>
        </div>
      </div>
      <KeepComponent v-for="k in state.keeps" :key="k.id" :keep-prop="k" />
    </div>
  </div>
</template>

<script>
import { computed, onMounted, reactive } from 'vue'
import { useRoute } from 'vue-router'
import { AppState } from '../AppState'
import { profileService } from '../services/ProfileService'
import { keepService } from '../services/KeepService'
import { vaultService } from '../services/VaultService'
import $ from 'jquery'
import { accountService } from '../services/AccountService'
export default {
  name: 'Profile',
  setup() {
    const route = useRoute()
    const state = reactive({
      account: computed(() => AppState.account),
      user: computed(() => AppState.user),
      profile: computed(() => AppState.activeProfile),
      keeps: computed(() => AppState.keeps),
      vaults: computed(() => AppState.vaults),
      newKeep: {},
      newVault: {},
      loading: true
    })
    onMounted(() => {
      accountService.getAccount()
    })
    onMounted(() => {
      accountService.getKeepsByAccountId(state.account.id)
      state.loading = false
    })
    onMounted(() => {
      accountService.getVaultsByAccountId(state.account.id)
    })
    onMounted(() => {
      profileService.getProfileById(state.account.id)
    })
    return {
      state,
      route,
      async createKeep() {
        await keepService.createKeep(state.newKeep)
        $('#createKeep').modal('hide')
        state.newKeep = {}
      },
      async createVault() {
        await vaultService.createVault(state.newVault)
        $('#createVault').modal('hide')
        state.newVault = {}
      }
    }
  }
}
</script>

<style scoped>
.hover{
  cursor: pointer;
}
img {
  max-width: 100px;
}
</style>
