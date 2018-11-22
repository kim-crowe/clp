import queryString from 'query-string';
import axios from 'axios';

const tokenKey = "id_token";

export default {

  install(Vue, options) {

    const parsedHash = queryString.parse(location.hash);
    console.log(parsedHash);

    var id_token = sessionStorage.getItem(tokenKey);
    if (parsedHash !== null && parsedHash[tokenKey]) {
      id_token = parsedHash[tokenKey];
      sessionStorage.setItem(tokenKey, id_token);
    }

    Vue.prototype.$auth = {
      id_token: id_token,
      isSignedIn: () => id_token !== null,
    }

    axios.defaults.headers.common["Authorization"] = "Bearer " + id_token;

    console.log(id_token);
  }
}
