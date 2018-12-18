<template>
  <div id="app">
    <div class="fixed pin z-50 overflow-auto bg-overlay flex justify-center" v-show="$modal">
      <dynamic :modal="$modal"/>
    </div>
    <div class="bg-cogs-primary">
      <nav class="w-full flex items-center justify-between flex-wrap py-2 px-6 container mx-auto">
        <span>
          <router-link to="/" class="no-underline">
            <cogs-glyph/>
          </router-link>
          <router-link
            to="/"
            class="text-cogs-secondary font-semibold text-xl tracking-tight no-underline"
          >CCG Works</router-link>
        </span>
        <span v-if="!isSignedIn">
          <a :href="signInUrl" class="inline-block rounded py-2 px-4 bg-cogs-secondary text-cogs-secondary hover:text-red-lightest no-underline">            
          Sign In</a>
        </span>
        <span v-if="isSignedIn">
          <router-link
            class="mx-2 bg-cogs-alt inline-block rounded py-2 px-4 text-red-darker no-underline"
            to="/game/new"
          ><i class="fas fa-plus"></i></router-link>
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
import LinkButton from "./components/LinkButton";
import CogsGlyph from "./components/CogsGlyph";
import DropDownButton from "./components/DropDownButton";
import Dynamic from "./components/Dynamic";
import * as qs from 'querystring';

export default {
  name: "App",
  components: { CogsGlyph, DropDownButton, Dynamic, LinkButton },
  mounted: function() {    
    profileService.getProfile().then(p => (this.profile = p));
  },
  computed: {
    isSignedIn: function() {
      return this.$auth.isSignedIn();
    },
    signInUrl: function() {            
      var result = "https://cogs.auth.eu-west-2.amazoncognito.com/login?" + qs.stringify(this.oauth);
      return result.replace("__uri", encodeURI(window.location));
    }    
  },
  data: function() {
    return {
      oauth: {
        response_type: "token",
        client_id: "1bf03fuqd017thrnnej7lcpeb7",
        redirect_uri: "__uri"
      },
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
  text-align: left;
  color: #2c3e50;
}

.bg-cogs-primary {
  background-color: #1a0315;
}

.bg-cogs-secondary {
  background-color: #535353;
}

.bg-cogs-alt {
  background-color: #dcae1d;
}

.text-cogs-secondary {
  color: #dcae1d;
}

.text-cogs-primary {
  color: #984b43;
}

.bg-overlay {
  background-color: rgba(0, 0, 0, 0.4);
}
</style>
