import axios from "axios";

export default {
  getProfile: function () {
    return axios.get("profile").then(r => r.data);
  }
}
