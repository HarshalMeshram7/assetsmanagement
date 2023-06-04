/* 
 * Period Common Functions
 * 06-OCT-2022
 */


async function ValidateDtPutoUse(url, vdate) {
    const axios = require('axios').default;
  //  axios.

    axios.get(url)
        .then(function (response) {
            // handle success
            console.log(response);
        })
        .catch(function (error) {
            // handle error
            console.log(error);
        })
        .then(function () {
            // always executed
        });
}