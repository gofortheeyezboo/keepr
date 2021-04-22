<template>
  <div class="home flex-grow-1 container-fluid ">
    <div class="row align-items-center justify-content-center">
      <div class="col-3">
        <h1 class="my-5 bg-dark text-light p-3 rounded d-flex align-items-center text-center">
          <span class="mx-auto text-white">Keeps</span>
        </h1>
      </div>
    </div>
    <hr>
    <div class="row grid">
      <KeepComponent v-for="k in state.keeps" :key="k.id" :keep-prop="k" />
    </div>
  </div>
</template>

<script>
import { computed, onMounted, reactive } from 'vue'
import { AppState } from '../AppState'
import { keepService } from '../services/KeepService'
export default {
  name: 'Home',
  setup() {
    const state = reactive({
      keeps: computed(() => AppState.keeps)
    })
    onMounted(() => keepService.getKeeps())
    return { state }
  }
}
</script>

<style scoped lang="scss">
.home{
  text-align: center;
  user-select: none;
  > img{
    height: 200px;
    width: 200px;
  }
}
.grid-item { width: 200px; }
.grid-item--width2 { width: 400px; }
</style>
