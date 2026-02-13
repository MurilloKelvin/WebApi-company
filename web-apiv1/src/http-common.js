import Axios from 'axios';

const createAxios = Axios.create({
    baseURL: 'https://localhost:7096',
});

export default createAxios;
