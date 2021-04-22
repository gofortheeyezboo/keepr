<template>
  <header>
    <Navbar />
  </header>
  <main class="bg-color">
    <router-view />
  </main>
  <footer>
    <div class="bg-dark text-light text-center p-4">
      CalQ L8d Industries
    </div>
  </footer>
</template>

<script>
import { computed } from 'vue'
import { AppState } from './AppState'
export default {
  name: 'App',
  setup() {
    return {
      appState: computed(() => AppState)
    }
  }
}
</script>
<style lang="scss">
@import "./assets/scss/main.scss";
.bg-color {
  background-color: darkgrey;
}
</style>
