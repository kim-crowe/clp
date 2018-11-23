import axios from "axios";

export default {
  getGames: function () {
    return axios.post("games/search", { searchText: "", page: 1, itemsPerPage: 20 }).then(r => r.data);
  },
  saveGame: function (name, description, image) {
    let formData = new FormData();
    formData.append("name", name);
    formData.append("description", description);
    formData.append("image", image);

    alert(image);

    return axios.post("games/create",
      formData,
      {
        headers: {
          "Content-Type": "multipart/form-data"
        }
      }
    )
  }
}
