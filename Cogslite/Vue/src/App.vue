<template>
  <div id="app">
    <div class="fixed pin z-50 overflow-auto bg-overlay flex justify-center" v-show="$modal">
      <dynamic :modal="$modal"/>
    </div>
    <div class="bg-cogs-black">
      <nav class="w-full flex items-center justify-between flex-wrap py-2 px-6 container mx-auto">
        <span>
          <router-link to="/" class="no-underline">
            <cogs-glyph/>
          </router-link>
          <router-link
            to="/"
            class="text-cogs-yellow font-semibold text-xl tracking-tight no-underline"
          >Cogslite</router-link>
        </span>
        <span v-if="!isSignedIn">
          <a
            class="btn bg-cogs-grey text-cogs-yellow"
            href="https://cogs.auth.eu-west-2.amazoncognito.com/login?response_type=token&client_id=1bf03fuqd017thrnnej7lcpeb7&redirect_uri=http%3A%2F%2Flocalhost%3A8080"
          >Sign In</a>
        </span>
        <span v-if="isSignedIn">
          <router-link class="mx-2 btn btn-cogs" to="/game/new">G</router-link>
          <drop-down-button :text="profile.userName">
            <div class="bg-white shadow rounded border overflow-hidden">
              <a
                href="#"
                class="no-underline block px-4 py-3 border-b text-grey-darkest bg-white hover:text-white hover:bg-red-darker whitespace-no-wrap"
              >My games</a>
              <a
                href="#"
                class="no-underline block px-4 py-3 border-b text-grey-darkest bg-white hover:text-white hover:bg-red-darker whitespace-no-wrap"
              >My decks</a>
              <a
                href="#"
                class="no-underline block px-4 py-3 border-b text-grey-darkest bg-white hover:text-white hover:bg-red-darker whitespace-no-wrap"
              >Logout</a>
            </div>
          </drop-down-button>
        </span>
      </nav>
    </div>
    <router-view/>
  </div>
</template>

<script>
import profileService from "./services/profileService";
import CogsGlyph from "./components/CogsGlyph";
import DropDownButton from "./components/DropDownButton";
import Dynamic from "./components/Dynamic";

export default {
  name: "App",
  components: { CogsGlyph, DropDownButton, Dynamic },
  mounted: function() {
    profileService.getProfile().then(p => (this.profile = p));
  },
  computed: {
    isSignedIn: function() {
      return this.$auth.isSignedIn();
    }
  },
  data: function() {
    return {
      profile: {}
    };
  }
};
</script>

<style>
@import url("http://fonts.googleapis.com/css?family=Open+Sans:300italic,400italic,700italic,300,400,700");

#app {
  font-family: "Open Sans", sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}

.bg-overlay {
  background-color: rgba(0, 0, 0, 0.4);
}
</style>
