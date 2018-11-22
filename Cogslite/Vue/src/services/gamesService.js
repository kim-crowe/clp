import axios from "axios";

export default {
  getGames: function () {
    return [
      { id: 3, name: "Bloodwars", image: "../images/back.jpg" },
      { id: 4, name: "Magic the gathering", image: "../images/mtg.jpg" },
      { id: 5, name: "Vampire: The Eternal Struggle", image: "../images/vtes.jpg" }
    ]
  },
  getGamesReal: function () {
    return axios.post("games/search", { searchText: "", page: 1, itemsPerPage: 20 }).then(r => r.data);
  },
  saveGame: function (name, description, image) {
    let formData = new FormData();
    formData.append("name", name);
    formData.append("description", description);
    formData.append("image", image);

    alert(image);

    axios.post("games/create",
      formData,
      {
        headers: {
          "Content-Type": "multipart/form-data"
        }
      }
    ).then(function () {
      console.log('SUCCESS!!');
    })
      .catch(function () {
        console.log('FAILURE!!');
      });
  }
}
