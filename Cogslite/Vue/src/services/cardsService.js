import axios from "axios";

export default {
  uploadCards: function (gameId, cardsPerRow, cardCount, cardSheet) {
    let formData = new FormData();
    formData.append("cardsPerRow", cardsPerRow);
    formData.append("cardCount", cardCount);
    formData.append("cardSheet", cardSheet);

    return axios.post(`game/${gameId}/upload`,
      formData,
      {
        headers: {
          "Content-Type": "multipart/form-data"
        }
      });
  },
  search: function (gameId, search) {
    return axios.post(`games/${gameId}/cards`, search);
  }
}
